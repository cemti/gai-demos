using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
using Stockfish.NET.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessAgent;

partial class MainForm
{
    private const string NoUserInputMessage = "No input by user.";

    private const string StockfishPath = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";

    private readonly Stockfish.NET.Core.Stockfish _engine = new(StockfishPath, 1)
    {
        SkillLevel = 0
    };

    // https://lichess.org/page/accuracy#first-compute-win
    private static double GetWinPercent(Evaluation evaluation)
    {
        static double GetValue(Evaluation evaluation) => evaluation switch
        {
            { Type: "mate", Value: > 0 } => 1000,
            { Type: "mate", Value: < 0 } => -1000,
            { Type: "mate" } => double.NegativeInfinity,
            { Value: var value } => value
        };

        return 100 / (1 + Math.Exp(-0.00368208 * GetValue(evaluation)));
    }

    private async Task Step(CancellationToken token)
    {
        for (; ; )
        {
            try
            {
                await RegisterMove(Color.White, token);
                await RegisterMove(Color.Black, token);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message != NoUserInputMessage)
                {
                    _ = MessageBox.Show(ex.Message);
                }

                break;
            }
            catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
            {
                break;
            }

            if (!cbLoop.Checked)
            {
                break;
            }
        }
    }

    private async Task RegisterMove(Color color, CancellationToken token)
    {
        StepTelemetry telemetry;
        _stopwatch.Restart();

        try
        {
            telemetry = await MakeMove(color, token);
        }
        finally
        {
            _stopwatch.Stop();
        }

        ShowStopwatch($"{telemetry.WinPercentage:0.##}% win rate for {color}");

        _telemetry.Add(telemetry);
        var count = _telemetry.Count;

        if (count > 1)
        {
            txtMoves.Text += ", ";
        }

        txtMoves.Text += $"{count}. {telemetry.Move}";
        txtMoves.SelectionStart = int.MaxValue;
        txtMoves.ScrollToCaret();
    }

    private async Task<StepTelemetry> MakeMove(Color color, CancellationToken token)
    {
        var maxAttempts = int.Parse(attemptsToolStripMenuItem.Text);
        var cbModel = color == Color.White ? cbModelWhite : cbModelBlack;

        if (_replayTelemetry is not null)
        {
            if (!_replayIterator.MoveNext())
            {
                throw new InvalidOperationException("End of replay.");
            }

            cbModel.Text = _replayIterator.Current.Model;
        }

        var model = cbModel.Text;

        for (; ; )
        {
            var bestMove = await Task.Run(() => _engine.GetBestMoveTime(500))
                           ?? throw new InvalidOperationException("Game over.");

            HashSet<string> invalidMoves = [];

            for (int i = 1; i <= maxAttempts; ++i)
            {
                string move;

                try
                {
                    move = await InputMove(color, model, invalidMoves, token);
                }
                catch (FormatException ex)
                {
                    ShowStopwatch(ex.Message);
                    continue;
                }

                token.ThrowIfCancellationRequested();

                var (isLegalMove, isElimination) = await MovePiece(move);

                if (!isLegalMove)
                {
                    ShowStopwatch($"invalid move: {move}");
                    _ = invalidMoves.Add(move);
                    continue;
                }

                var evaluation = await Task.Run(() => _engine.GetEvaluation(100));

                // GetEvaluation inverts for Black
                if (color == Color.White)
                {
                    evaluation.Value = -evaluation.Value;
                }

                var winPercent = 100 - GetWinPercent(evaluation);
                return new(model, new(move, isElimination), i, invalidMoves, winPercent, _stopwatch.Elapsed);
            }

            model = "Stockfish";
        }
    }

    private async Task<string> InputMove(Color color, string model, ICollection<string> invalidMoves, CancellationToken token)
    {
        if (_replayTelemetry is not null)
        {
            if (invalidMoves.Count > 0)
            {
                throw new InvalidOperationException("Invalid move to replay.");
            }

            return _replayIterator.Current.Move.RawMove;
        }

        switch (model)
        {
            case "Manual":
                var move = Interaction.InputBox("Input move:", "Manual input");

                if (move == "")
                {
                    throw new InvalidOperationException(NoUserInputMessage);
                }

                return move;

            case "Stockfish":
                return await Task.Run(_engine.GetBestMove);

            default:
                Dictionary<string, object> options;

                using (MemoryStream memoryStream = new())
                {
                    await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, memoryStream);
                    options = GetModelOptions(color, model, invalidMoves, memoryStream);
                }

                return (await CallLLMAsync(options, token)).Replace("x", "");
        }
    }

    private static bool IsElimination(string fenBefore, string fenAfter)
    {
        var counts = new[] { fenBefore, fenAfter }.Select(x => x.Split(' ')[0].Trim().Count(char.IsLetter)).ToArray();
        return counts[0] > counts[1];
    }

    private async Task<(bool IsMoveLegal, bool IsElimination)> MovePiece(string move)
    {
        var (isMoveLegal, isElimination, currentPosition) = await Task.Run(() =>
        {
            var prevPosition = _engine.GetFenPosition();

            _engine.SetPosition([.. _telemetry.Select(x => x.Move.RawMove).Append(move)]);

            var currentPosition = _engine.GetFenPosition();

            return currentPosition == prevPosition
                   ? (false, false, currentPosition)
                   : (true, IsElimination(prevPosition, currentPosition), currentPosition);
        });

        if (isMoveLegal)
        {
            _ = await webView21.ExecuteScriptAsync($"setPosition('{currentPosition}');");

            if (highlightMovesToolStripMenuItem.Checked)
            {
                _ = await webView21.ExecuteScriptAsync($@"
highlightElement('{move[..2]}');
highlightElement('{move[2..]}');
");
            }
        }

        return (isMoveLegal, isElimination);
    }
}

using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
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
    private const string StockfishPath = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";
    private const int MaxAttempts = 8;

    private readonly Stockfish.NET.Core.Stockfish _engine = new(StockfishPath, 1)
    {
        SkillLevel = 0
    };

    private async Task Step(CancellationToken token)
    {
        for (; ; )
        {
            try
            {
                await RegisterMove(cbModelWhite.Text, token);
                await RegisterMove(cbModelBlack.Text, token);
            }
            catch (InvalidOperationException ex)
            {
                _ = MessageBox.Show(ex.Message);
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

    private async Task RegisterMove(string model, CancellationToken token)
    {
        StepTelemetry telemetry;
        _stopwatch.Restart();

        try
        {
            telemetry = await MakeMove(model, token);
        }
        finally
        {
            _stopwatch.Stop();
            ShowStopwatch();
        }

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

    private async Task<StepTelemetry> MakeMove(string model, CancellationToken token)
    {
        for (; ; )
        {
            var bestMove = await Task.Run(() => _engine.GetBestMoveTime(500))
                           ?? throw new InvalidOperationException("Game over.");

            HashSet<string> invalidMoves = [];

            for (int i = 1; i <= MaxAttempts; ++i)
            {
                string move;

                try
                {
                    move = await InputMove(model, invalidMoves, token);
                }
                catch (FormatException ex)
                {
                    ShowStopwatch(ex.Message);
                    continue;
                }

                token.ThrowIfCancellationRequested();

                var (isLegalMove, isElimination) = await MovePiece(move);

                if (isLegalMove)
                {
                    return new(model, new(move, isElimination), i, invalidMoves, _stopwatch.Elapsed);
                }

                ShowStopwatch($"invalid move: {move}");
                _ = invalidMoves.Add(move);
            }

            model = "Stockfish";
        }
    }

    private async Task<string> InputMove(string model, ICollection<string> invalidMoves, CancellationToken token)
    {
        switch (model)
        {
            case "Manual" when _replayTelemetry is not null:
                if (invalidMoves.Count > 0)
                {
                    throw new InvalidOperationException("Invalid move to replay.");
                }

                if (!_replayIterator.MoveNext())
                {
                    throw new InvalidOperationException("End of replay.");
                }

                return _replayIterator.Current.Move.RawMove;

            case "Manual":
                return await Task.Run(() => Interaction.InputBox("Input move:", "Manual input"));

            case "Stockfish":
                return await Task.Run(_engine.GetBestMove);

            default:
                Dictionary<string, object> payload;

                using (MemoryStream memoryStream = new())
                {
                    await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, memoryStream);
                    payload = GetPayload(model, invalidMoves, memoryStream);
                }

                return (await CallLLMAsync(payload, token)).Replace("x", "");
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
        }

        return (isMoveLegal, isElimination);
    }
}

using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessAgent;

public partial class MainForm : Form
{
    private const int MaxAttempts = 8;

    private const string StockfishPath = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";

    private static readonly string[] Models = ["qwen2.5vl:32b-q8_0", "gemma3:27b-it-q8_0", "qwen2.5vl:32b", "gemma3:27b", "llama3.2-vision", "Stockfish", "Manual"];

    private readonly Stopwatch _stopwatch = new();

    private readonly Stockfish.NET.Core.Stockfish _engine = new(StockfishPath, 1)
    {
        SkillLevel = 0
    };

    private readonly List<StepTelemetry> _telemetry = [];

    private CancellationTokenSource _cancellationTokenSource = new();

    public MainForm()
    {
        InitializeComponent();
        cbModelWhite.DataSource = new BindingSource(Models, "");
        cbModelBlack.DataSource = new BindingSource(Models, "");
        webView21.Source = new(Path.GetFullPath("chessboard.html"));
    }

    private void ShowStopwatch(string input = null)
    {
        timeElapsedLabel.Text = _stopwatch.ToString();

        if (input is not null)
        {
            timeElapsedLabel.Text += $" ({input})";
        }
    }

    private void SetBusy(bool busy)
    {
        btnReset.Enabled = !busy;
        btnStep.Text = busy ? "Cancel" : "Step";
    }

    private string GenerateSystemPrompt(ICollection<string> invalidMoves)
    {
        var isWhite = (_telemetry.Count & 1) == 0;
        var player = isWhite ? "White" : "Black";
        var opponent = isWhite ? "Black" : "White";
        var exampleMove = isWhite ? "e2e4" : "e7e5";

        var prompt = @$"You are playing as {player} in a chess game.

Your task is to make a move that defeats {opponent} so that your king will not be in check.

The attached image shows the current board state.

Respond using Long Algebraic Notation only, four characters.

Example: {exampleMove}

Answer: <original file><original rank><destination file><destination rank>";

        if (_telemetry.Count > 0)
        {
            var query = from pair in _telemetry.Index()
                        where (pair.Index & 1) == (isWhite ? 0 : 1)
                        select pair.Item.Move;

            prompt += $"\n\nYour last moves are: {string.Join(", ", query)}";
        }

        if (invalidMoves.Count > 0)
        {
            prompt += $"\n\nDo not respond with one of these moves: {string.Join(", ", invalidMoves)}";
        }

        return prompt;
    }

    private Dictionary<string, object> GetPayload(string model, ICollection<string> invalidMoves, MemoryStream ms)
    {
        string[] images = [Convert.ToBase64String(ms.ToArray())];
        Random rnd = new();

        return new()
        {
            { "model", model },
            { "system", GenerateSystemPrompt(invalidMoves) },
            { "prompt", "Move." },
            { "images", images },
            { "options", new
            {
                seed = rnd.Next(),
                temperature = rnd.NextDouble()
            } },
            { "stream", false }
        };
    }

    private async void BtnStep_Click(object sender, EventArgs e)
    {
        if (btnStep.Text == "Cancel")
        {
            _cancellationTokenSource.Cancel();

            while (btnStep.Text == "Cancel")
            {
                await Task.Delay(10);
            }

            _cancellationTokenSource = new();
            return;
        }

        SetBusy(true);

        var token = _cancellationTokenSource.Token;

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

        SetBusy(false);
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

    private static async Task<string> CallLLMAsync(IReadOnlyDictionary<string, object> payload, CancellationToken cancellationToken)
    {
        using HttpClient client = new()
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:11434/api/generate", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new FormatException($"LLM Error: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        var move = doc.RootElement.GetProperty("response").GetString();

        if (LanRegex().Matches(move) is [.., { Value: var value }])
        {
            return value;
        }

        return move;
    }

    [GeneratedRegex("([a-h][1-8]){2}")]
    private static partial Regex LanRegex();

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

    private async void BtnReset_Click(object sender, EventArgs e)
    {
        _telemetry.Clear();
        txtMoves.Clear();
        _engine.SetPosition();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");
    }
}

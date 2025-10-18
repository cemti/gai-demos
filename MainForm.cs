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
    private static string GenerateSystemPrompt(bool isWhite)
    {
        var player = isWhite ? "White" : "Black";
        var opponent = isWhite ? "Black" : "White";
        var exampleMove = isWhite ? "e2e4" : "e7e5";

        return @$"You are playing as {player} in a chess game.

Your task is to make a move that defeats {opponent} so that your king will not be in check.

The attached image shows the current board state.

Respond using Long Algebraic Notation only, four characters.

Example: {exampleMove}

Answer: <original file><original rank><destination file><destination rank>";
    }

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
        UseWaitCursor = busy;
        btnReset.Enabled = !busy;
        btnStep.Text = busy ? "Cancel" : "Step";
    }

    private Dictionary<string, object> GetPayload(string model, ICollection<string> invalidMoves, MemoryStream ms)
    {
        int isBlack = _telemetry.Count & 1;
        var prompt = "Move.";

        if (invalidMoves.Count > 0)
        {
            prompt += $"\n\nDo not respond with one of these moves: {string.Join(", ", invalidMoves)}";
        }

        if (_telemetry.Count > 0)
        {
            var query = from pair in _telemetry.Index()
                        where ((pair.Index & 1) ^ isBlack) == 0
                        select pair.Item.Move;

            prompt += $"\n\nYour last moves are: {string.Join(", ", query)}";
        }

        string[] images = [Convert.ToBase64String(ms.ToArray())];

        Random rnd = new();

        Dictionary<string, object> payload = new()
        {
            { "model", model },
            { "system", GenerateSystemPrompt(isBlack == 0) },
            { "prompt", prompt },
            { "images", images },
            { "options", new
            {
                seed = rnd.Next(),
                temperature = rnd.NextDouble()
            } },
            { "stream", false }
        };

        return payload;
    }

    private async void BtnStep_Click(object sender, EventArgs e)
    {
        var token = _cancellationTokenSource.Token;

        if (btnStep.Text == "Cancel")
        {
            _cancellationTokenSource.Cancel();
            return;
        }

        await Step(token);
        _cancellationTokenSource = new();
    }

    private async Task Step(CancellationToken token)
    {
        SetBusy(true);

        try
        {
            await RegisterMove(cbModelWhite.Text, token);
            await RegisterMove(cbModelBlack.Text, token);
        }
        catch (InvalidOperationException ex)
        {
            _ = MessageBox.Show(ex.Message);
            return;
        }
        catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
        {
            return;
        }
        finally
        {
            SetBusy(false);
        }

        if (cbLoop.Checked)
        {
            btnStep.PerformClick();
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
        if (_engine.GetBestMoveTime(500) == null)
        {
            throw new InvalidOperationException("Game over.");
        }

        HashSet<string> invalidMoves = [];

        for (; ; )
        {
            var move = await InputMove(model, invalidMoves, token);

            if (MovePiece(move, out var isElimination))
            {
                return new(new(move, isElimination), invalidMoves, _stopwatch.Elapsed);
            }

            ShowStopwatch($"invalid move: {move}");
            _ = invalidMoves.Add(move);
        }
    }

    private async Task<string> InputMove(string model, ICollection<string> invalidMoves, CancellationToken token)
    {
        switch (model)
        {
            case "Manual":
                return Interaction.InputBox("Input move:", "Manual input");

            case "Stockfish":
                return _engine.GetBestMove();

            default:
                Dictionary<string, object> payload;

                using (MemoryStream memoryStream = new())
                {
                    await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, memoryStream);
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
            return "LLM Error: " + response.ReasonPhrase;
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

    private bool MovePiece(string move, out bool isElimination)
    {
        isElimination = false;

        if (move is not null)
        {
            var prevPosition = _engine.GetFenPosition();

            _engine.SetPosition([.. _telemetry.Select(x => x.Move.RawMove).Append(move)]);

            var currentPosition = _engine.GetFenPosition();

            if (currentPosition == prevPosition)
            {
                return false;
            }

            isElimination = IsElimination(prevPosition, currentPosition);
            _ = webView21.ExecuteScriptAsync($"setPosition('{currentPosition}');");
            return true;
        }

        return false;
    }

    private async void BtnReset_Click(object sender, EventArgs e)
    {
        _telemetry.Clear();
        txtMoves.Clear();
        _engine.SetPosition();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");
    }
}

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
    private const string SystemPrompt = @"You are playing as White in a chess game.

Your task is to make a move that defeats Black so that your king will not be in check.

The attached image shows the current board state.

Respond using Long Algebraic Notation only, four characters.

Example: e2e4

Answer: <original file><original rank><destination file><destination rank>";

    private const string StockfishPath = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";

    private readonly Stopwatch _stopwatch = new();
    private readonly Stockfish.NET.Core.Stockfish _engine = new(StockfishPath, 1)
    {
        SkillLevel = 0
    };

    private readonly List<ChessMove> _moves = [];
    private readonly List<StepTelemetry> _telemetry = [];

    private CancellationTokenSource _cancellationTokenSource = new();

    public MainForm()
    {
        InitializeComponent();
        modelComboBox.DataSource = new[] { "qwen2.5vl:32b-q8_0", "gemma3:27b-it-q8_0", "qwen2.5vl:32b", "gemma3:27b", "llama3.2-vision", "Manual" };
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

        if (busy)
        {
            _stopwatch.Start();
            return;
        }

        ShowStopwatch();
        _stopwatch.Reset();
    }

    private Dictionary<string, object> GetPayload(MemoryStream ms)
    {
        var prompt = "Move.";

        if (_telemetry[^1].InvalidMoves is { Count: > 0 } invalidMoves)
        {
            prompt += $"\n\nDo not respond with one of these moves: {string.Join(", ", invalidMoves)}";
        }

        if (_moves.Count > 0)
        {
            prompt += $"\n\nYour last moves are: {string.Join(", ", _moves.Where((x, i) => (i & 1) == 0).Select(x => x.Move))}";
        }

        string[] images = [Convert.ToBase64String(ms.ToArray())];

        Random rnd = new();

        Dictionary<string, object> payload = new()
        {
            { "model", modelComboBox.Text },
            { "system", SystemPrompt },
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
            if (_engine.GetBestMoveTime(500) == null)
            {
                _ = MessageBox.Show("Game over.");
                cbLoop.Checked = false;
                return;
            }

            StepTelemetry current = new();
            _telemetry.Add(current);

            for (; ; )
            {
                var move = await InputMove(token);

                if (MovePiece(move))
                {
                    current.TimeTaken = _stopwatch.Elapsed;
                    break;
                }

                ShowStopwatch($"invalid move: {move}");
                _ = current.InvalidMoves.Add(move);
            }

            if (!MovePiece(_engine.GetBestMove()))
            {
                cbLoop.Checked = false;
            }
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

    private async Task<string> InputMove(CancellationToken token)
    {
        string move = null;

        if (modelComboBox.Text == "Manual")
        {
            move = Interaction.InputBox("Input move:", "Manual input");
        }
        else
        {
            Dictionary<string, object> payload;

            using (MemoryStream memoryStream = new())
            {
                await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, memoryStream);
                payload = GetPayload(memoryStream);
            }

            try
            {
                move = await CallLLMAsync(payload, token);
            }
            catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
            {

            }
        }

        move = move.Replace("x", "");
        return move;
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

    private bool MovePiece(string move)
    {
        if (move is not null)
        {
            var prevPosition = _engine.GetFenPosition();

            _engine.SetPosition([.. _moves.Select(x => x.Move).Append(move)]);

            var currentPosition = _engine.GetFenPosition();

            if (currentPosition == prevPosition)
            {
                return false;
            }

            bool isElimination = IsElimination(prevPosition, currentPosition);
            _moves.Add(new(move, isElimination));

            _ = webView21.ExecuteScriptAsync($"setPosition('{currentPosition}');");
            txtMoves.Text = string.Join(", ", _moves.Index().Select(x => $"{x.Index + 1}. {x.Item}"));
            return true;
        }

        return false;
    }

    private async void BtnReset_Click(object sender, EventArgs e)
    {
        _moves.Clear();
        _telemetry.Clear();
        txtMoves.Clear();
        _engine.SetPosition();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");
    }
}

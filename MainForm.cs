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

namespace WindowsErrorAnalyzer;

public partial class MainForm : Form
{
    private const string SystemPrompt = @"# Description
You are playing as White in a chess game. Your task is to make a move that defeats Black.

# Task
Do not repeat any previous moves. Your response must be a new move by White.

Respond using Long Algebraic Notation (LAN) only - no words, no punctuation, no commentary.

The attached image shows the current board state.

Example: e2e4

Answer: <original file><original rank><destination file><destination rank>

# Obligations

Before answering, verify that the move is legal to perform on the chessboard.";

    private const string StockfishPath = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";

    private readonly Stopwatch _stopwatch = new();
    private readonly Stockfish.NET.Core.Stockfish _engine = new(StockfishPath);
    private string[] _moves = [];

    public MainForm()
    {
        InitializeComponent();
        modelComboBox.DataSource = new[] { "qwen2.5vl:32b", "gemma3:27b", "llama3.2-vision" };
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
        btnStep.Enabled = !busy;

        if (busy)
        {
            _stopwatch.Start();
            return;
        }

        ShowStopwatch();
        _stopwatch.Reset();
    }

    private Dictionary<string, object> GetPayload(MemoryStream ms, IEnumerable<string> invalidMoves)
    {
        var prompt = "";
        /*
        if (_moves.Length > 0)
        {
            prompt += $"\n\nBe informed of this game history: [{string.Join(", ", _moves)}]";
        }
        */
        if (invalidMoves.Any())
        {
            prompt += $"\n\nThe following moves are illegal (DO NOT USE THEM): {string.Join(", ", invalidMoves)}";
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
                temperature = 0
            } },
            { "stream", false }
        };

        return payload;
    }

    private async void BtnStep_Click(object sender, EventArgs e)
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

            string move = null;
            HashSet<string> invalidMoves = [];

            for (; ; )
            {
                Dictionary<string, object> payload;

                using (MemoryStream memoryStream = new())
                {
                    await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, memoryStream);
                    payload = GetPayload(memoryStream, invalidMoves);
                }

                move = (await CallLLMAsync(payload)).Replace("x", "");

                if (await MovePiece(move))
                {
                    break;
                }

                _ = invalidMoves.Add(move);
                ShowStopwatch($"invalid move: {move}");
            }

            if (!await MovePiece(_engine.GetBestMove()))
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

    private static async Task<string> CallLLMAsync(IReadOnlyDictionary<string, object> payload)
    {
        using HttpClient client = new()
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:11434/api/generate", content);

        if (!response.IsSuccessStatusCode)
        {
            return "LLM Error: " + response.ReasonPhrase;
        }

        var json = await response.Content.ReadAsStringAsync();
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

    private async Task<bool> MovePiece(string move)
    {
        if (move is not null)
        {
            var prevPosition = _engine.GetFenPosition();

            string[] tempMoves = [.. _moves, move];
            _engine.SetPosition(tempMoves);

            var currentPosition = _engine.GetFenPosition();

            if (currentPosition == prevPosition)
            {
                return false;
            }

            _moves = tempMoves;
            _ = await webView21.ExecuteScriptAsync($"setPosition('{currentPosition}');");
            txtMoves.Text = string.Join(", ", _moves.Index().Select(x => $"{x.Index + 1}. {x.Item}"));
            return true;
        }

        return false;
    }

    private async void BtnReset_Click(object sender, EventArgs e)
    {
        _moves = [];
        txtMoves.Clear();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");
    }
}

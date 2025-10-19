using Stockfish.NET.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ChessAgent;

partial class MainForm
{
    private const string OllamaUrl = "http://localhost:11434/api/generate";

    private string GenerateSystemPrompt(Color color, ICollection<string> invalidMoves)
    {
        var isWhite = color == Color.White;
        var player = isWhite ? "White" : "Black";
        var opponent = isWhite ? "Black" : "White";
        var exampleMove = isWhite ? "e2e4" : "e7e5";

        var prompt = @$"You are playing as {player} in a chess game.

Your task is to make a move that defeats {opponent} so that your king will not be in check.

The attached image shows the current board state.

Respond using Long Algebraic Notation only, four characters.

Example: {exampleMove}

Answer: <original file><original rank><destination file><destination rank>";

        if (recallPastMovesToolStripMenuItem.Checked && _telemetry.Count > 0)
        {
            var query = from pair in _telemetry.Index()
                        where (pair.Index & 1) == (isWhite ? 0 : 1)
                        select pair.Item.Move;

            prompt += $"\n\nYour last moves are: {string.Join(", ", query)}";
        }

        if (recallPastOpponentMovesToolStripMenuItem.Checked && _telemetry.Count > 0)
        {
            var query = from pair in _telemetry.Index()
                        where (pair.Index & 1) == (isWhite ? 1 : 0)
                        select pair.Item.Move;

            prompt += $"\n\nYour opponent's last moves are: {string.Join(", ", query)}";
        }

        if (recallIllegalMovesToolStripMenuItem.Checked && invalidMoves.Count > 0)
        {
            prompt += $"\n\nDo not respond with one of these moves: {string.Join(", ", invalidMoves)}";
        }

        return prompt;
    }

    private Dictionary<string, object> GetModelOptions(Color color, string model, ICollection<string> invalidMoves, MemoryStream ms)
    {
        string[] images = [Convert.ToBase64String(ms.ToArray())];
        Random rnd = new();

        return new()
        {
            { "model", model },
            { "system", GenerateSystemPrompt(color, invalidMoves) },
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

    private static async Task<string> CallLLMAsync(IReadOnlyDictionary<string, object> options, CancellationToken cancellationToken)
    {
        using HttpClient client = new()
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        var content = new StringContent(JsonSerializer.Serialize(options), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(OllamaUrl, content, cancellationToken);

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
}

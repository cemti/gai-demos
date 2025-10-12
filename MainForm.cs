using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsErrorAnalyzer;

public partial class MainForm : Form
{
    private readonly Stopwatch _stopwatch = new();

    public MainForm()
    {
        InitializeComponent();
        modelComboBox.DataSource = new[] { "granite3.2-vision", "llama3.2-vision" };
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        btnAsk.Enabled = !busy;
        btnExplain.Enabled = !busy;

        if (busy)
        {
            _stopwatch.Start();
            timeElapsedLabel.Text = "";
        }
        else
        {
            _stopwatch.Stop();
            timeElapsedLabel.Text = _stopwatch.ToString();
            _stopwatch.Reset();
        }
    }

    private void BtnLoadImage_Click(object sender, EventArgs e)
    {
        using OpenFileDialog ofd = new();
        ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = Image.FromFile(ofd.FileName);
        }
    }

    private Dictionary<string, object> GetExplainPayload()
    {
        string[] images = null;
        var prompt = "Explain this Windows error message";

        if (modelComboBox.Text.Contains("vision"))
        {
            using MemoryStream ms = new();
            pictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            images = [Convert.ToBase64String(ms.ToArray())];
        }
        else
        {
            prompt += $":\n{txtExtracted.Text}";
        }

        Dictionary<string, object> payload = new()
        {
            { "model", modelComboBox.Text },
            { "prompt", prompt },
            { "stream", false }
        };

        if (images is not null)
        {
            payload.Add("images", images);
        }

        return payload;
    }

    private async void BtnExplain_Click(object sender, EventArgs e)
    {
        txtAnswer.Clear();
        SetBusy(true);
        txtExplaination.Text = await CallLLMAsync(GetExplainPayload());
        SetBusy(false);
    }

    private async void BtnAsk_Click(object sender, EventArgs e)
    {
        var extracted = txtExtracted.Text;
        var question = txtQuestion.Text;

        Dictionary<string, object> payload = new()
        {
            { "model", modelComboBox.Text },
            { "prompt", @$"Based on this error message:
{extracted}
Answer this question:
{question}" },
            { "stream", false }
        };

        SetBusy(true);
        txtAnswer.Text = await CallLLMAsync(payload);
        SetBusy(false);
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
        return doc.RootElement.GetProperty("response").GetString().Preprocess();
    }

    private void ModelComboBox_SelectionChangeCommitted(object sender, EventArgs e)
    {
        txtExtracted.Enabled = !modelComboBox.Text.Contains("vision");
    }
}

file static class StringExtensions
{
    public static string Preprocess(this string input) => input.ReplaceLineEndings().Trim();
}

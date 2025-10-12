using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsErrorAnalyzer;

public partial class MainForm : Form
{
    private const string Datapath = @"C:\Users\Cristian\AppData\Local\Programs\Tesseract-OCR\tessdata";
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
            var (text, path) = RunOCR(ofd.FileName);
            txtExtracted.Text = text.Preprocess();
            pictureBox.Image = Image.FromFile(path);
        }
    }

    private static string PreprocessAndCrop(string imagePath)
    {
        using var src = Cv2.ImRead(imagePath, ImreadModes.Color);

        using var gray = new Mat();
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

        using var thresh = new Mat();
        Cv2.Threshold(gray, thresh, 200, 255, ThresholdTypes.BinaryInv);
        Cv2.FindContours(thresh, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

        var largestContour = contours.OrderByDescending(x => Cv2.ContourArea(x)).FirstOrDefault();

        if (largestContour is not [_, ..])
        {
            return imagePath;
        }

        var rect = Cv2.BoundingRect(largestContour);

        // Add padding but exclude window frame/title/buttons
        rect.X += 10;
        rect.Y += 40;  // skip title bar
        rect.Width -= 20;
        rect.Height -= 80; // skip button area

        if (rect is { X: < 0 } or { Y: < 0 } or { Width: <= 0 } or { Height: <= 0 })
        {
            return imagePath;
        }

        string tempFile = Path.Combine(Path.GetTempPath(), "cropped.png");
        using var roi = new Mat(src, rect);
        Cv2.ImWrite(tempFile, roi);
        return tempFile;
    }

    private static (string Text, string TempPath) RunOCR(string imagePath)
    {
        try
        {
            string croppedPath = PreprocessAndCrop(imagePath);

            using var engine = new TesseractEngine(Datapath, "eng", EngineMode.Default);
            using var img = Pix.LoadFromFile(croppedPath);
            using var page = engine.Process(img);
            return (page.GetText(), croppedPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show("OCR Error: " + ex.Message);
            return ("", "");
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

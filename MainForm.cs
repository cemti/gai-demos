using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsErrorAnalyzer;

public partial class MainForm : Form
{
    private const string Datapath = @"C:\Users\Cristian\AppData\Local\Programs\Tesseract-OCR\tessdata";

    public MainForm()
    {
        InitializeComponent();
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


    private async void BtnSummarize_Click(object sender, EventArgs e)
    {
        var extractedText = txtExtracted.Text;
        
        if (!string.IsNullOrWhiteSpace(extractedText))
        {
            string summary = await CallLLMAsync(@$"Summarize this Windows error message:
{extractedText}");
            txtSummary.Text = summary.Preprocess();
        }
    }

    private async void BtnAsk_Click(object sender, EventArgs e)
    {
        var extractedText = txtExtracted.Text;
        string question = txtQuestion.Text;

        if (!string.IsNullOrWhiteSpace(question))
        {
            string answer = await CallLLMAsync(@$"Based on this error:
{extractedText}
Answer this question:
{question}");
            txtAnswer.Text = answer.Preprocess();
        }
    }

    private static async Task<string> CallLLMAsync(string prompt)
    {
        using var client = new HttpClient();

        var payload = new
        {
            model = "llama3.1:8b",
            prompt,
            stream = false
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:11434/api/generate", content);

        if (!response.IsSuccessStatusCode)
        {
            return "LLM Error: " + response.ReasonPhrase;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("response").GetString();
    }
}

file static class StringExtensions
{
    public static string Preprocess(this string input) => input.ReplaceLineEndings().Trim();
}

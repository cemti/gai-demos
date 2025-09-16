using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsErrorAnalyzer
{
    public partial class MainForm : Form
    {
        private const string Datapath = @"C:\Users\Cristian\AppData\Local\Programs\Tesseract-OCR\tessdata";
        private string extractedText = "";

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
                pictureBox.Image = Image.FromFile(ofd.FileName);
                extractedText = RunOCR(ofd.FileName);
                txtExtracted.Text = extractedText;
            }
        }

        private static string RunOCR(string imagePath)
        {
            try
            {
                using var engine = new TesseractEngine(Datapath, "eng", EngineMode.Default);
                using var img = Pix.LoadFromFile(imagePath);
                using var page = engine.Process(img);
                return page.GetText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("OCR Error: " + ex.Message);
                return "";
            }
        }

        private async void BtnSummarize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return;
            }

            string summary = await CallLLMAsync(@$"Summarize this Windows error message:
{extractedText}");
            txtSummary.Text = summary;
        }

        private async void BtnAsk_Click(object sender, EventArgs e)
        {
            string question = txtQuestion.Text;

            if (string.IsNullOrWhiteSpace(question))
            {
                return;
            }

            string answer = await CallLLMAsync(@$"Based on this error:
{extractedText}
Answer this question:
{question}");
            txtAnswer.Text = answer;
        }

        private static async Task<string> CallLLMAsync(string prompt)
        {
            using var client = new HttpClient();
            var payload = new
            {
                model = "llama3",
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
}

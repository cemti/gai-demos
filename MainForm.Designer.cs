
namespace WindowsErrorAnalyzer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.TextBox txtExtracted;
        private System.Windows.Forms.Button btnSummarize;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.TextBox txtQuestion;
        private System.Windows.Forms.Button btnAsk;
        private System.Windows.Forms.TextBox txtAnswer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.txtExtracted = new System.Windows.Forms.TextBox();
            this.btnSummarize = new System.Windows.Forms.Button();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.txtQuestion = new System.Windows.Forms.TextBox();
            this.btnAsk = new System.Windows.Forms.Button();
            this.txtAnswer = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            // pictureBox
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Size = new System.Drawing.Size(300, 200);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // btnLoadImage
            this.btnLoadImage.Location = new System.Drawing.Point(330, 12);
            this.btnLoadImage.Size = new System.Drawing.Size(120, 30);
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.Click += new System.EventHandler(this.BtnLoadImage_Click);

            // txtExtracted
            this.txtExtracted.Location = new System.Drawing.Point(12, 220);
            this.txtExtracted.Multiline = true;
            this.txtExtracted.Size = new System.Drawing.Size(438, 60);

            // btnSummarize
            this.btnSummarize.Location = new System.Drawing.Point(12, 290);
            this.btnSummarize.Size = new System.Drawing.Size(120, 30);
            this.btnSummarize.Text = "Summarize";
            this.btnSummarize.Click += new System.EventHandler(this.BtnSummarize_Click);

            // txtSummary
            this.txtSummary.Location = new System.Drawing.Point(12, 330);
            this.txtSummary.Multiline = true;
            this.txtSummary.Size = new System.Drawing.Size(438, 60);

            // txtQuestion
            this.txtQuestion.Location = new System.Drawing.Point(12, 400);
            this.txtQuestion.Size = new System.Drawing.Size(438, 23);

            // btnAsk
            this.btnAsk.Location = new System.Drawing.Point(12, 430);
            this.btnAsk.Size = new System.Drawing.Size(120, 30);
            this.btnAsk.Text = "Ask";
            this.btnAsk.Click += new System.EventHandler(this.BtnAsk_Click);

            // txtAnswer
            this.txtAnswer.Location = new System.Drawing.Point(12, 470);
            this.txtAnswer.Multiline = true;
            this.txtAnswer.Size = new System.Drawing.Size(438, 60);

            // MainForm
            this.ClientSize = new System.Drawing.Size(470, 540);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.txtExtracted);
            this.Controls.Add(this.btnSummarize);
            this.Controls.Add(this.txtSummary);
            this.Controls.Add(this.txtQuestion);
            this.Controls.Add(this.btnAsk);
            this.Controls.Add(this.txtAnswer);
            this.Text = "Windows Error Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

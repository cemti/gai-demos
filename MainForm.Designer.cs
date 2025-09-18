
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
            pictureBox = new System.Windows.Forms.PictureBox();
            btnLoadImage = new System.Windows.Forms.Button();
            txtExtracted = new System.Windows.Forms.TextBox();
            btnSummarize = new System.Windows.Forms.Button();
            txtSummary = new System.Windows.Forms.TextBox();
            txtQuestion = new System.Windows.Forms.TextBox();
            btnAsk = new System.Windows.Forms.Button();
            txtAnswer = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new System.Drawing.Point(12, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(300, 200);
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new System.Drawing.Point(330, 12);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new System.Drawing.Size(120, 30);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Load Image";
            btnLoadImage.Click += BtnLoadImage_Click;
            // 
            // txtExtracted
            // 
            txtExtracted.Location = new System.Drawing.Point(12, 220);
            txtExtracted.Multiline = true;
            txtExtracted.Name = "txtExtracted";
            txtExtracted.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtExtracted.Size = new System.Drawing.Size(438, 60);
            txtExtracted.TabIndex = 2;
            // 
            // btnSummarize
            // 
            btnSummarize.Location = new System.Drawing.Point(12, 290);
            btnSummarize.Name = "btnSummarize";
            btnSummarize.Size = new System.Drawing.Size(120, 30);
            btnSummarize.TabIndex = 3;
            btnSummarize.Text = "Summarize";
            btnSummarize.Click += BtnSummarize_Click;
            // 
            // txtSummary
            // 
            txtSummary.Location = new System.Drawing.Point(12, 330);
            txtSummary.Multiline = true;
            txtSummary.Name = "txtSummary";
            txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtSummary.Size = new System.Drawing.Size(438, 60);
            txtSummary.TabIndex = 4;
            // 
            // txtQuestion
            // 
            txtQuestion.Location = new System.Drawing.Point(12, 400);
            txtQuestion.Name = "txtQuestion";
            txtQuestion.Size = new System.Drawing.Size(438, 23);
            txtQuestion.TabIndex = 5;
            // 
            // btnAsk
            // 
            btnAsk.Location = new System.Drawing.Point(12, 430);
            btnAsk.Name = "btnAsk";
            btnAsk.Size = new System.Drawing.Size(120, 30);
            btnAsk.TabIndex = 6;
            btnAsk.Text = "Ask";
            btnAsk.Click += BtnAsk_Click;
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new System.Drawing.Point(12, 470);
            txtAnswer.Multiline = true;
            txtAnswer.Name = "txtAnswer";
            txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtAnswer.Size = new System.Drawing.Size(438, 60);
            txtAnswer.TabIndex = 7;
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(470, 540);
            Controls.Add(pictureBox);
            Controls.Add(btnLoadImage);
            Controls.Add(txtExtracted);
            Controls.Add(btnSummarize);
            Controls.Add(txtSummary);
            Controls.Add(txtQuestion);
            Controls.Add(btnAsk);
            Controls.Add(txtAnswer);
            Name = "MainForm";
            Text = "Windows Error Analyzer";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

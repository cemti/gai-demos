
namespace WindowsErrorAnalyzer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.TextBox txtExtracted;
        private System.Windows.Forms.Button btnExplain;
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
            btnExplain = new System.Windows.Forms.Button();
            txtExplaination = new System.Windows.Forms.TextBox();
            txtQuestion = new System.Windows.Forms.TextBox();
            btnAsk = new System.Windows.Forms.Button();
            txtAnswer = new System.Windows.Forms.TextBox();
            modelComboBox = new System.Windows.Forms.ComboBox();
            timeElapsedLabel = new System.Windows.Forms.Label();
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
            txtExtracted.Size = new System.Drawing.Size(438, 100);
            txtExtracted.TabIndex = 2;
            // 
            // btnExplain
            // 
            btnExplain.Location = new System.Drawing.Point(12, 326);
            btnExplain.Name = "btnExplain";
            btnExplain.Size = new System.Drawing.Size(120, 30);
            btnExplain.TabIndex = 3;
            btnExplain.Text = "Explain";
            btnExplain.Click += BtnExplain_Click;
            // 
            // txtExplaination
            // 
            txtExplaination.Location = new System.Drawing.Point(12, 362);
            txtExplaination.Multiline = true;
            txtExplaination.Name = "txtExplaination";
            txtExplaination.ReadOnly = true;
            txtExplaination.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtExplaination.Size = new System.Drawing.Size(438, 100);
            txtExplaination.TabIndex = 4;
            // 
            // txtQuestion
            // 
            txtQuestion.Location = new System.Drawing.Point(12, 468);
            txtQuestion.Name = "txtQuestion";
            txtQuestion.Size = new System.Drawing.Size(438, 23);
            txtQuestion.TabIndex = 5;
            // 
            // btnAsk
            // 
            btnAsk.Enabled = false;
            btnAsk.Location = new System.Drawing.Point(12, 497);
            btnAsk.Name = "btnAsk";
            btnAsk.Size = new System.Drawing.Size(120, 30);
            btnAsk.TabIndex = 6;
            btnAsk.Text = "Ask";
            btnAsk.Click += BtnAsk_Click;
            // 
            // txtAnswer
            // 
            txtAnswer.Location = new System.Drawing.Point(12, 533);
            txtAnswer.Multiline = true;
            txtAnswer.Name = "txtAnswer";
            txtAnswer.ReadOnly = true;
            txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtAnswer.Size = new System.Drawing.Size(438, 100);
            txtAnswer.TabIndex = 7;
            // 
            // modelComboBox
            // 
            modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new System.Drawing.Point(138, 331);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new System.Drawing.Size(121, 23);
            modelComboBox.TabIndex = 8;
            modelComboBox.SelectionChangeCommitted += ModelComboBox_SelectionChangeCommitted;
            // 
            // timeElapsedLabel
            // 
            timeElapsedLabel.AutoSize = true;
            timeElapsedLabel.Location = new System.Drawing.Point(265, 334);
            timeElapsedLabel.Name = "timeElapsedLabel";
            timeElapsedLabel.Size = new System.Drawing.Size(0, 15);
            timeElapsedLabel.TabIndex = 9;
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(470, 643);
            Controls.Add(timeElapsedLabel);
            Controls.Add(modelComboBox);
            Controls.Add(pictureBox);
            Controls.Add(btnLoadImage);
            Controls.Add(txtExtracted);
            Controls.Add(btnExplain);
            Controls.Add(txtExplaination);
            Controls.Add(txtQuestion);
            Controls.Add(btnAsk);
            Controls.Add(txtAnswer);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Windows Error Analyzer";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.ComboBox modelComboBox;
        private System.Windows.Forms.TextBox txtExplaination;
        private System.Windows.Forms.Label timeElapsedLabel;
    }
}


namespace WindowsErrorAnalyzer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnScreenshot;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Button btnExecute;

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
            btnScreenshot = new System.Windows.Forms.Button();
            txtPrompt = new System.Windows.Forms.TextBox();
            btnExecute = new System.Windows.Forms.Button();
            txtResponse = new System.Windows.Forms.TextBox();
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
            // btnScreenshot
            // 
            btnScreenshot.Location = new System.Drawing.Point(330, 12);
            btnScreenshot.Name = "btnScreenshot";
            btnScreenshot.Size = new System.Drawing.Size(120, 30);
            btnScreenshot.TabIndex = 1;
            btnScreenshot.Text = "Screenshot";
            btnScreenshot.Click += BtnLoadImage_Click;
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new System.Drawing.Point(12, 220);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtPrompt.Size = new System.Drawing.Size(438, 100);
            txtPrompt.TabIndex = 2;
            // 
            // btnExecute
            // 
            btnExecute.Location = new System.Drawing.Point(12, 326);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new System.Drawing.Size(120, 30);
            btnExecute.TabIndex = 3;
            btnExecute.Text = "Execute";
            btnExecute.Click += BtnExecute_Click;
            // 
            // txtResponse
            // 
            txtResponse.Location = new System.Drawing.Point(12, 362);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.ReadOnly = true;
            txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtResponse.Size = new System.Drawing.Size(438, 100);
            txtResponse.TabIndex = 4;
            // 
            // modelComboBox
            // 
            modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new System.Drawing.Point(138, 331);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new System.Drawing.Size(121, 23);
            modelComboBox.TabIndex = 8;
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
            ClientSize = new System.Drawing.Size(470, 477);
            Controls.Add(timeElapsedLabel);
            Controls.Add(modelComboBox);
            Controls.Add(pictureBox);
            Controls.Add(btnScreenshot);
            Controls.Add(txtPrompt);
            Controls.Add(btnExecute);
            Controls.Add(txtResponse);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Windows Agent";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.ComboBox modelComboBox;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label timeElapsedLabel;
    }
}

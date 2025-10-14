
namespace ChessAgent
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStep;

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
            btnStep = new System.Windows.Forms.Button();
            txtMoves = new System.Windows.Forms.TextBox();
            modelComboBox = new System.Windows.Forms.ComboBox();
            timeElapsedLabel = new System.Windows.Forms.Label();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnReset = new System.Windows.Forms.Button();
            cbLoop = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // btnStep
            // 
            btnStep.Location = new System.Drawing.Point(12, 635);
            btnStep.Name = "btnStep";
            btnStep.Size = new System.Drawing.Size(75, 23);
            btnStep.TabIndex = 3;
            btnStep.Text = "Step";
            btnStep.Click += BtnStep_Click;
            // 
            // txtMoves
            // 
            txtMoves.Location = new System.Drawing.Point(12, 665);
            txtMoves.Multiline = true;
            txtMoves.Name = "txtMoves";
            txtMoves.ReadOnly = true;
            txtMoves.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtMoves.Size = new System.Drawing.Size(609, 100);
            txtMoves.TabIndex = 4;
            // 
            // modelComboBox
            // 
            modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new System.Drawing.Point(93, 636);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new System.Drawing.Size(121, 23);
            modelComboBox.TabIndex = 8;
            // 
            // timeElapsedLabel
            // 
            timeElapsedLabel.AutoSize = true;
            timeElapsedLabel.Location = new System.Drawing.Point(220, 643);
            timeElapsedLabel.Name = "timeElapsedLabel";
            timeElapsedLabel.Size = new System.Drawing.Size(0, 15);
            timeElapsedLabel.TabIndex = 9;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            webView21.Location = new System.Drawing.Point(12, 12);
            webView21.Name = "webView21";
            webView21.Size = new System.Drawing.Size(609, 617);
            webView21.TabIndex = 10;
            webView21.ZoomFactor = 1D;
            // 
            // btnReset
            // 
            btnReset.Location = new System.Drawing.Point(546, 636);
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(75, 23);
            btnReset.TabIndex = 11;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // cbLoop
            // 
            cbLoop.AutoSize = true;
            cbLoop.Location = new System.Drawing.Point(487, 639);
            cbLoop.Name = "cbLoop";
            cbLoop.Size = new System.Drawing.Size(53, 19);
            cbLoop.TabIndex = 12;
            cbLoop.Text = "Loop";
            cbLoop.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(633, 782);
            Controls.Add(cbLoop);
            Controls.Add(btnReset);
            Controls.Add(webView21);
            Controls.Add(timeElapsedLabel);
            Controls.Add(modelComboBox);
            Controls.Add(btnStep);
            Controls.Add(txtMoves);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Chess Agent";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.ComboBox modelComboBox;
        private System.Windows.Forms.TextBox txtMoves;
        private System.Windows.Forms.Label timeElapsedLabel;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbLoop;
    }
}

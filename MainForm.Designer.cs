
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
            cbModelWhite = new System.Windows.Forms.ComboBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnReset = new System.Windows.Forms.Button();
            cbLoop = new System.Windows.Forms.CheckBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            timeElapsedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            cbModelBlack = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            statusStrip1.SuspendLayout();
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
            txtMoves.Size = new System.Drawing.Size(609, 99);
            txtMoves.TabIndex = 4;
            // 
            // cbModelWhite
            // 
            cbModelWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbModelWhite.FormattingEnabled = true;
            cbModelWhite.Location = new System.Drawing.Point(140, 635);
            cbModelWhite.Name = "cbModelWhite";
            cbModelWhite.Size = new System.Drawing.Size(121, 23);
            cbModelWhite.TabIndex = 8;
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
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { timeElapsedLabel });
            statusStrip1.Location = new System.Drawing.Point(0, 767);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(633, 22);
            statusStrip1.TabIndex = 13;
            statusStrip1.Text = "statusStrip1";
            // 
            // timeElapsedLabel
            // 
            timeElapsedLabel.Name = "timeElapsedLabel";
            timeElapsedLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // cbModelBlack
            // 
            cbModelBlack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbModelBlack.FormattingEnabled = true;
            cbModelBlack.Location = new System.Drawing.Point(311, 635);
            cbModelBlack.Name = "cbModelBlack";
            cbModelBlack.Size = new System.Drawing.Size(121, 23);
            cbModelBlack.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(93, 640);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(41, 15);
            label1.TabIndex = 14;
            label1.Text = "White:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(267, 640);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 15);
            label2.TabIndex = 14;
            label2.Text = "Black:";
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(633, 789);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Controls.Add(cbLoop);
            Controls.Add(btnReset);
            Controls.Add(webView21);
            Controls.Add(cbModelBlack);
            Controls.Add(cbModelWhite);
            Controls.Add(btnStep);
            Controls.Add(txtMoves);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Chess Agent";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.ComboBox cbModelWhite;
        private System.Windows.Forms.TextBox txtMoves;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbLoop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timeElapsedLabel;
        private System.Windows.Forms.ComboBox cbModelBlack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

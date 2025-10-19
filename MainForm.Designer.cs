
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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            telemetryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            replayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            unloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            promptingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            recallPastMovesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            recallIllegalMovesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            maximumAttemptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            attemptsToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            recallPastOpponentMovesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStep
            // 
            btnStep.Location = new System.Drawing.Point(12, 650);
            btnStep.Name = "btnStep";
            btnStep.Size = new System.Drawing.Size(75, 23);
            btnStep.TabIndex = 3;
            btnStep.Text = "Step";
            btnStep.Click += BtnStep_Click;
            // 
            // txtMoves
            // 
            txtMoves.Location = new System.Drawing.Point(12, 680);
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
            cbModelWhite.Location = new System.Drawing.Point(140, 650);
            cbModelWhite.Name = "cbModelWhite";
            cbModelWhite.Size = new System.Drawing.Size(121, 23);
            cbModelWhite.TabIndex = 8;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            webView21.Enabled = false;
            webView21.Location = new System.Drawing.Point(12, 27);
            webView21.Name = "webView21";
            webView21.Size = new System.Drawing.Size(609, 617);
            webView21.TabIndex = 10;
            webView21.ZoomFactor = 1D;
            // 
            // btnReset
            // 
            btnReset.Location = new System.Drawing.Point(546, 651);
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
            cbLoop.Location = new System.Drawing.Point(487, 654);
            cbLoop.Name = "cbLoop";
            cbLoop.Size = new System.Drawing.Size(53, 19);
            cbLoop.TabIndex = 12;
            cbLoop.Text = "Loop";
            cbLoop.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { timeElapsedLabel });
            statusStrip1.Location = new System.Drawing.Point(0, 782);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(633, 22);
            statusStrip1.SizingGrip = false;
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
            cbModelBlack.Location = new System.Drawing.Point(311, 650);
            cbModelBlack.Name = "cbModelBlack";
            cbModelBlack.Size = new System.Drawing.Size(121, 23);
            cbModelBlack.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(93, 655);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(41, 15);
            label1.TabIndex = 14;
            label1.Text = "White:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(267, 655);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 15);
            label2.TabIndex = 14;
            label2.Text = "Black:";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { telemetryToolStripMenuItem, promptingToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(633, 24);
            menuStrip1.TabIndex = 15;
            menuStrip1.Text = "menuStrip1";
            // 
            // telemetryToolStripMenuItem
            // 
            telemetryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { saveToolStripMenuItem, replayToolStripMenuItem });
            telemetryToolStripMenuItem.Name = "telemetryToolStripMenuItem";
            telemetryToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            telemetryToolStripMenuItem.Text = "Telemetry";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // replayToolStripMenuItem
            // 
            replayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadToolStripMenuItem, unloadToolStripMenuItem });
            replayToolStripMenuItem.Name = "replayToolStripMenuItem";
            replayToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            replayToolStripMenuItem.Text = "Replay";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += LoadToolStripMenuItem_Click;
            // 
            // unloadToolStripMenuItem
            // 
            unloadToolStripMenuItem.Name = "unloadToolStripMenuItem";
            unloadToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            unloadToolStripMenuItem.Text = "Unload";
            unloadToolStripMenuItem.Click += UnloadToolStripMenuItem_Click;
            // 
            // promptingToolStripMenuItem
            // 
            promptingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { recallPastMovesToolStripMenuItem, recallIllegalMovesToolStripMenuItem, recallPastOpponentMovesToolStripMenuItem, maximumAttemptsToolStripMenuItem });
            promptingToolStripMenuItem.Name = "promptingToolStripMenuItem";
            promptingToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            promptingToolStripMenuItem.Text = "Prompting";
            // 
            // recallPastMovesToolStripMenuItem
            // 
            recallPastMovesToolStripMenuItem.Checked = true;
            recallPastMovesToolStripMenuItem.CheckOnClick = true;
            recallPastMovesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            recallPastMovesToolStripMenuItem.Name = "recallPastMovesToolStripMenuItem";
            recallPastMovesToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            recallPastMovesToolStripMenuItem.Text = "Recall past moves";
            // 
            // recallIllegalMovesToolStripMenuItem
            // 
            recallIllegalMovesToolStripMenuItem.Checked = true;
            recallIllegalMovesToolStripMenuItem.CheckOnClick = true;
            recallIllegalMovesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            recallIllegalMovesToolStripMenuItem.Name = "recallIllegalMovesToolStripMenuItem";
            recallIllegalMovesToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            recallIllegalMovesToolStripMenuItem.Text = "Recall illegal moves per step";
            // 
            // maximumAttemptsToolStripMenuItem
            // 
            maximumAttemptsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { attemptsToolStripMenuItem });
            maximumAttemptsToolStripMenuItem.Name = "maximumAttemptsToolStripMenuItem";
            maximumAttemptsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            maximumAttemptsToolStripMenuItem.Text = "Maximum attempts";
            // 
            // attemptsToolStripMenuItem
            // 
            attemptsToolStripMenuItem.Name = "attemptsToolStripMenuItem";
            attemptsToolStripMenuItem.Size = new System.Drawing.Size(180, 23);
            attemptsToolStripMenuItem.Text = "8";
            attemptsToolStripMenuItem.TextChanged += AttemptsToolStripMenuItem_TextChanged;
            // 
            // recallPastOpponentMovesToolStripMenuItem
            // 
            recallPastOpponentMovesToolStripMenuItem.CheckOnClick = true;
            recallPastOpponentMovesToolStripMenuItem.Name = "recallPastOpponentMovesToolStripMenuItem";
            recallPastOpponentMovesToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            recallPastOpponentMovesToolStripMenuItem.Text = "Recall past opponent moves";
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(633, 804);
            Controls.Add(menuStrip1);
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
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Chess Agent";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem telemetryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem promptingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recallPastMovesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recallIllegalMovesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maximumAttemptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox attemptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recallPastOpponentMovesToolStripMenuItem;
    }
}

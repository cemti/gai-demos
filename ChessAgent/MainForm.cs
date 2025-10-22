using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessAgent;

public partial class MainForm : Form
{
    private static readonly string[] Models = ["qwen2.5vl:32b-q8_0", "gemma3:27b-it-q8_0", "Stockfish", "Manual"];

    private Stockfish.NET.Core.Stockfish _engine;

    private readonly List<StepTelemetry> _telemetry = [];

    private StepTelemetry[] _replayTelemetry;
    private IEnumerator<StepTelemetry> _replayIterator;

    private readonly Stopwatch _stopwatch = new();

    private CancellationTokenSource _cancellationTokenSource = new();

    public MainForm()
    {
        InitializeComponent();
        cbModelWhite.DataSource = new BindingSource(Models, "");
        cbModelBlack.DataSource = new BindingSource(Models, "");
        webView21.Source = new(Path.GetFullPath("chessboard.html"));

        stockfishPathTextBoxToolStripMenuItem.Text = @"D:\Stockfish\stockfish-windows-x86-64-bmi2.exe";
        ollamaEndpointTextBoxToolStripMenuItem.Text = "http://localhost:11434";
    }

    private void EnsureStockfish()
    {
        if (_engine is not null)
        {
            return;
        }

        var path = stockfishPathTextBoxToolStripMenuItem.Text;

        if (!File.Exists(path))
        {
            throw new InvalidOperationException("Invalid path for the Stockfish executable.");
        }

        UseWaitCursor = true;
        stockfishPathTextBoxToolStripMenuItem.Enabled = false;

        _engine = new(path, 1)
        {
            SkillLevel = 0
        };

        UseWaitCursor = false;
    }

    private void ShowStopwatch(string input = null)
    {
        timeElapsedLabel.Text = _stopwatch.ToString();

        if (input is not null)
        {
            timeElapsedLabel.Text += $" ({input})";
        }
    }

    private void SetBusy(bool busy)
    {
        btnReset.Enabled = !busy;
        replayToolStripMenuItem.Enabled = !busy;
        btnStep.Text = busy ? "Cancel" : "Step";
    }

    private async void BtnStep_Click(object sender, EventArgs e)
    {
        if (btnStep.Text == "Cancel")
        {
            _cancellationTokenSource.Cancel();

            while (btnStep.Text == "Cancel")
            {
                await Task.Delay(10);
            }

            _cancellationTokenSource = new();
            return;
        }

        EnsureStockfish();

        SetBusy(true);
        await Step(_cancellationTokenSource.Token);
        SetBusy(false);
    }

    private async void BtnReset_Click(object sender, EventArgs e)
    {
        if (_replayTelemetry is not null)
        {
            _replayIterator = _replayTelemetry.AsEnumerable().GetEnumerator();
        }

        timeElapsedLabel.Text = "";
        _telemetry.Clear();
        txtMoves.Clear();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");

        if (_engine is not null)
        {
            stockfishPathTextBoxToolStripMenuItem.Enabled = true;
            _engine.Dispose();
            _engine = null;
        }
    }
}

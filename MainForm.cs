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

        _telemetry.Clear();
        txtMoves.Clear();
        _engine.SetPosition();
        _ = await webView21.ExecuteScriptAsync("resetBoard();");
    }
}

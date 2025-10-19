using System;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace ChessAgent;

partial class MainForm
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using SaveFileDialog saveFileDialog = new()
        {
            Title = "Save telemetry data",
            Filter = "JSON file (*.json)|*.json"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            using var stream = saveFileDialog.OpenFile();
            JsonSerializer.Serialize(stream, _telemetry, SerializerOptions);
        }
    }

    private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Title = "Replay telemetry",
            Filter = "JSON file (*.json)|*.json"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            using var stream = openFileDialog.OpenFile();

            _replayTelemetry = JsonSerializer.Deserialize<StepTelemetry[]>(stream);
            _replayIterator = _replayTelemetry.AsEnumerable().GetEnumerator();

            cbModelWhite.Text = "Manual";
            cbModelWhite.Enabled = false;

            cbModelBlack.Text = "Manual";
            cbModelBlack.Enabled = false;
        }
    }

    private void UnloadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _replayTelemetry = null;
        _replayIterator = null;

        cbModelWhite.Enabled = true;
        cbModelBlack.Enabled = true;
    }

    private void AttemptsToolStripMenuItem_TextChanged(object sender, EventArgs e)
    {
        var text = attemptsToolStripMenuItem.Text;

        if (text != "" && !(int.TryParse(text, out var result) && result > 0))
        {
            attemptsToolStripMenuItem.Text = "8";
        }
    }
}

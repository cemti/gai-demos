using System;
using System.Collections.Generic;

namespace WindowsErrorAnalyzer;

internal class StepTelemetry
{
    public HashSet<string> InvalidMoves { get; } = [];
    public TimeSpan TimeTaken { get; set; } = TimeSpan.Zero;
}

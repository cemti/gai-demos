using System;
using System.Collections.Generic;

namespace ChessAgent;

internal class StepTelemetry
{
    public HashSet<string> InvalidMoves { get; } = [];
    public TimeSpan TimeTaken { get; set; } = TimeSpan.Zero;
}

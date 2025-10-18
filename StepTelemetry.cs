using System;
using System.Collections.Generic;

namespace ChessAgent;

internal readonly record struct StepTelemetry(ChessMove Move, HashSet<string> InvalidMoves, TimeSpan TimeTaken);

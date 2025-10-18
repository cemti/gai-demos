using System;
using System.Collections.Generic;

namespace ChessAgent;

internal readonly record struct StepTelemetry(string Model, ChessMove Move, int AttemptCount, HashSet<string> InvalidMoves, TimeSpan TimeTaken);

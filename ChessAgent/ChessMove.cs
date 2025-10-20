namespace ChessAgent;

internal readonly record struct ChessMove(string RawMove, bool IsElimination)
{
    public override string ToString() => IsElimination ? RawMove[..2] + 'x' + RawMove[2..] : RawMove;
}

namespace WindowsErrorAnalyzer;

internal readonly record struct ChessMove(string Move, bool IsElimination)
{
    public override string ToString() => IsElimination ? Move[..2] + 'x' + Move[2..] : Move;
}

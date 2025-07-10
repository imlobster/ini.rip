namespace INIRIP.Models
{
    public readonly record struct Token(TokenKind Kind, int Start, int Length);
}

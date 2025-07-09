namespace MINI.Models
{
    public enum TokenKind : byte
    {
        Unknown,
        Section,
        Literal,
        EqualSign,
        OpenBracketsSign,
        CloseBracketsSign,
        EOL,
    }
}

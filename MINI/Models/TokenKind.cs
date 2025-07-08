namespace MINI.Models
{
    public enum TokenKind : byte
    {
        Unknown,
        Comment,
        Identifier,
        NumericalValue,
        LiteralValue,
        BooleanValue,
        EqualSign,
        OpenBracketsSign,
        CloseBracketsSign,
    }
}

using INIRIP.Analysis;
using INIRIP.Models;

namespace INIRIP
{
    public static class Encoder
    {
        private static readonly Lexer lexer = new();
        private static readonly Parser parser = new();

        public static bool TryDecode
        (
            string source,

            out Dictionary<
                    ReadOnlyMemory<char>,
                    Dictionary<
                        ReadOnlyMemory<char>,
                        ReadOnlyMemory<char>
                    >
            > outvals
        )
        {
            if ( source == null ) { outvals = []; return false; }
            if (!lexer.TryLexText(source.AsSpan(), out ReadOnlySpan<INIRIP.Models.Token> tokens)) { outvals = []; return false; }
            if (!parser.TryParseTokens(source, tokens, out outvals)) { outvals = []; return false; }
            return true;
        }
    }
}

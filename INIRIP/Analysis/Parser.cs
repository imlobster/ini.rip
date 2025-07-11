using INIRIP.Models;

namespace INIRIP.Analysis
{
    public class Parser
    {
        private Dictionary<
                    ReadOnlyMemory<char>,
                    Dictionary<
                        ReadOnlyMemory<char>,
                        ReadOnlyMemory<char>
                    >
            > values
        = [];

        private int secinf_start, secinf_length;

        private int pointerPosition = 0;
        private Token current = new();

        public bool TryParseTokens
        (
            string source,
            ReadOnlySpan<Token> intokens,
            out Dictionary<
                    ReadOnlyMemory<char>,
                    Dictionary<
                        ReadOnlyMemory<char>,
                        ReadOnlyMemory<char>
                    >
            > outvals
        )
        {
            values.Clear();
            pointerPosition = 0;

            while(pointerPosition < intokens.Length)
            {
                current = intokens[pointerPosition];

//#if DEBUG
//                Console.WriteLine($"current: {{{source.Substring(current.Start, current.Length)}}} with kind {current.Kind}");
//#endif

                switch (current.Kind)
                {
                    case TokenKind.Section:
//#if DEBUG
//                        Console.WriteLine($"INMAINCYCLE\tsection, {source.Substring(current.Start, current.Length)}");
//#endif
                        BuildSection(source, intokens);
                        break;
                    default:
//#if DEBUG
//                        Console.WriteLine($"INMAINCYCLE\ttoken, {source.Substring(current.Start, current.Length)}");
//#endif
                        pointerPosition++;
                        break;
                }
            }

            if (values.Count < 1) { outvals = []; return false; }

            outvals = values;
            return true;
        }

        private void SkipToTheNextLine(ReadOnlySpan<Token> intokens)
        {
            while(pointerPosition < intokens.Length)
            {
//#if DEBUG
//                Console.WriteLine($"\tINSKIPTOTHENEXTLINE\ttoken, {pointerPosition}");
//#endif
                current = intokens[pointerPosition];

                if(current.Kind != TokenKind.EOL)
                {
                    pointerPosition++;
                    continue;
                }

                pointerPosition++;
                break;
            }
        }

#region Builders
        private void BuildSection(string source, ReadOnlySpan<Token> intokens)
        {
//#if DEBUG
//            Console.WriteLine($"\tINBUILDSECTION\tchecking ???: {{{source.Substring(current.Start, current.Length)}}} with kind {current.Kind}");
//#endif

            if (values.ContainsKey(source.AsMemory(current.Start, current.Length))) { SkipToTheNextLine(intokens); return; }

            secinf_start = current.Start;
            secinf_length = current.Length;

            Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> dictbuffer = [];

            SkipToTheNextLine(intokens);

            while (pointerPosition < intokens.Length)
            {
                current = intokens[pointerPosition];
//#if DEBUG
//                Console.WriteLine($"\tINBUILDSECTION\tcurrent !!!: {{{source.Substring(current.Start, current.Length)}}} with kind {current.Kind}");
//#endif

                if (current.Kind != TokenKind.Literal) { break; }

                BuildValue(source, intokens, ref dictbuffer);
            }

//#if DEBUG
//            Console.WriteLine($"\tINBUILDSECTION\tadding section: {{{source.Substring(secinf_start, secinf_length)}}}");
//#endif

            if (!values.TryAdd(source.AsMemory(secinf_start, secinf_length), dictbuffer))
            {
//#if DEBUG
//                Console.WriteLine($"\tINBUILDSECTION\tcant add !!!: {{{source.Substring(secinf_start, secinf_length)}}} with kind {current.Kind}");
//#endif
                return; 
            } 
        }

        private void BuildValue(string source, ReadOnlySpan<Token> intokens, ref Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> dictbuffer)
        {
            while (pointerPosition < intokens.Length)
            {
                current = intokens[pointerPosition];

//#if DEBUG
//                Console.WriteLine($"\tINBUILDSECTION\tcurrent: {{{source.Substring(current.Start, current.Length)}}} with kind {current.Kind}");
//#endif

                if (current.Kind == TokenKind.EOL)
                {
                    pointerPosition++;
                    continue;
                }

                break;
            }

//#if DEBUG
//            Console.WriteLine($"\tINBUILDVALUE\tmaybe literal?, {source.Substring(current.Start, current.Length)}");
//#endif
            if (current.Kind != TokenKind.Literal) { SkipToTheNextLine(intokens); return; }
            pointerPosition++;
            current = intokens[pointerPosition];
//#if DEBUG
//            Console.WriteLine($"\tINBUILDVALUE\tmaybe equal?, {source.Substring(current.Start, current.Length)}");
//#endif
            if (pointerPosition < intokens.Length && current.Kind != TokenKind.EqualSign) { SkipToTheNextLine(intokens); return; }
            pointerPosition++;
            current = intokens[pointerPosition];
//#if DEBUG
//            Console.WriteLine($"\tINBUILDVALUE\tmaybe literal?, {source.Substring(current.Start, current.Length)}");
//#endif
            if (pointerPosition < intokens.Length && current.Kind != TokenKind.Literal) { SkipToTheNextLine(intokens); return; }
            if (dictbuffer.ContainsKey(source.AsMemory(intokens[pointerPosition-2].Start, intokens[pointerPosition-2].Length))) { SkipToTheNextLine(intokens); return; }

            dictbuffer.TryAdd
                    (
                        source.AsMemory
                            (
                                intokens[pointerPosition - 2].Start,
                                intokens[pointerPosition - 2].Length
                            ),
                        source.AsMemory(current.Start, current.Length)
                    );

            SkipToTheNextLine(intokens); return;
        }
#endregion
    }
}
using INIRIP.Models;
using System.Runtime.InteropServices;

namespace INIRIP.Analysis
{
    public class Lexer
    {
        private int caretPosition = 0, tempCaretPosition = 0, tempCaretLength = 0;
        
        private readonly List<Token> tokens = [];
        private char current = ' ';

        public bool TryLexText(ReadOnlySpan<char> source, out ReadOnlySpan<Token> outtokens)
        {
            tokens.Clear();
            caretPosition = 0;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition]; 

                if(current == '\r')
                {
                    caretPosition++;

                    if (caretPosition < source.Length && source[caretPosition] == '\n')
                    {
#if DEBUG
                        Console.WriteLine("\t\\r\\n symbols");
#endif
                        tokens.Add(new(TokenKind.EOL, caretPosition - 1, 2));
                        caretPosition++;
                        continue;
                    }

#if DEBUG
                    Console.WriteLine("\t\\n symbol");
#endif
                    tokens.Add(new(TokenKind.EOL, caretPosition - 1, 1));
                    continue;
                }

                if(current == '\n')
                {
#if DEBUG
                    Console.WriteLine("\t\\n symbol");
#endif
                    caretPosition++;
                    tokens.Add(new(TokenKind.EOL, caretPosition - 1, 1));
                    continue;
                }

                if(current == '\t' || current == ' ' || current == '\f' || current == '\v')
                {
#if DEBUG
                    Console.WriteLine("\tuseless symbol");
#endif
                    caretPosition++;
                    continue;
                }

                switch (current)
                {
                    case '=':
#if DEBUG
                        Console.WriteLine($"\t equal sign, {current}");
#endif
                        tokens.Add(new(TokenKind.EqualSign, caretPosition, 1));
                        caretPosition++;
                        break;
                    case ';' or '#':
#if DEBUG
                        Console.WriteLine($"\t comment, {current}");
#endif
                        SkipComment(ref source);
                        break;
                    case '[':
#if DEBUG
                        Console.WriteLine($"\t section symbol, {current}");
#endif

                        caretPosition++;
                        BuildSection(ref source);
                        break;
                    default:
#if DEBUG
                        Console.WriteLine($"\t default, {current}");
#endif
                        BuildLiteral(ref source);
                        break;
                }
            }

            if(tokens.Count < 1)
            {
                outtokens = default;
                return false;
            }

            outtokens = CollectionsMarshal.AsSpan(tokens);
            return true;
        }

#region Builders

        private void SkipComment(ref ReadOnlySpan<char> source)
        {
            while(caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (current == '\r' || current == '\n')
                {
                    break;
                }

#if DEBUG
                Console.WriteLine($"\tIN SKIPCOMMENT\t symbol {current}");
#endif
                caretPosition++;
            }
        }

        private void BuildSection(ref ReadOnlySpan<char> source)
        {
            tempCaretPosition = caretPosition;
            tempCaretLength = 0;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (current == ']' || current == '\r' || current == '\n')
                {
                    caretPosition++;
                    break;
                }

#if DEBUG
                Console.WriteLine($"\tIN BUILDSECTION\t symbol {current}");
#endif
                caretPosition++;
                tempCaretLength++;
            }

            tokens.Add(new(TokenKind.Section, tempCaretPosition, tempCaretLength));
        }

        private void BuildLiteral(ref ReadOnlySpan<char> source)
        {
            tempCaretPosition = caretPosition;
            tempCaretLength = 0;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (current == '\r' || current == '\n') { break; }
                if(current == '=' || current == ';' || current == '#') { break; }
#if DEBUG
                Console.WriteLine($"\tIN BUILDLITERAL\t symbol {current}");
#endif
                caretPosition++;
                tempCaretLength++;
            }

            tokens.Add(new(TokenKind.Literal, tempCaretPosition, tempCaretLength));
        }

#endregion
    }
}

using MINI.Models;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MINI.Analysis
{
    public class Lexer
    {
        private int caretPosition=0, tempCaretPosition=0, tempCaretLength=0;
        private TokenKind tempTokenKind = TokenKind.Unknown;
        private readonly List<Token> tokens = [];
        private char current = ' ';

        public bool TryLexText(ReadOnlySpan<char> source, out ReadOnlySpan<Token> outtokens)
        {
            tokens.Clear();
            caretPosition = 0;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (
                    current == ' ' ||
                    current == '\t'||
                    current == '\n'||
                    current == '\r'
                )
                {
                    caretPosition++;
                    continue;
                }

                tempTokenKind = current switch
                {
                    ';' => TokenKind.Comment,
                    '[' => TokenKind.OpenBracketsSign,
                    _ when current != ' ' && current != '\t' && current != '\n' && current != '\r' => TokenKind.Identifier,
                    _ => TokenKind.Unknown
                };

                switch (tempTokenKind)
                {
                    case TokenKind.Comment:BuildComment(ref source); break;
                    case TokenKind.Identifier:
                        if (!TryBuildKeyValuePair(ref source))
                        {
                            outtokens = default;
                            return false;
                        }
                        break;
                    case TokenKind.OpenBracketsSign:
                        if (!TryBuildSection(ref source))
                        {
                            outtokens = default;
                            return false;
                        }
                        break;
                    default: caretPosition++; break;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildComment(ref ReadOnlySpan<char> source)
        {
            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (
                    current == '\n' ||
                    current == '\r'
                )
                {
                    break;
                }

                caretPosition++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryBuildSection(ref ReadOnlySpan<char> source)
        {
            tempCaretPosition = caretPosition;
            tempCaretLength = 0;
            caretPosition++;

            while (caretPosition < source.Length)
            {
                if (
                    current == '\n' ||
                    current == '\r'
                )
                {
                    return false;
                }

                if(current == ']')
                {
                    break;
                }

                caretPosition++;
            }

            tokens.Add(new Token(TokenKind.Identifier, tempCaretPosition+1, tempCaretLength));

            caretPosition++;

            tokens.Add(new Token(TokenKind.EqualSign, caretPosition, 1));

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryBuildKeyValuePair(ref ReadOnlySpan<char> source)
        {
            tempCaretPosition = caretPosition;
            tempCaretLength = 1;
            caretPosition++;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (
                    current == '\n' ||
                    current == '\r'
                )
                {
                    return false;
                }

                if (current == '=')
                {
                    break;
                }

                caretPosition++; tempCaretLength++;
            }

            tokens.Add(new Token(TokenKind.Identifier, tempCaretPosition, tempCaretLength));

            caretPosition++;

            tokens.Add(new Token(TokenKind.EqualSign, caretPosition, 1));

            caretPosition++;
            tempCaretPosition = caretPosition;
            tempCaretLength = 1;
            tempTokenKind = TokenKind.NumericalValue;

            while (caretPosition < source.Length)
            {
                current = source[caretPosition];

                if (
                    current == '\n' ||
                    current == '\r'
                )
                {
                    break;
                }

                if (tempTokenKind == TokenKind.NumericalValue && (!(current >= '0' && current <= '9') &&
                    current != '-' && current != '.')
                )
                {
                    tempTokenKind = TokenKind.LiteralValue;
                }

                caretPosition++; tempCaretLength++;
            }

            if (tempTokenKind == TokenKind.Identifier && tempCaretLength <= 5)
            {

                if (
                    source.Slice(tempCaretPosition, tempCaretLength) == "true" ||
                    source.Slice(tempCaretPosition, tempCaretLength) == "false"
                )
                {
                    tempTokenKind = TokenKind.BooleanValue;
                }

            }

            tokens.Add(new Token(tempTokenKind, tempCaretPosition, tempCaretLength));

            return true;
        }

        #endregion
    }
}

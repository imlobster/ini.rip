using INIRIP.Analysis;
using INIRIP.Compiler;

namespace INIRIP
{
    public static class Coder
    {
        private static readonly Lexer lexer = new();
        private static readonly Parser parser = new();

        /// <summary>
        /// Read values from ini.
        /// </summary>
        /// <param name="source">Your readed ini file</param>
        /// <param name="outvals">Values deserialized from ini</param>
        /// <returns>True if success. False if error</returns>
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

        /// <summary>
        /// Save your values into ini.
        /// </summary>
        /// <param name="fileFullPath">Path in format 'C:\Users\Desktop\MyFile.ini'</param>
        /// <param name="invalues">Values to be serialized</param>
        /// <returns>True if success. False if error</returns>
        public static bool TryEncode
        (
            string fileFullPath,

            Dictionary<
                    ReadOnlyMemory<char>,
                    Dictionary<
                        ReadOnlyMemory<char>,
                        ReadOnlyMemory<char>
                    >
            > invalues
        )
        {
            if(fileFullPath == null)
            {
                return false;
            }

            return FileExport.TryExportFile(invalues, fileFullPath);
        }
    }
}

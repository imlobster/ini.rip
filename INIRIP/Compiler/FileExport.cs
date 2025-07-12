using System.Text;

namespace INIRIP.Compiler
{
    public static class FileExport
    {
        private static readonly StringBuilder stringbuilder = new();

        public static bool TryExportFile
        (
            Dictionary<
                    ReadOnlyMemory<char>,
                    Dictionary<
                        ReadOnlyMemory<char>,
                        ReadOnlyMemory<char>
                    >
            > invalues,

            string filePathWithNameAndExtension
        )
        {
            stringbuilder.Clear();
            foreach (var kvp in invalues)
            {
                stringbuilder.Append('[');
                stringbuilder.Append(kvp.Key);
                stringbuilder.Append("]\r\n");
                foreach(var kvp2 in kvp.Value)
                {
                    stringbuilder.Append(kvp2.Key);
                    stringbuilder.Append('=');
                    stringbuilder.Append(kvp2.Value);
                    stringbuilder.Append("\r\n");
                }
                stringbuilder.Append("\r\n");
            }

            try
            {
                File.WriteAllText(
                    filePathWithNameAndExtension,
                    stringbuilder.ToString()
                );
                return true;
            }
            catch { return false; }
        }
    }
}

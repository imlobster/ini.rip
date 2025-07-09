using MINI;
using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        string source = @"; comment
[section]
key=value
";

        ReadOnlySpan<char> sauce = source.AsSpan();

        MINI.Analysis.Lexer lexer = new MINI.Analysis.Lexer();

        //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

        //Stopwatch sw = new();

        //Console.WriteLine("Warmup...");

        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //GC.Collect();
        //// Теперь замер

        //for (int i = 0; i < 1_000_000; i++)
        //{
        //    lexer.TryLexText(sauce, out _);
        //}

        //Console.WriteLine("Warmup ended.");

        //Thread.Sleep(3000);

        //sw.Restart();
        
        if(!lexer.TryLexText(sauce, out ReadOnlySpan<MINI.Models.Token> tokens)) { Console.WriteLine("Problem!"); return; }

        //sw.Stop();

        //Console.WriteLine($"Elapsed on lexing in Stopwatch {sw.ElapsedTicks} with Stopwatch.Frequency {Stopwatch.Frequency}");

        foreach (MINI.Models.Token token in tokens)
        {
            Console.WriteLine($"{token.ToString()} with type {token.Kind} and content {{{source.Substring(token.Start, token.Length)}}}");
        }
    }
}
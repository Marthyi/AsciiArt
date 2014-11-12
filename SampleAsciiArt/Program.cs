namespace SampleAsciiArt
{
    using System;

    using AsciiArt;

    public static class Program
    {
        private static void Main(string[] args)
        {
            AsciiArt.Write("Hello");
            AsciiArt.WriteLineSeparator();

            Console.WriteLine(AsciiArt.ToString("Hello"));
            AsciiArt.WriteLineSeparator('~');
            
            AsciiArt.Write("Hello", 25,"Comic sans ms");
            AsciiArt.WriteLineSeparator('_');

            Console.Read();
        }
    }
}

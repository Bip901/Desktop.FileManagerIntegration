using Desktop.FileManagerIntegration;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Show folder (d) or highlight file (f)?");
            if ((Console.ReadLine() ?? string.Empty).ToUpperInvariant() == "D")
            {
                string exampleDirectory = AppContext.BaseDirectory;
                Console.WriteLine($"Showing folder: {exampleDirectory}");
                FileManager.ShowFolder(exampleDirectory);
            }
            else
            {
                string exampleFile = Directory.EnumerateFiles(AppContext.BaseDirectory).FirstOrDefault() ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                Console.WriteLine($"Highlighting file: {exampleFile}");
                FileManager.HighlightInContainingFolder(exampleFile);
            }
        }
    }
}

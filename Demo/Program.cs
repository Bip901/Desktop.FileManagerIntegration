using Desktop.FileManagerIntegration;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? exampleFile = Directory.EnumerateFiles(AppContext.BaseDirectory).FirstOrDefault() ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileManager.HighlightInContainingFolder(exampleFile);
        }
    }
}

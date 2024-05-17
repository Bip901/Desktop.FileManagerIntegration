using System.Diagnostics;
using System.IO;

namespace Desktop.FileManagerIntegration;

internal static class OSXFileManager
{
    private const string OPEN_PATH = "open";

    /// <summary>
    /// Opens the parent of the given file or directory in the default file explorer, and highlights the child.
    /// </summary>
    /// <param name="checkedPath">A path to an existing file or directory.</param>
    internal static void HighlightInContainingFolder(string checkedPath)
    {
        ShowFolder(Path.GetDirectoryName(checkedPath) ?? checkedPath);
    }

    /// <summary>
    /// Opens the given folder in the default file explorer as if the user had navigated to it.
    /// </summary>
    /// <param name="path">A path to an existing directory.</param>
    internal static void ShowFolder(string path)
    {
        ProcessStartInfo processStartInfo = new(OPEN_PATH);
        processStartInfo.ArgumentList.Add(path);
        Process.Start(processStartInfo);
    }
}

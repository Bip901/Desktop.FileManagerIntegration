using System.Diagnostics;
using System.IO;

namespace Desktop.FileManagerIntegration;

internal static class WindowsFileManager
{
    private const string WINDOWS_EXPLORER_PATH = "explorer.exe";
    private const string WINDOWS_EXPLORER_FALLBACK_PATH = "C:\\Windows\\explorer.exe";

    private static string GetExplorerPath()
    {
        string explorerPath = WINDOWS_EXPLORER_PATH;
        if (File.Exists(explorerPath))
        {
            return explorerPath;
        }
        return WINDOWS_EXPLORER_FALLBACK_PATH;
    }

    /// <summary>
    /// Replaces forward slashes with backwards slashes, to avoid explorer.exe confusing them for commandline switches.
    /// </summary>
    private static string NormalizeDirectorySeparator(string path)
    {
        return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// Opens the parent of the given file or directory in the default file explorer, and highlights the child.
    /// </summary>
    /// <param name="checkedPath">A path to an existing file or directory.</param>
    internal static void HighlightInContainingFolder(string checkedPath)
    {
        ProcessStartInfo processStartInfo = new(GetExplorerPath());
        processStartInfo.ArgumentList.Add("/select,");
        processStartInfo.ArgumentList.Add(NormalizeDirectorySeparator(checkedPath));
        Process.Start(processStartInfo);
    }

    /// <summary>
    /// Opens the given folder in the default file explorer as if the user had navigated to it.
    /// </summary>
    /// <param name="path">A path to an existing directory.</param>
    internal static void ShowFolder(string path)
    {
        ProcessStartInfo processStartInfo = new(GetExplorerPath());
        processStartInfo.ArgumentList.Add(NormalizeDirectorySeparator(path));
        Process.Start(processStartInfo);
    }
}

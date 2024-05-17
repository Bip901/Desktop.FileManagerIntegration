using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Desktop.FileManagerIntegration;

/// <summary>
/// A static utility class for interfacing with the file manager.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Opens the parent of the given file or directory in the default file explorer, and highlights the child.
    /// </summary>
    /// <param name="path">A path to a file or directory.</param>
    /// <remarks>
    /// The macOS implementation currently only opens the parent without highlighting the child.
    /// </remarks>
    /// <exception cref="ArgumentException">The given path represents a non-existent file or directory.</exception>
    /// <exception cref="PlatformNotSupportedException"/>
    public static void HighlightInContainingFolder(string path)
    {
        path = Path.GetFullPath(path);
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            throw new ArgumentException($"File or directory not found: {path}", nameof(path));
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            WindowsFileManager.HighlightInContainingFolder(path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            LinuxFileManager.HighlightInContainingFolder(path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            OSXFileManager.HighlightInContainingFolder(path);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    /// Opens the given folder in the default file explorer as if the user had navigated to it.
    /// </summary>
    /// <param name="path">A path to a directory.</param>
    /// <exception cref="DirectoryNotFoundException"/>
    public static void ShowFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Directory not found: {path}");
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            WindowsFileManager.ShowFolder(path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            LinuxFileManager.ShowFolder(path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            OSXFileManager.ShowFolder(path);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}

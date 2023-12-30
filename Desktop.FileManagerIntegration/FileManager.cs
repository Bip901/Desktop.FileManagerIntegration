using System.Diagnostics;
using System.Runtime.InteropServices;
using Tmds.DBus.Protocol;
using Tmds.DBus.SourceGenerator;

namespace Desktop.FileManagerIntegration;

/// <summary>
/// A static utility class for interfacing with the file manager.
/// </summary>
public static class FileManager
{
    private const string WINDOWS_EXPLORER_PATH = "explorer.exe";
    private const string WINDOWS_EXPLORER_FALLBACK_PATH = "C:\\Windows\\explorer.exe";
    private const string DBUS_FILEMANAGER_SERVICE_NAME = "org.freedesktop.FileManager1";
    private const string DBUS_FILEMANAGER_SERVICE_OBJECT_PATH = "/org/freedesktop/FileManager1";
    private const string OSX_OPEN_PATH = "open";

    /// <summary>
    /// Opens the parent of the given file or directory in the default file explorer, and highlights the child.
    /// </summary>
    /// <param name="path">A path to a file or directory.</param>
    /// <remarks>
    /// The macOS implementation currently only opens the parent without highlighting the child.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="PlatformNotSupportedException"/>
    public static void HighlightInContainingFolder(string path)
    {
        path = Path.GetFullPath(path);
        if (path.EndsWith(Path.DirectorySeparatorChar) && path.Length > 1) //Trailing slashes make some Linux file managers navigate into the directory
        {
            path = path[..^1];
        }
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            throw new ArgumentException($"File or directory not found: {path}", nameof(path));
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string explorerPath = WINDOWS_EXPLORER_PATH;
            if (!File.Exists(explorerPath))
            {
                explorerPath = WINDOWS_EXPLORER_FALLBACK_PATH;
            }
            ProcessStartInfo processStartInfo = new(explorerPath);
            processStartInfo.ArgumentList.Add("/select,");
            processStartInfo.ArgumentList.Add(path);
            Process.Start(processStartInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Connection connection;
            string? sessionAddress = Address.Session;
            if (sessionAddress == null)
            {
                connection = Connection.Session;
            }
            else
            {
                connection = new(new ClientConnectionOptions(sessionAddress) { AutoConnect = true });
            }
            using (connection)
            {
                OrgFreedesktopFileManager1 fileManagerProxy = new(connection, DBUS_FILEMANAGER_SERVICE_NAME, DBUS_FILEMANAGER_SERVICE_OBJECT_PATH);
                fileManagerProxy.ShowItemsAsync(new[] { "file:///" + path }, string.Empty).Wait();
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path = Path.GetDirectoryName(path) ?? path;
            ProcessStartInfo processStartInfo = new(OSX_OPEN_PATH);
            processStartInfo.ArgumentList.Add(path);
            Process.Start(processStartInfo);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}

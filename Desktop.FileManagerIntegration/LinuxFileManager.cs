using System.IO;
using System.Text.Encodings.Web;
using Tmds.DBus.Protocol;
using Tmds.DBus.SourceGenerator;

namespace Desktop.FileManagerIntegration;

internal static class LinuxFileManager
{
    private const string DBUS_FILEMANAGER_SERVICE_NAME = "org.freedesktop.FileManager1";
    private const string DBUS_FILEMANAGER_SERVICE_OBJECT_PATH = "/org/freedesktop/FileManager1";

    private static Connection GetDBusConnection()
    {
        string? sessionAddress = Address.Session;
        if (sessionAddress == null)
        {
            return Connection.Session;
        }
        else
        {
            return new(new ClientConnectionOptions(sessionAddress) { AutoConnect = true });
        }
    }

    /// <summary>
    /// Opens the parent of the given file or directory in the default file explorer, and highlights the child.
    /// </summary>
    /// <param name="checkedPath">A path to an existing file or directory.</param>
    internal static void HighlightInContainingFolder(string checkedPath)
    {
        if (checkedPath.EndsWith(Path.DirectorySeparatorChar) && checkedPath.Length > 1) //Trailing slashes make some Linux file managers navigate into the directory
        {
            checkedPath = checkedPath[..^1];
        }
        using Connection connection = GetDBusConnection();
        OrgFreedesktopFileManager1 fileManagerProxy = new(connection, DBUS_FILEMANAGER_SERVICE_NAME, DBUS_FILEMANAGER_SERVICE_OBJECT_PATH);
        fileManagerProxy.ShowItemsAsync(new[] { "file:///" + UrlEncoder.Default.Encode(checkedPath) }, string.Empty).Wait();
    }

    /// <summary>
    /// Opens the given folder in the default file explorer as if the user had navigated to it.
    /// </summary>
    /// <param name="path">A path to an existing directory.</param>
    internal static void ShowFolder(string path)
    {
        using Connection connection = GetDBusConnection();
        OrgFreedesktopFileManager1 fileManagerProxy = new(connection, DBUS_FILEMANAGER_SERVICE_NAME, DBUS_FILEMANAGER_SERVICE_OBJECT_PATH);
        fileManagerProxy.ShowFoldersAsync(new[] { "file:///" + UrlEncoder.Default.Encode(path) }, string.Empty).Wait();
    }
}

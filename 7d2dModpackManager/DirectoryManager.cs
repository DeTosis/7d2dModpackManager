using System.IO;
namespace _7d2dModpackManager;

public static class DirectoryManager {
    public static void CopyDirectory(string src, string dst) {
        var dir = new DirectoryInfo(src);
        DirectoryInfo[] dirs = dir.GetDirectories();


        Directory.CreateDirectory(dst);

        foreach (FileInfo file in dir.GetFiles()) {
            string targetFilePath = Path.Combine(dst, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs) {
            string newDestinationDir = Path.Combine(dst, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir);
        }
    }
}

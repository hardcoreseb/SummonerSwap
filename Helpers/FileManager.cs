using System.IO;

namespace SummonerSwap.Helpers
{
    public static class FileManager
    {
        public static void CopyDirectory(string source, string destination)
        {
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destination));

            foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                string targetFilePath = filePath.Replace(source, destination);
                string? targetDirectory = Path.GetDirectoryName(targetFilePath);

                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                File.Copy(filePath, targetFilePath, true);
            }
        }
    }
}

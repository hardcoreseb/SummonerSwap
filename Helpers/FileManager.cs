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
                File.Copy(filePath, filePath.Replace(source, destination), true);
        }
    }
}

using System.IO;

namespace MusicPlayer.Core.Utilities
{
    public static class Logger
    {
        private const string fileName = "Log.txt";

        public static void Log(string message)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            long size = new FileInfo(fileName).Length;

            if (size > 2048)
            {
                File.Delete(fileName);
            }

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(message);
            sw.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPlayer.Core.Utilities
{
    public static class SubdirectoryFileFinder
    {
        public static IEnumerable<string> GetFilesFromSubdirectories(string[] directories, string[] searchPatterns)
        {
            var files = Enumerable.Empty<string>();
            foreach (string dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    Logger.Log("Directory does not exist: " + dir);
                    throw new ArgumentException("Directory does not exist!");
                }
                foreach (string pattern in searchPatterns)
                {
                    files = files.Concat(EnumerateFiles(dir, pattern, SearchOption.AllDirectories));
                }
            }
            return files;
        }


        // this method safely gets all files from subdirectories while dealing with potential UnauthorizedAccessExceptions 
        private static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOpt)
        {
            try
            {
                var dirFiles = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories)
                {
                    dirFiles = Directory.EnumerateDirectories(path)
                                        .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
                }
                return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}

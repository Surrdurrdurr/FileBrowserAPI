using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBrowser.Data.Models
{
    public static class FileBrowserModel
    {
        public const string DefaultDirectory = @"C:\Files";
        public static IEnumerable<string> GetAllFiles(string path = DefaultDirectory)
        {
            var files = Directory.EnumerateFiles(path);

            return files.Select(f => Path.GetFileName(f));
        }

    }
}

using Microsoft.AspNetCore.Http;
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

        public static async Task UploadFile(IFormFile file, string path = DefaultDirectory)
        {
            var filePath = Path.Join(path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

    }
}

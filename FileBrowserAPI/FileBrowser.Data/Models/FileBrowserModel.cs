using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileBrowser.Data.Models
{
    public static class FileBrowserModel
    {
        public const string DefaultDirectory = @"C:\Files";
        public static IEnumerable<string> GetAllFiles(string path = DefaultDirectory)
        {
            try
            {
                var files = Directory.EnumerateFiles(path);
                return files.Select(f => Path.GetFileName(f));
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }

        public static async Task<bool> UploadFile(IFormFile file, string path = DefaultDirectory)
        {
            var filePath = Path.Join(path, file.FileName);
            if (!IsPathValid(filePath))
                return false;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return true;
        }

        public static IEnumerable<string> GetFilteredFiles(string pattern, string path = DefaultDirectory)
        {
            try
            {
                var regex = new Regex(pattern);
                var files = GetAllFiles(path);
                if (files == null)
                    return null;

                var filteredFiles = new List<string>();

                foreach (var file in files)
                {
                    if (regex.IsMatch(file))
                        filteredFiles.Add(file);
                }
                return filteredFiles;
            }
            catch (RegexParseException)
            {
                return null;
            }
        }

        public static IEnumerable<string> DeleteFilteredFiles(string pattern, string path = DefaultDirectory)
        {
            try
            {
                var regex = new Regex(pattern);
                var files = GetAllFiles(path);
                if (files == null)
                    return null;

                var filesWithDir = Directory.EnumerateFiles(path);

                var result = new List<string>();

                foreach (var file in files)
                {
                    if (regex.IsMatch(file))
                    {
                        File.Delete(filesWithDir.FirstOrDefault(f => f.Contains(file)));
                        result.Add(file);
                    }
                }
                return result;
            }
            catch (ArgumentNullException) // If somehow file is deleted manually during foreach
            {
                return null;
            }
            catch (RegexParseException)
            {
                return null;
            }
        }

        private static bool IsPathValid(string path, bool withFile = true)
        {
            try
            {
                Path.GetPathRoot(path);
                Path.GetDirectoryName(path);
                if (withFile)
                    Path.GetFileName(path);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}

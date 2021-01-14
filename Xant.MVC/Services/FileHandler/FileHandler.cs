using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xant.MVC.Services.FileHandler
{
    /// <summary>
    /// Default implementation for IFileHandler.
    /// </summary>
    public class FileHandler : IFileHandler
    {
        //Static path values
        private static class ConstantPath
        {
            public const string Posts = "posts";
            public const string Profiles = "profiles";
            public const string Uploads = "uploads";
        }

        private string GetUploadFolderPath(FileHandlerFolder fileHandlerFolder)
        {
            if (fileHandlerFolder == FileHandlerFolder.Posts)
                return ConstantPath.Posts;

            return ConstantPath.Profiles;
        }
        private async Task Upload(IFormFile file, string uploadPath)
        {
            Directory.CreateDirectory(uploadPath);

            var fullPath = Path.Combine(uploadPath, file.FileName);

            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UploadMedia(
            IFormFileCollection files,
            string webRootPath,
            string filePath, 
            FileHandlerFolder fileHandlerFolder)
        {
            string uploadPath = webRootPath + "\\" + ConstantPath.Uploads + "\\"
                                + GetUploadFolderPath(fileHandlerFolder) + "\\" + filePath;
            foreach (var file in files)
            {
                await Upload(file, uploadPath);
            }
        }

        public void DeleteMedia(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder)
        {
            string uploadPath = webRootPath + "\\" + ConstantPath.Uploads + "\\"
                                + GetUploadFolderPath(fileHandlerFolder) + "\\" + filePath;
            try
            {
                if (Directory.Exists(uploadPath))
                {
                    Directory.Delete(uploadPath, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string> GetFilesSourceList(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder)
        {
            var folderPath = GetUploadFolderPath(fileHandlerFolder);
            string uploadPath = webRootPath + "\\" + ConstantPath.Uploads + "\\"
                                + folderPath + "\\" + filePath;

            List<string> fileSources = new List<string>();
            try
            {
                var files = Directory.GetFiles(
                    uploadPath,
                    "*.*",
                    SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    string src = "\\" + ConstantPath.Uploads + "\\"
                                 + folderPath + "\\" + filePath + "\\" + Path.GetFileName(file);

                    fileSources.Add(src.Replace(@"\", "/"));
                }

            }
            catch { }

            return fileSources;
        }

        public string GetFileSource(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder)
        {
            var folderPath = GetUploadFolderPath(fileHandlerFolder);
            string uploadPath = webRootPath + "\\" + ConstantPath.Uploads + "\\"
                                + folderPath + "\\" + filePath;

            try
            {
                var files = Directory.GetFiles(
                    uploadPath,
                    "*.*",
                    SearchOption.TopDirectoryOnly);

                if (files.Length > 0 && files[0] != null)
                {
                    var path = "\\" + ConstantPath.Uploads + "\\" + 
                               folderPath + "\\" + filePath + "\\" + Path.GetFileName(files[0]);

                    return path.Replace(@"\", "/");
                }
            }
            catch { }

            return null;
        }
    }

}

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
        private string GetUploadFolderPath(FileHandlerFolder fileHandlerFolder)
        {
            if (fileHandlerFolder == FileHandlerFolder.Posts)
                return "posts";

            return "profiles";
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
        public async Task UploadMedia(IFormFileCollection files, string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder)
        {
            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
                                + GetUploadFolderPath(fileHandlerFolder) + "\\" + filePath;
            foreach (var file in files)
            {
                await Upload(file, uploadPath);
            }
        }

        public async Task UploadUserProfile(IFormFile file, string webRootPath, string filePath)
        {
            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
                                + "profiles" + "\\" + filePath;

            await Upload(file, uploadPath);
        }

        public void DeleteMedia(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder)
        {

            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
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
            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
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
                    string src = "\\" + "uploads" + "\\"
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
            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
                                + folderPath + "\\" + filePath;

            try
            {
                var files = Directory.GetFiles(
                    uploadPath,
                    "*.*",
                    SearchOption.TopDirectoryOnly);

                if (files.Length > 0 && files[0] != null)
                {
                    var path = "\\" + "uploads" + "\\" + folderPath + "\\" + filePath + "\\" + Path.GetFileName(files[0]);
                    
                    return path.Replace(@"\", "/");
                }
            }
            catch { }

            return null;
        }

        public string GetUserProfileSource(string webRootPath, string filePath)
        {
            string uploadPath = webRootPath + "\\" + "uploads" + "\\"
                                + "profiles" + "\\" + filePath;

            try
            {
                var files = Directory.GetFiles(
                    uploadPath,
                    "*.*",
                    SearchOption.TopDirectoryOnly);

                var path = "\\" + "uploads" + "\\"
                             + "profiles" + "\\" + filePath + "\\" + Path.GetFileName(files[0]);

                return path.Replace(@"\", "/");
            }
            catch { }

            return null;
        }
    }

}

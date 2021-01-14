using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Xant.MVC.Services.FileHandler
{
    /// <summary>
    /// Use to handle files operations, like upload, deletion and getting files path
    /// </summary>
    public interface IFileHandler
    {
        Task UploadMedia(IFormFileCollection files, string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder);
        void DeleteMedia(string webRootPath, string filePath, FileHandlerFolder folder);
        List<string> GetFilesSourceList(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder);
        string GetFileSource(string webRootPath, string filePath, FileHandlerFolder fileHandlerFolder);
    }

    /// <summary>
    /// File handler folder enum
    /// </summary>
    public enum FileHandlerFolder
    {
        Posts,
        Profiles
    }
}

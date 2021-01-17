using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;
using Xant.MVC.Services.FileHandler;

namespace Xant.MVC.Mappings.Resolvers.WebsiteFrontResolvers
{
    /// <summary>
    /// Auto mapper resolver for PostViewModel UserFilePath
    /// </summary>
    public class PostViewModelUserFilePathResolver : IValueResolver<Post, PostViewModel, string>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileHandler _fileHandler;

        public PostViewModelUserFilePathResolver(IWebHostEnvironment env, IFileHandler fileHandler)
        {
            _env = env;
            _fileHandler = fileHandler;
        }
        public string Resolve(Post source, PostViewModel destination, string destMember, ResolutionContext context)
        {
            return _fileHandler.GetFileSource(
                _env.WebRootPath,
                source.User.FilesPathGuid.ToString(),
                FileHandlerFolder.Profiles);
        }
    }
}

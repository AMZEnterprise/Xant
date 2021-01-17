using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Mappings.Resolvers.WebsiteFrontResolvers;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Mappings
{
    public class WebsiteFrontProfile : Profile
    {
        public WebsiteFrontProfile()
        {
            //Header mapping
            CreateMap<Setting, HeaderViewModel>();

            //Footer mapping
            CreateMap<Setting, FooterViewModel>();

            //Call to action mapping
            CreateMap<Setting, CallToActionViewModel>();

            //Contact mapping
            CreateMap<Setting, ContactViewModel>();

            //Price plans mapping
            CreateMap<Setting, PricePlansViewModel>();

            //Blog mappings
            CreateMap<PostCategory, PostCategoryViewModel>();
            CreateMap<Post, PostViewModel>()
                .ForMember(x => x.PostCategoryViewModel,
                    y =>
                        y.MapFrom(u => u.PostCategory))
                .ForMember(x => x.UserFullName,
                    y =>
                        y.MapFrom<PostViewModelUserFullNameResolver>())
                .ForMember(x => x.UserFilePath,
                    y =>
                        y.MapFrom<PostViewModelUserFilePathResolver>())
                .ForMember(x => x.FilePath,
                    y => y.MapFrom<PostViewModelFilePathResolver>())
                .ForMember(x => x.PostCommentViewModels,
                    y =>
                        y.MapFrom(u => u.PostComments));
            CreateMap<PostComment, PostCommentViewModel>()
                .ForMember(x => x.UserFullName,
                    y =>
                        y.MapFrom<PostCommentViewModelUserFullName>());
            CreateMap<PostCommentFormViewModel, PostComment>();

            //Contact mapping
            CreateMap<ContactFormViewModel, Contact>();
        }
    }
}

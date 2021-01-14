using AutoMapper;
using System;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Mappings.Resolvers;

namespace Xant.MVC.Mappings
{
    public class AdminPanelProfile : Profile
    {
        public AdminPanelProfile()
        {
            //Contact mappings
            CreateMap<Contact, ContactIndexViewModel>();
            CreateMap<Contact, ContactFormViewModel>();
            CreateMap<ContactFormViewModel, Contact>();

            //Post mapping
            CreateMap<Post, PostIndexViewModel>()
                .ForMember(x => x.UserFullName,
                    y =>
                        y.MapFrom<PostIndexViewModelUserFullNameResolver>());
            CreateMap<Post, PostFormViewModel>()
                .ForMember(x => x.UserFullName,
                    y =>
                        y.MapFrom<PostFormUserFullNameResolver>())
                .ForMember(x => x.FilePath,
                    y =>
                        y.MapFrom<PostFormViewModelFilePathResolver>());
            CreateMap<PostFormViewModel, Post>()
                .ForMember(x => x.FilesPathGuid,
                    y =>
                        y.MapFrom(u => Guid.NewGuid()));

            //PostCategory mappings
            CreateMap<PostCategory, PostCategoryIndexViewModel>();
            CreateMap<PostCategory, PostCategoryFormViewModel>();
            CreateMap<PostCategoryFormViewModel, PostCategory>();
            CreateMap<PostCategoryType, PostCategoryTypeEnumViewModel>();
            CreateMap<PostCategoryTypeEnumViewModel, PostCategoryType>();

            //PostComment mappings
            CreateMap<PostComment, PostCommentIndexViewModel>();
            CreateMap<PostComment, PostCommentFormViewModel>();
            CreateMap<PostComment, PostCommentReplyFormViewModel>()
                .ForMember(x => x.ParentId,
                    y =>
                        y.MapFrom(u => u.Parent.Id))
                .ForMember(x => x.PostId,
                    y =>
                        y.MapFrom(u => u.PostId))
                .ForMember(x => x.ParentBody,
                    y =>
                        y.MapFrom(u => u.Body));
            CreateMap<PostCommentReplyFormViewModel, PostComment>();

            //Setting mappings
            CreateMap<Setting, SettingFormViewModel>();
            CreateMap<SettingFormViewModel, Setting>();

            //User mappings
            CreateMap<User, UserIndexViewModel>()
                .ForMember(x => x.UserFullName,
                    y =>
                        y.MapFrom<UserIndexViewModelUserFullNameResolver>());
            CreateMap<UserCreateFormViewModel, User>();
            CreateMap<User, UserFormViewModel>()
                .ForMember(x => x.FilePath,
                    y =>
                        y.MapFrom<UserFormViewModelFilePathResolver>()); ;
            CreateMap<UserFormViewModel, User>();
        }
    }
}

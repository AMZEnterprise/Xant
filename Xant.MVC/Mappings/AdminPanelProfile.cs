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
                .ForMember(x=>x.UserFullName,
                    y=>
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

            //Setting mappings
            CreateMap<Setting, SettingFormViewModel>();
            CreateMap<SettingFormViewModel, Setting>();
        }
    }
}

using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.MVC.Mappings
{
    public class AdminPanelProfile : Profile
    {
        public AdminPanelProfile()
        {
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

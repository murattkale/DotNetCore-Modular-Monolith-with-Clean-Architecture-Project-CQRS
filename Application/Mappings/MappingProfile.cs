using Application.Features.ContentPages.Commands;
using Application.Features.ContentPages.Dtos;
using Application.Features.Documents.Commands;
using Application.Features.Documents.Dtos;
using Application.Features.Forms.Commands;
using Application.Features.Forms.Dtos;
using Application.Features.FormTypes.Commands;
using Application.Features.FormTypes.Dtos;
using Application.Features.LangDisplays.Commands;
using Application.Features.LangDisplays.Dtos;
using Application.Features.Langs.Commands;
using Application.Features.Langs.Dtos;
using Application.Features.SiteConfigs.Commands;
using Application.Features.Users.Commands;
using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using dotnetcoreproject.Domain.Entities;

namespace dotnetcoreproject.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateDocumentsCommand, Documents>();
        CreateMap<PatchDocumentsCommand, Documents>();
        CreateMap<Documents, DocumentsDto>();

        CreateMap<CreateContentPageCommand, ContentPage>();
        CreateMap<PatchContentPageCommand, ContentPage>();
        CreateMap<ContentPage, ContentPageDto>();


        CreateMap<CreateUserCommand, User>();
        CreateMap<PatchUserCommand, User>();
        CreateMap<User, UserDto>();

        CreateMap<CreateSiteConfigCommand, SiteConfig>();
        CreateMap<PatchSiteConfigCommand, SiteConfig>();
        CreateMap<SiteConfig, SiteConfigDto>();

        CreateMap<CreateLangCommand, Lang>();
        CreateMap<PatchLangCommand, Lang>();
        CreateMap<Lang, LangDto>();
        
        CreateMap<CreateLangDisplayCommand, LangDisplay>();
        CreateMap<PatchLangDisplayCommand, LangDisplay>();
        CreateMap<LangDisplay, LangDisplayDto>();
        
        CreateMap<CreateFormTypeCommand, FormType>();
        CreateMap<PatchFormTypeCommand, FormType>();
        CreateMap<FormType, FormTypeDto>();
        
        CreateMap<CreateFormsCommand, Forms>();
        CreateMap<PatchFormsCommand, Forms>();
        CreateMap<Forms, FormsDto>();
        
    }
}
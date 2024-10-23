using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Features.LangDisplays.Dtos;
using Application.Features.Langs.Dtos;
using Application.Features.Users.Dtos;
using Domain.Enums;
using Domain.Helpers;
using dotnetcoreproject.Domain.Entities;
using Microsoft.Extensions.Configuration;
using UI.Models;

public static class SessionRequest
{
    static IHttpContextAccessor _IHttpContextAccessor;
    static IConfiguration _IConfiguration;

    public static void Configure(IHttpContextAccessor __IHttpContextAccessor, IConfiguration __IConfiguration)
    {
        _IHttpContextAccessor = __IHttpContextAccessor;
        _IConfiguration = __IConfiguration;
    }


    public static int LanguageId => (_IHttpContextAccessor.HttpContext.Session.GetInt32("LangId") == null
        ? 1
        : _IHttpContextAccessor.HttpContext.Session.GetInt32("LangId")).ToInt();

    public static int? BeforeLanguageId => _IHttpContextAccessor.HttpContext.Session.GetInt32("LangId");


    public static string LanguageName =>
        Languages?.Where(o => o.LangId == LanguageId)?.Select(o => o.Name)?.FirstOrDefault();

    public static List<LangDto> Languages => _IHttpContextAccessor.HttpContext.Session.Get<List<LangDto>>("_languages");
    public static UserDto User => _IHttpContextAccessor.HttpContext.Session.Get<UserDto>("_user");

    public static List<SiteConfigDto> SiteConfig =>
        _IHttpContextAccessor.HttpContext.Session.Get<List<SiteConfigDto>>("_siteconfig");

    public static List<LangDisplayDto> LangDisplay =>
        _IHttpContextAccessor.HttpContext.Session.Get<List<LangDisplayDto>>("_LangDisplay");

    public static List<EnumModel> ContentTypes => Enum.GetValues(typeof(ContentTypes)).Cast<int>().Select(x =>
            new EnumModel
                { name = ((ContentTypes)x).ToStr(), value = x.ToString(), text = ((ContentTypes)x).ExGetDescription() })
        .ToList();

    public static List<FormType> FormTypes =>
        _IHttpContextAccessor.HttpContext.Session.Get<List<FormType>>("_formtypes");

    public static string BaseFolderImage(this string imageUrl) => "/fileupload/UserFiles/Folders/";

    public static string SetImage(this string imageUrl)
    {
        return !string.IsNullOrEmpty(imageUrl) ? "/fileupload/UserFiles/Folders/" + imageUrl : "";
    }

    public static string Trans(this string ParamName)
    {
        var lang = LangDisplay.FirstOrDefault(o => o.ParamName == ParamName);
        var text = ParamName;
        if (lang != null)
        {
            switch (LanguageId)
            {
                case 1:
                {
                    text = lang.Name_1;
                }
                    break;
                case 2:
                {
                    text = lang.Name_2;
                }
                    break;
                case 3:
                {
                    text = lang.Name_3;
                }
                    break;
                case 4:
                {
                    text = lang.Name_4;
                }
                    break;
                case 5:
                {
                    text = lang.Name_5;
                }
                    break;
                case 6:
                {
                    text = lang.Name_6;
                }
                    break;
                case 7:
                {
                    text = lang.Name_7;
                }
                    break;
                case 8:
                {
                    text = lang.Name_8;
                }
                    break;
                case 9:
                {
                    text = lang.Name_9;
                }
                    break;
                case 10:
                {
                    text = lang.Name_10;
                }
                    break;
            }
        }
        else
        {
        }

        return text;
    }
}
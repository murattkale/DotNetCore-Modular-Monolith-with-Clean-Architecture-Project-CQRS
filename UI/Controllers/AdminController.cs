using System.Threading.Tasks;
using Application.Features.FormTypes.Queries;
using Application.Features.LangDisplays.Queries;
using Application.Features.Langs.Queries;
using Application.Features.SiteConfigs.Queries;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers;

public class AdminController(IMediator mediator) : Controller
{
    public async Task<bool?> SetData()
    {
        var lang = new GetAllLangQuery
        {
            PagedRequest = new PagedRequest<Lang>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                          cacheControl == "false"),
                OrderByColumn = "OrderNo",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 20
            }
        };
        var _languages = await mediator.Send(lang);
        HttpContext.Session.Set("_languages", _languages.Data.Items);

        var langdisplay = new GetAllLangDisplayQuery
        {
            PagedRequest = new PagedRequest<LangDisplay>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl3) &&
                          cacheControl3 == "false"),
                OrderByColumn = "OrderNo",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 9999
            }
        };
        
        var _langdisplay = await mediator.Send(langdisplay);
        HttpContext.Session.Set("_langdisplay", _langdisplay.Data.Items);
        
        var formtypes = new GetAllFormTypeQuery
        {
            PagedRequest = new PagedRequest<FormType>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl4) &&
                          cacheControl4 == "false"),
                OrderByColumn = "OrderNo",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 9999
            }
        };

        var _formtypes = await mediator.Send(formtypes);
        HttpContext.Session.Set("_formtypes", _formtypes.Data.Items);

        // var siteconfig = new GetSiteConfigByKeyQuery()
        // {
        //     Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl2) &&
        //               cacheControl2 == "false"),
        //     ConfigKey = "DataCacheDuration"
        // };
        //
        // var _siteconfig = await _mediator.Send(siteconfig);
        // HttpContext.Session.Set("_siteconfig", _siteconfig.Data);
        return true;
    }

    [Route("admin/dashboard")]
    public async Task<IActionResult> Index()
    {
        await SetData();
        return View();
    }

    [Route("admin/login")]
    public async Task<IActionResult> Login()
    {
        await SetData();
        return View();
    }


    public IActionResult SetLangId(int langId)
    {
        HttpContext.Session.SetString("LangId", langId.ToString());
        return Ok();
    }
}
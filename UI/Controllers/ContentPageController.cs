using System;
using System.Threading.Tasks;
using Application.Features.ContentPages.Commands;
using Application.Features.ContentPages.Dtos;
using Application.Features.ContentPages.Queries;
using Application.Features.FormTypes.Queries;
using Application.Features.LangDisplays.Queries;
using Application.Features.Langs.Queries;
using Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers;

public class ContentPageController : Controller
{
    IMediator mediator;

    public ContentPageController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("admin/contentpage/list")]
    public async Task<IActionResult> List()
    {
        await SetData();
        return View();
    }


    [Route("admin/contentpage/getpaging")]
    [HttpPost]
    public async Task<IActionResult> GetPaging([FromBody] PagedRequest<ContentPage> pagedRequest)
    {
        var req = new GetAllContentPageQuery
        {
            PagedRequest = pagedRequest
        };
        var result = await mediator.Send(req);

        var paging = new PagedResponse<ContentPageDto>
        {
            data = result.Data.Items,
            draw = result.Data.PageNumber,
            recordsFiltered = result.Data.TotalCount,
            recordsTotal = result.Data.TotalCount,
            LastModifiedOn = DateTime.Now.ToString()
        };

        return Json(paging);
    }

    [Route("admin/contentpage/updatecreate")]
    [HttpPost]
    public async Task<IActionResult> updatecreate(CreateContentPageCommand command)
    {
        var result = await mediator.Send(command);
        return Json(result);
    }


    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetContentPageByIdQuery
        {
            Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                      cacheControl == "false"),
            Id = id
        });
        return Json(result);
    }

    [Route("admin/contentpage/delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeleteContentPageCommand
        {
            Id = id
        });
        return Json(result);
    }

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
}
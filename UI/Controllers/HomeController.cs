using System.Threading.Tasks;
using Application.Features.ContentPages.Queries;
using Application.Features.LangDisplays.Queries;
using Application.Features.Langs.Queries;
using Application.Features.SiteConfigs.Queries;
using Domain;
using Domain.Enums;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool?> SetData()
    {
        if (SessionRequest.LanguageId == 1) HttpContext.Session.SetInt32("LangId", 1);

        var siteconfigQuery = new GetAllSiteConfigQuery()
        {
            PagedRequest = new PagedRequest<SiteConfig>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl1) &&
                          cacheControl1 == "false"),
                OrderByColumn = "Id",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 50
            }
        };

        var _siteconfig = await _mediator.Send(siteconfigQuery);
        HttpContext.Session.Set("_siteconfig", _siteconfig.Data.Items);

        var languagesQuery = new GetAllLangQuery()
        {
            PagedRequest = new PagedRequest<Lang>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl2) &&
                          cacheControl2 == "false"),
                OrderByColumn = "Id",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 50
            }
        };

        var _languages = await _mediator.Send(languagesQuery);
        HttpContext.Session.Set("_languages", _languages.Data.Items);


        var HeaderFooterQuery = new GetAllContentPageQuery
        {
            PagedRequest = new PagedRequest<ContentPage>
            {
                Predicate = o => o.IsHeaderMenu == true || o.ContentTypes == ContentTypes.FooterPage,
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                          cacheControl == "false"),
                OrderByColumn = "OrderNo",
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 50
            }
        };
        ViewBag.HeaderFooter = await _mediator.Send(HeaderFooterQuery);

        var langdisplay = new GetAllLangDisplayQuery
        {
            PagedRequest = new PagedRequest<LangDisplay>
            {
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl3) &&
                          cacheControl3 == "false"),
                OrderByDescending = false,
                PageNumber = 1,
                PageSize = 9999
            }
        };

        var _langdisplay = await _mediator.Send(langdisplay);
        HttpContext.Session.Set("_LangDisplay", _langdisplay.Data.Items);

        return true;
    }

    public async Task<IActionResult> Index()
    {
        await SetData();
        var query = new GetAllContentPageQuery
        {
            PagedRequest = new PagedRequest<ContentPage>
            {
                Predicate = o => o.ContentTypes == ContentTypes.MainPage || o.ContentTypes == ContentTypes.Slider,
                Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                          cacheControl == "false"),
                OrderByColumn = "Id",
                OrderByDescending = true,
                PageNumber = 1,
                PageSize = 25
            }
        };
        var response = await _mediator.Send(query);
        if (response.IsSuccess) return View(response);
        return BadRequest(response);
    }


    [HttpGet("detail/{link}")]
    public async Task<IActionResult> BaseContent(GetContentPageByLinkQuery query)
    {
        if (!string.IsNullOrEmpty(query.Link))
        {
            var _cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                           cacheControl == "false");
            query.Cache = _cache;
            HttpContext.Session.SetInt32("IsLang", 0);
            var response = await _mediator.Send(query);
            HttpContext.Session.SetInt32("IsLang", 1);
            if (response.IsSuccess)
            {
                if (response.Data.LangId != SessionRequest.LanguageId)
                {
                    if (SessionRequest.BeforeLanguageId !=null && SessionRequest.BeforeLanguageId != response.Data.LangId)
                    {
                        HttpContext.Session.SetInt32("LangStatus", 0);
                        if (response.Data.OrjId > 0)
                        {
                            HttpContext.Session.SetInt32("IsLang", 0);
                            response = await _mediator.Send(new GetContentPageByIdQuery
                                { Id = response.Data.OrjId.Value, Cache = _cache });
                            HttpContext.Session.SetInt32("IsLang", 1);
                        }
                        else
                        {
                            HttpContext.Session.SetInt32("IsLang", 0);
                            response = await _mediator.Send(new GetContentPageByOrjIdQuery
                                { Id = response.Data.Id, Cache = _cache });
                            HttpContext.Session.SetInt32("IsLang", 1);
                        }
                        
                        return Redirect(response.Data.Link);
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("LangId", response.Data.LangId.Value);
                        HttpContext.Session.SetInt32("BeforeLangId", response.Data.LangId.Value);
                    }
                }


                ViewBag.url = response.Data.Link;
                ViewBag.title = response.Data.Name;
                ViewBag.description = response.Data.Description;
                ViewBag.image = response.Data.ImageUrlPrefix(DocType.Picture);
                ViewBag.page = response.Data;
                await SetData();
                return View(response);
            }
            else
            {
                Redirect("/");
            }
        }
        else
        {
            return Redirect("/");
        }

        return Redirect("/");
    }

    public IActionResult SetLangId(int LangId)
    {
        HttpContext.Session.SetInt32("BeforeLangId", SessionRequest.LanguageId);
        HttpContext.Session.SetInt32("LangId", LangId);
        HttpContext.Response.Headers["Cache-Control"] = "no-cache";
        return Ok();
    }
}
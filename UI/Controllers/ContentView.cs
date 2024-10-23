using System.Threading.Tasks;
using Application.Features.ContentPages.Dtos;
using Application.Features.ContentPages.Queries;
using Domain;
using Domain.Enums;
using Domain.Helpers;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class ContentView : ViewComponent
{
    private readonly IMediator _mediator;

    public ContentView(IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task<IViewComponentResult> InvokeAsync(ContentPageDto _page)
    {
        switch (_page.TemplateType)
        {
            case TemplateType.HtmlRaw:
            {
                break;
            }
            case TemplateType.BlogList:
            {
                var query = new GetAllContentPageQuery
                {
                    PagedRequest = new PagedRequest<ContentPage>
                    {
                        Predicate = o => o.ParentId == _page.Id,
                        Cache = true,
                        OrderByColumn = "Id",
                        OrderByDescending = true,
                        PageNumber = 1,
                        PageSize = 25
                    }
                };
                var response = await _mediator.Send(query);
                return View(_page.TemplateType.ToStr(), response.Data.Items);
                break;
            }
            case TemplateType.BlogDetail:
            {
                break;
            }
            case TemplateType.EventContentList:
            {
                break;
            }
            case TemplateType.EventList:
            {
                var query = new GetAllContentPageQuery
                {
                    PagedRequest = new PagedRequest<ContentPage>
                    {
                        Predicate = o => o.ParentId == _page.Id,
                        Cache = true,
                        OrderByColumn = "Id",
                        OrderByDescending = true,
                        PageNumber = 1,
                        PageSize = 25
                    }
                };
                var response = await _mediator.Send(query);
                return View(_page.TemplateType.ToStr(), response.Data.Items);
                break;
            }
            case TemplateType.EventDetail:
            {
                break;
            }
            case TemplateType.Contact:
            {
                break;
            }
            default: break;
        }

        return View(_page.TemplateType.ToStr(), _page);
    }


  
}
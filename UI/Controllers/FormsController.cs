using System;
using System.Threading.Tasks;
using Application.Features.Forms.Commands;
using Application.Features.Forms.Dtos;
using Application.Features.Forms.Queries;
using Application.Features.FormTypes.Queries;
using Application.Features.LangDisplays.Queries;
using Application.Features.Langs.Queries;
using Application.Features.Users.Queries;
using Domain;
using Domain.Entities;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers;

public class FormsController : Controller
{
    IMediator mediator;

    public FormsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("admin/Forms/list")]
    public async Task<IActionResult> List()
    {
        return View();
    }


    [Route("admin/Forms/getpaging")]
    [HttpPost]
    public async Task<IActionResult> GetPaging([FromBody] PagedRequest<Forms> pagedRequest)
    {
        var req = new GetAllFormsQuery
        {
            PagedRequest = pagedRequest
        };
        var result = await mediator.Send(req);

        var paging = new PagedResponse<FormsDto>
        {
            data = result.Data.Items,
            draw = result.Data.PageNumber,
            recordsFiltered = result.Data.TotalCount,
            recordsTotal = result.Data.TotalCount,
            LastModifiedOn = DateTime.Now.ToString()
        };

        return Json(paging);
    }

    [Route("forms/updatecreate")]
    [HttpPost]
    public async Task<IActionResult> updatecreate(CreateFormsCommand command)
    {
        var result = await mediator.Send(command);
        return Json(result);
    }

    [Route("admin/Forms/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetFormsByIdQuery
        {
            Cache = !(HttpContext.Request.Query.TryGetValue("cache", out var cacheControl) &&
                      cacheControl == "false"),
            Id = id
        });
        return Json(result);
    }

    [Route("admin/Forms/delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeleteFormsCommand
        {
            Id = id
        });
        return Json(result);
    }

   
}
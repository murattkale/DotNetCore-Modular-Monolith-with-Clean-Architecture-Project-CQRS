using System.Threading.Tasks;
using Application.Features.ContentPages.Dtos;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class FormView : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ContentPageDto _page)
    {
        return View(_page.FormType.FormCode.ToStr(), _page);
    }
}
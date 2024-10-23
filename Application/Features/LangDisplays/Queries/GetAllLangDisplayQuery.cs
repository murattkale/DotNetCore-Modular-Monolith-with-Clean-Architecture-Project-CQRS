using Application.Features.LangDisplays.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.LangDisplays.Queries;

public class GetAllLangDisplayQuery : IRequest<ResponseModel<PagedResult<LangDisplayDto>>>
{
    public PagedRequest<LangDisplay> PagedRequest { get; set; }
}
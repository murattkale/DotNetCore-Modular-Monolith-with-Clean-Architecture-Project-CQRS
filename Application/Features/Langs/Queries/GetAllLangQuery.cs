using Application.Features.Langs.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Queries;

public class GetAllLangQuery : IRequest<ResponseModel<PagedResult<LangDto>>>
{
    public PagedRequest<Lang> PagedRequest { get; set; }
}
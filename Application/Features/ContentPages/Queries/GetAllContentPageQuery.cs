using Application.Features.ContentPages.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class GetAllContentPageQuery : IRequest<ResponseModel<PagedResult<ContentPageDto>>>
{
    public PagedRequest<ContentPage> PagedRequest { get; set; }
}
using Application.Features.ContentPages.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class GetContentPageByLinkQuery : IRequest<ResponseModel<ContentPageDto>>
{
    public required string Link { get; set; }
    public required bool Cache { get; set; }
}
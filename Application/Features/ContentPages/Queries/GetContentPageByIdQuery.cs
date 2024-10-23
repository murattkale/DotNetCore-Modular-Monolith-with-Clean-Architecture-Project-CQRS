using Application.Features.ContentPages.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class GetContentPageByIdQuery : IRequest<ResponseModel<ContentPageDto>>
{
    public GetContentPageByIdQuery(int id)
    {
        Id = id;
    }

    public GetContentPageByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
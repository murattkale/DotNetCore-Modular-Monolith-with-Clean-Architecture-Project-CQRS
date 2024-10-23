using Application.Features.ContentPages.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class GetContentPageByOrjIdQuery : IRequest<ResponseModel<ContentPageDto>>
{
    public GetContentPageByOrjIdQuery(int id)
    {
        Id = id;
    }

    public GetContentPageByOrjIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
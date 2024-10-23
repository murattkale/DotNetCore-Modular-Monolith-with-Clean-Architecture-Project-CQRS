using Application.Features.Documents.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Queries;

public class GetDocumentsByIdQuery : IRequest<ResponseModel<DocumentsDto>>
{
    public GetDocumentsByIdQuery(int id)
    {
        Id = id;
    }

    public GetDocumentsByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
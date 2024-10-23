using Application.Features.Documents.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Queries;

public class GetAllDocumentsQuery : IRequest<ResponseModel<PagedResult<DocumentsDto>>>
{
    public PagedRequest<dotnetcoreproject.Domain.Entities.Documents> PagedRequest { get; set; }
}
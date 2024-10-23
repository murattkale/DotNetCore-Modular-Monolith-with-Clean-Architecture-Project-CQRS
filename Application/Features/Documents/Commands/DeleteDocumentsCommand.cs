using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Commands;

public class DeleteDocumentsCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Commands;

public class DeleteContentPageCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}
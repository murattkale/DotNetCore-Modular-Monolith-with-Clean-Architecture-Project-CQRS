using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Commands;

public class DeleteFormsCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.FormTypes.Commands;

public class DeleteFormTypeCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}
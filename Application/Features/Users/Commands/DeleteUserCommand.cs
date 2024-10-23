using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Users.Commands;

public class DeleteUserCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}
#nullable enable
using Application.Features.Users.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<ResponseModel<UserDto>>
{
    public required string Mail { get; set; }
    public required string Pass { get; set; }
    public required string UserName { get; set; }
}
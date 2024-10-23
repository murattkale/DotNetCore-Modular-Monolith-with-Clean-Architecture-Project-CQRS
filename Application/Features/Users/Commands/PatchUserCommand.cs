using Application.Features.Users.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.Users.Commands;

public class PatchUserCommand : IRequest<ResponseModel<UserDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<User> PatchDocument { get; set; }
}
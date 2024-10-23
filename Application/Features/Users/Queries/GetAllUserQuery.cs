using Application.Features.Users.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries;

public class GetAllUserQuery : IRequest<ResponseModel<PagedResult<UserDto>>>
{
    public PagedRequest<User> PagedRequest { get; set; }
}
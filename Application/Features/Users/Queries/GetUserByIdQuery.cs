using Application.Features.Users.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<ResponseModel<UserDto>>
{
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }

    public GetUserByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
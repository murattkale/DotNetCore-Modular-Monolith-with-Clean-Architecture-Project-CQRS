using Application.Features.Forms.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Queries;

public class GetFormsByIdQuery : IRequest<ResponseModel<FormsDto>>
{
    public GetFormsByIdQuery(int id)
    {
        Id = id;
    }

    public GetFormsByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
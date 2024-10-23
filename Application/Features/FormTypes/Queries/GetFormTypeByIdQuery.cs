using Application.Features.FormTypes.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.FormTypes.Queries;

public class GetFormTypeByIdQuery : IRequest<ResponseModel<FormTypeDto>>
{
    public GetFormTypeByIdQuery(int id)
    {
        Id = id;
    }

    public GetFormTypeByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
using Application.Features.LangDisplays.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.LangDisplays.Queries;

public class GetLangDisplayByIdQuery : IRequest<ResponseModel<LangDisplayDto>>
{
    public GetLangDisplayByIdQuery(int id)
    {
        Id = id;
    }

    public GetLangDisplayByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
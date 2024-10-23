using Application.Features.Langs.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Queries;

public class GetLangByIdQuery : IRequest<ResponseModel<LangDto>>
{
    public GetLangByIdQuery(int id)
    {
        Id = id;
    }

    public GetLangByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}
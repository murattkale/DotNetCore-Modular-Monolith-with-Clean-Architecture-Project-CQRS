using Application.Features.Langs.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.Langs.Commands;

public class PatchLangCommand : IRequest<ResponseModel<LangDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<Lang> PatchDocument { get; set; }
}
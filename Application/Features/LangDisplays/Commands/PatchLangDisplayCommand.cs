using Application.Features.LangDisplays.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.LangDisplays.Commands;

public class PatchLangDisplayCommand : IRequest<ResponseModel<LangDisplayDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<LangDisplay> PatchDocument { get; set; }
}
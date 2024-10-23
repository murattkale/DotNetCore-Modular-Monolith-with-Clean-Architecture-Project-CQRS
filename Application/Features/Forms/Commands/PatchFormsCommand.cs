using Application.Features.Forms.Dtos;
using dotnetcoreproject.Domain;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.Forms.Commands;

public class PatchFormsCommand : IRequest<ResponseModel<FormsDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<Domain.Entities.Forms> PatchDocument { get; set; }
}
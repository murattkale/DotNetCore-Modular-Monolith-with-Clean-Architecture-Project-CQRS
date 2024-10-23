using Application.Features.FormTypes.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.FormTypes.Commands;

public class PatchFormTypeCommand : IRequest<ResponseModel<FormTypeDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<FormType> PatchDocument { get; set; }
}
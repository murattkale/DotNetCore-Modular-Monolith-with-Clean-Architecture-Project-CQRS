using Application.Features.Documents.Dtos;
using dotnetcoreproject.Domain;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.Documents.Commands;

public class PatchDocumentsCommand : IRequest<ResponseModel<DocumentsDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<dotnetcoreproject.Domain.Entities.Documents> PatchDocument { get; set; }
}
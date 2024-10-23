using Application.Features.ContentPages.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.ContentPages.Commands;

public class PatchContentPageCommand : IRequest<ResponseModel<ContentPageDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<ContentPage> PatchDocument { get; set; }
}
#nullable enable
using Application.Features.Langs.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Commands;

public class CreateLangCommand : IRequest<ResponseModel<LangDto>>
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required bool IsDefault { get; set; }
    public string? Logo { get; set; }
}
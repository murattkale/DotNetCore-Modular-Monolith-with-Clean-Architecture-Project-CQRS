#nullable enable
using Application.Features.LangDisplays.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.LangDisplays.Commands;

public class CreateLangDisplayCommand : IRequest<ResponseModel<LangDisplayDto>>
{
    public required string ParamName { get; set; }
    public required string Name_1 { get; set; }
    public string? Name_2 { get; set; }
    public string? Name_3 { get; set; }
    public string? Name_4 { get; set; }
    public string? Name_5 { get; set; }
    public string? Name_6 { get; set; }
    public string? Name_7 { get; set; }
    public string? Name_8 { get; set; }
    public string? Name_9 { get; set; }
    public string? Name_10 { get; set; }
    public string? Description { get; set; }
}
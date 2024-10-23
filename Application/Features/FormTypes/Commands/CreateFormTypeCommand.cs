#nullable enable
using Application.Features.FormTypes.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.FormTypes.Commands;

public class CreateFormTypeCommand : IRequest<ResponseModel<FormTypeDto>>
{
    public required string Name { get; set; }
    public string? FormCode { get; set; }
    public string? Desc { get; set; }
    public string? Mail { get; set; }
    public string? MailCC { get; set; }
}
#nullable enable
using Application.Features.Forms.Dtos;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Commands;

public class CreateFormsCommand : IRequest<ResponseModel<FormsDto>>
{
    public int FormTypeId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Mail { get; set; }
    public string? Phone { get; set; }
    public string? Adress { get; set; }
    public string? Subject { get; set; }
    public string? Message { get; set; }
}
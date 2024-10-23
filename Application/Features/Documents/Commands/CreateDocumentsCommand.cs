#nullable enable
using Application.Features.Documents.Dtos;
using Domain.Enums;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Commands;

public class CreateDocumentsCommand : IRequest<ResponseModel<DocumentsDto>>
{
    public int? ContentPageId { get; set; }
    public int? FormsId { get; set; }
    public required string ImageUrl { get; set; }
    public required DocType DocType { get; set; }
    public bool? IsMobile { get; set; }
    public bool? IsDesktop { get; set; }
}
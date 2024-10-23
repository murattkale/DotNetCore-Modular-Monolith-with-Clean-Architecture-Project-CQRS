#nullable enable
using Domain.Enums;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;

namespace Application.Features.Documents.Dtos;

public class DocumentsDto : BaseEntity
{
    public int? ContentPageId { get; set; }
    public ContentPage ContentPage { get; set; }
    public int? FormsId { get; set; }
    public Domain.Entities.Forms Forms { get; set; }
    public  string ImageUrl { get; set; }
    public  DocType DocType { get; set; }
    public bool? IsMobile { get; set; }
    public bool? IsDesktop { get; set; }
   
}
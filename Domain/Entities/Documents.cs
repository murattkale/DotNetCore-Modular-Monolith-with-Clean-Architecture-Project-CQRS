using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;

namespace dotnetcoreproject.Domain.Entities;

[Table("Documents")]
public class Documents : BaseEntity
{
    public int? ContentPageId { get; set; }

    [ForeignKey("ContentPageId")]
    [InverseProperty("Documents")]
    public ContentPage ContentPage { get; set; }

    public int? FormsId { get; set; }

    [ForeignKey("FormsId")]
    [InverseProperty("Documents")]
    public Forms Forms { get; set; }


    public required string ImageUrl { get; set; }
    public required DocType DocType { get; set; }
    public bool? IsMobile { get; set; }
    public bool? IsDesktop { get; set; }
}
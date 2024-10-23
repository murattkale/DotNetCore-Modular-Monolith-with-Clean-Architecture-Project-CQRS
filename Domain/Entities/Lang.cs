using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcoreproject.Domain.Entities;

[Table("Lang")]
public class Lang : BaseEntity
{
    [DisplayName("Dil Adı")] public required string Name { get; set; }
    [DisplayName("Dil Kodu")] public required string Code { get; set; }
    [DisplayName("Varsayılan Dil")] public required bool IsDefault { get; set; }

    [DisplayName("Lang Logo")] public string? Logo { get; set; }
}
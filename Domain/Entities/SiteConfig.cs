using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcoreproject.Domain.Entities;

[Table("SiteConfig")]
public class SiteConfig : BaseEntity
{
    public required string ConfigKey { get; set; }
    public required string ConfigValue { get; set; }
}
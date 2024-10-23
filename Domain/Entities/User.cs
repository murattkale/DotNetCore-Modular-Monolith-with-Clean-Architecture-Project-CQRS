using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcoreproject.Domain.Entities;

[Table("User")]
public class User : BaseEntity
{
    [DisplayName("Mail")] public required string Mail { get; set; }
    [DisplayName("Pass")] public required string Pass { get; set; }
    [DisplayName("Ad Soyad")] public required string NameSurname { get; set; }
}
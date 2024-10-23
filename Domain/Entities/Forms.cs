using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;

namespace Domain.Entities;

[Table("Forms")]
public class Forms : BaseEntity
{
    [ForeignKey("FormTypeId")]
    [InverseProperty("Forms")]
    [DisplayName("Form Tipi")]
    public FormType FormType { get; set; }
    [DisplayName("Form Tipi")] public int FormTypeId { get; set; }
    [DisplayName("Name")] public string? Name { get; set; }
    [DisplayName("Surname")] public string? Surname { get; set; }
    [DisplayName("Mail")] public string? Mail { get; set; }
    [DisplayName("Phone")] public string? Phone { get; set; }
    [DisplayName("Adress")] public string? Adress { get; set; }
    [DisplayName("Subject")] public string? Subject { get; set; }
    [DisplayName("Message")] public string? Message { get; set; }

    public List<Documents> Documents { get; set; } = new();
}
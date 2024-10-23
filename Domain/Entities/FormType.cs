using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace dotnetcoreproject.Domain.Entities;

[Table("FormType")]
public class FormType : BaseEntity
{
    [DisplayName("Name")] public required string Name { get; set; }
    [DisplayName("Form Kodu")] public string? FormCode { get; set; }
    [DisplayName("Desc")] public string? Desc { get; set; }
    [DisplayName("Mail")] public string? Mail { get; set; }
    [DisplayName("MailCC")] public string? MailCC { get; set; }

    public List<Forms> Forms { get; set; } = new();
    public List<ContentPage> ContentPages { get; set; } = new();
}
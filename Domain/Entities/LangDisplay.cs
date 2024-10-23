using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcoreproject.Domain.Entities;

[Table("LangDisplay")]
public class LangDisplay : BaseEntity
{
    [DisplayName("Paramater")] public required string ParamName { get; set; }

    public string Name_1 { get; set; }
    public string? Name_2 { get; set; }
    public string? Name_3 { get; set; }
    public string? Name_4 { get; set; }
    public string? Name_5 { get; set; }
    public string? Name_6 { get; set; }
    public string? Name_7 { get; set; }
    public string? Name_8 { get; set; }
    public string? Name_9 { get; set; }
    public string? Name_10 { get; set; }


    [DisplayName("Description")] public string? Description { get; set; }
}
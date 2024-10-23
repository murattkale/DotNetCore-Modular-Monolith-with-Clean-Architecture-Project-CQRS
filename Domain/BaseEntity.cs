using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcoreproject.Domain;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    [Required] public DateTime CreaDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public int? CreaUser { get; set; }
    public int? ModUser { get; set; }
    public bool? IsActive { get; set; }
    public int? OrderNo { get; set; }

    public int? LangId { get; set; }
}
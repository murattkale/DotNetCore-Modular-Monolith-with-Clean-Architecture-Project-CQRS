#nullable enable
using dotnetcoreproject.Domain;

namespace Application.Features.Langs.Dtos;

public class LangDto : BaseEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsDefault { get; set; }
    public string? Logo { get; set; }
}
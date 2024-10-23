#nullable enable
using System.Collections.Generic;
using Application.Features.Forms.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;

namespace Application.Features.FormTypes.Dtos;

public class FormTypeDto : BaseEntity
{
    public required string Name { get; set; }
    public required string FormCode { get; set; }
    public string? Desc { get; set; }
    public string? Mail { get; set; }
    public string? MailCC { get; set; }

    public List<FormsDto> Forms { get; set; } = new();
}
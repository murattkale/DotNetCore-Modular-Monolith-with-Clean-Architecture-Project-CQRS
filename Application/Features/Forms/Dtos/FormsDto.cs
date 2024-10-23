#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using Application.Features.Documents.Dtos;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;

namespace Application.Features.Forms.Dtos;

public class FormsDto : BaseEntity
{
    public FormType FormType { get; set; }
    [DisplayName("Form Tipi")] public int FormTypeId { get; set; }
    [DisplayName("Name")] public string? Name { get; set; }
    [DisplayName("Surname")] public string? Surname { get; set; }
    [DisplayName("Mail")] public string? Mail { get; set; }
    [DisplayName("Phone")] public string? Phone { get; set; }
    [DisplayName("Adress")] public string? Adress { get; set; }
    [DisplayName("Subject")] public string? Subject { get; set; }
    [DisplayName("Message")] public string? Message { get; set; }
    public List<DocumentsDto> Documents { get; set; } = new();
}
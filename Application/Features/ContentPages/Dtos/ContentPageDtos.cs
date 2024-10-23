#nullable enable
using System.Collections.Generic;
using System.Linq;
using Application.Features.Documents.Dtos;
using Domain.Enums;
using Domain.Helpers;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;

namespace Application.Features.ContentPages.Dtos;

public class ContentPageDto : BaseEntity
{
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public ContentTypes ContentTypes { get; set; }
    public TemplateType? TemplateType { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? ContentShort { get; set; }
    public string? ContentData { get; set; }
    public string? ExternalLink { get; set; }
    public int? OrjId { get; set; }
    public ContentPageDto? Orj { get; set; }
    public string? BannerText { get; set; }
    public string? BannerButtonText { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonTitle { get; set; }
    public string? ButtonLink { get; set; }
    public string? VideoLink { get; set; }
    public bool? IsSubMenu { get; set; }
    public bool? IsForm { get; set; }
    public bool? IsHeaderMenu { get; set; }
    public bool? IsFooterMenu { get; set; }
    public bool? IsHamburgerMenu { get; set; }
    public bool? IsSideMenu { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeyword { get; set; }
    public string? MetaDescription { get; set; }
    public int? ContentOrderNo { get; set; }
    public bool? IsPublish { get; set; }
    public bool? IsClick { get; set; }
    public string? TagManager { get; set; }
    public string? Location { get; set; }
    public string? LocationLink { get; set; }
    public string? Address { get; set; }
    public List<DocumentsDto>? Documents { get; set; } = new();
    public List<ContentPageDto>? Children { get; set; } = new();
    public ContentPageDto? Parent { get; set; }
    public string? OrjName { get; set; }

    public int? FormTypeId { get; set; }
    public  FormType? FormType { get; set; }
    public string? ImageUrlPrefix(DocType _DocType) => Documents?
        .FirstOrDefault(o => o.DocType == _DocType)?
        .ImageUrl?
        .AddUploadPrefix();
}
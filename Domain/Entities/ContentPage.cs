using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Helpers;

namespace dotnetcoreproject.Domain.Entities;

[Table("ContentPage")]
public class ContentPage : BaseEntity
{
    [DisplayName("Başlık")] public required string Name { get; set; }

    [DisplayName("Üst Kategori")] public int? ParentId { get; set; }

    [DisplayName("Üst Liste")]
    [ForeignKey("ParentId")]
    [InverseProperty("Children")]
    public virtual ContentPage? Parent { get; set; }

    [DisplayName("Orjinal Id")] public int? OrjId { get; set; }

    [DisplayName("Orjinal")]
    [ForeignKey("OrjId")]
    [InverseProperty("OrjRelation")]
    public virtual ContentPage? Orj { get; set; }


    [DisplayName("İçerik Tipi")] public required ContentTypes ContentTypes { get; set; }

    [NotMapped] public string ContentTypesName => ContentTypes.ExGetDescription();


    [DisplayName("Şablon Tipi")] public TemplateType? TemplateType { get; set; }


    [NotMapped] public string TemplateTypeName => TemplateType.ExGetDescription();


    [DisplayName("Sayfa Url")] public string? Link { get; set; }

    [DataType("text")]
    [DisplayName("Açıklama")]
    public string? Description { get; set; }

    [DataType("text")]
    [DisplayName("Kısa İçerik")]
    public string? ContentShort { get; set; }

    [DataType("text")]
    [DisplayName("İçerik")]
    public string? ContentData { get; set; }

    [DisplayName("Dış Url")] public string? ExternalLink { get; set; }

    [DisplayName("Banner Yazı")] public string? BannerText { get; set; }

    [DisplayName("Banner Button Yazı")] public string? BannerButtonText { get; set; }

    [DisplayName("Button Yazı")] public string? ButtonText { get; set; }

    [DisplayName("Button Başlık")] public string? ButtonTitle { get; set; }

    [DisplayName("Button Link")] public string? ButtonLink { get; set; }

    [DisplayName("Video Link")] public string? VideoLink { get; set; }

    [DisplayName("Alt Kategori")] public bool? IsSubMenu { get; set; }

    [DisplayName("Form Onayı")] public bool? IsForm { get; set; }

    [DisplayName("Üst Menü")] public bool? IsHeaderMenu { get; set; }

    [DisplayName("Alt Menü")] public bool? IsFooterMenu { get; set; }

    [DisplayName("Hamburger Menü")] public bool? IsHamburgerMenu { get; set; }

    [DisplayName("Yan Menü")] public bool? IsSideMenu { get; set; }

    [DisplayName("Meta Title")] public string? MetaTitle { get; set; }

    [DisplayName("Meta Keywords")] public string? MetaKeyword { get; set; }

    [DisplayName("Meta Description")] public string? MetaDescription { get; set; }

    [DisplayName("İçerik Sırası")] public int? ContentOrderNo { get; set; }

    [DisplayName("Yayına Alma Durumu")] public bool? IsPublish { get; set; }

    [DisplayName("Tıklanabilir")] public bool? IsClick { get; set; }

    [DataType(DataType.Html)]
    [DisplayName("Tag Manager")]
    public string? TagManager { get; set; }

    [DataType("Location")] public string? Location { get; set; }

    public string? LocationLink { get; set; }

    [DataType("text")]
    [DisplayName("Address")]
    public string? Address { get; set; }

    
    [ForeignKey("FormTypeId")]
    [InverseProperty("ContentPages")]
    [DisplayName("Form Tipi")]
    public int? FormTypeId { get; set; }

    [DisplayName("Form Tipi")]
    public  FormType FormType { get; set; }
    
    public List<Documents> Documents { get; set; } = new();

    public List<ContentPage>? Children { get; set; } = new();

    public List<ContentPage>? OrjRelation { get; set; } = new();
}
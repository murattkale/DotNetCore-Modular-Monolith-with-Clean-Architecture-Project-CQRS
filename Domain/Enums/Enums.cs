using System.ComponentModel;

namespace Domain.Enums;

public enum ContentTypes : int
{
    [Description("Genel Sayfalar")] GeneralPage = 1,
    [Description("Anasayfa")] MainPage = 2,
    [Description("Blog")] Blog = 3,
    [Description("Slider")] Slider = 4,
    [Description("Footer")] FooterPage = 5,
    [Description("Etkinlikler")] Events = 6,
}

public enum TemplateType : int
{
    [Description("Boş Template")] None = 0,
    [Description("Static HTML")] HtmlRaw = 2,
    [Description("Blog Listeleme")] BlogList = 3,
    [Description("Blog Detay")] BlogDetail = 4,
    [Description("Etkinlik Listesi")] EventList = 5,
    [Description("Etkinlik İçerik Listesi")] EventContentList = 6,
    [Description("Etkinlik Detayı")] EventDetail = 7,
    [Description("İletişim")] Contact = 8,
}

public enum DocType
{
    [Description("Görsel")] Picture = 1,
    [Description("Ön Görsel")] Thumb = 2,
    [Description("Banner")] Banner = 3,
    [Description("Galeri")] Gallery = 3,
    [Description("Döküman")] Document = 4
}
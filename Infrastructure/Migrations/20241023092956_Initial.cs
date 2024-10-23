using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LangDisplay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangDisplay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    OrjId = table.Column<int>(type: "int", nullable: true),
                    ContentTypes = table.Column<int>(type: "int", nullable: false),
                    TemplateType = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentShort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubMenu = table.Column<bool>(type: "bit", nullable: true),
                    IsForm = table.Column<bool>(type: "bit", nullable: true),
                    IsHeaderMenu = table.Column<bool>(type: "bit", nullable: true),
                    IsFooterMenu = table.Column<bool>(type: "bit", nullable: true),
                    IsHamburgerMenu = table.Column<bool>(type: "bit", nullable: true),
                    IsSideMenu = table.Column<bool>(type: "bit", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentOrderNo = table.Column<int>(type: "int", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    IsClick = table.Column<bool>(type: "bit", nullable: true),
                    TagManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormTypeId = table.Column<int>(type: "int", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentPage_ContentPage_OrjId",
                        column: x => x.OrjId,
                        principalTable: "ContentPage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentPage_ContentPage_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ContentPage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentPage_FormType_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_FormType_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentPageId = table.Column<int>(type: "int", nullable: true),
                    FormsId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    IsMobile = table.Column<bool>(type: "bit", nullable: true),
                    IsDesktop = table.Column<bool>(type: "bit", nullable: true),
                    CreaDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreaUser = table.Column<int>(type: "int", nullable: true),
                    ModUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    LangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_ContentPageId",
                        column: x => x.ContentPageId,
                        principalTable: "ContentPage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Forms_FormsId",
                        column: x => x.FormsId,
                        principalTable: "Forms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_FormTypeId",
                table: "ContentPage",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_OrjId",
                table: "ContentPage",
                column: "OrjId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_ParentId",
                table: "ContentPage",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ContentPageId",
                table: "Documents",
                column: "ContentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FormsId",
                table: "Documents",
                column: "FormsId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FormTypeId",
                table: "Forms",
                column: "FormTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Lang");

            migrationBuilder.DropTable(
                name: "LangDisplay");

            migrationBuilder.DropTable(
                name: "SiteConfig");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ContentPage");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "FormType");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.TenantConfig
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tenant = table.Column<string>(maxLength: 200, nullable: false),
                    EmailVerification = table.Column<bool>(nullable: false),
                    FirstPartyRequired = table.Column<bool>(nullable: false),
                    IsSecondPageRegistration = table.Column<bool>(nullable: false),
                    SecondPageRegistrationUrl = table.Column<string>(maxLength: 500, nullable: true),
                    ContactUsUrl = table.Column<string>(maxLength: 200, nullable: true),
                    SiteUrl = table.Column<string>(maxLength: 200, nullable: true),
                    LoginUrl = table.Column<string>(maxLength: 200, nullable: true),
                    GoogleTagManagerId = table.Column<string>(maxLength: 100, nullable: true),
                    OptimizelyId = table.Column<string>(maxLength: 100, nullable: true),
                    PermutiveProjectId = table.Column<string>(maxLength: 100, nullable: true),
                    PermutiveApiKey = table.Column<string>(maxLength: 100, nullable: true),
                    RegisterUrl = table.Column<string>(maxLength: 100, nullable: true),
                    IsVanityUrl = table.Column<bool>(nullable: false),
                    ExternalRegistrationUrl = table.Column<string>(maxLength: 100, nullable: true),
                    StyleManifestUrl = table.Column<string>(maxLength: 100, nullable: true),
                    GoogleAnalyticsId = table.Column<string>(maxLength: 100, nullable: true),
                    ReCaptchaSiteKey = table.Column<string>(maxLength: 100, nullable: true),
                    ReCaptchaSecretKey = table.Column<string>(maxLength: 100, nullable: true),
                    IsReCaptchaEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantConfigurationId = table.Column<Guid>(nullable: false),
                    TypeName = table.Column<string>(maxLength: 200, nullable: false),
                    Text = table.Column<string>(maxLength: 255, nullable: true),
                    Lead = table.Column<string>(maxLength: 255, nullable: true),
                    IsVisibleOnRegistration = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimTypes_TenantConfigurations_TenantConfigurationId",
                        column: x => x.TenantConfigurationId,
                        principalTable: "TenantConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantConfigurationId = table.Column<Guid>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    ConsumerKey = table.Column<string>(maxLength: 200, nullable: false),
                    Secret = table.Column<string>(maxLength: 200, nullable: false),
                    AuthenticationType = table.Column<string>(maxLength: 200, nullable: false),
                    Caption = table.Column<string>(maxLength: 200, nullable: false),
                    Callback = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalProviders_TenantConfigurations_TenantConfigurationId",
                        column: x => x.TenantConfigurationId,
                        principalTable: "TenantConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantConfigurationId = table.Column<Guid>(nullable: false),
                    RequireDigit = table.Column<bool>(nullable: false),
                    RequireUpperCase = table.Column<bool>(nullable: false),
                    RequireLowerCase = table.Column<bool>(nullable: false),
                    RequireNonAlphaNumeric = table.Column<bool>(nullable: false),
                    MinimumLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordPolicies_TenantConfigurations_TenantConfigurationId",
                        column: x => x.TenantConfigurationId,
                        principalTable: "TenantConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_TypeName",
                table: "ClaimTypes",
                columns: new[] { "TenantConfigurationId", "TypeName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalProviders_TenantConfigurationId",
                table: "ExternalProviders",
                column: "TenantConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordPolicies_TenantConfigurationId",
                table: "PasswordPolicies",
                column: "TenantConfigurationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenant",
                table: "TenantConfigurations",
                column: "Tenant",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimTypes");

            migrationBuilder.DropTable(
                name: "ExternalProviders");

            migrationBuilder.DropTable(
                name: "PasswordPolicies");

            migrationBuilder.DropTable(
                name: "TenantConfigurations");
        }
    }
}

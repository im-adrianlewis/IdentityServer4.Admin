using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class UserClaimUpdatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClaimUpdatedDate",
                table: "UserClaims",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimUpdatedDate",
                table: "UserClaims");
        }
    }
}

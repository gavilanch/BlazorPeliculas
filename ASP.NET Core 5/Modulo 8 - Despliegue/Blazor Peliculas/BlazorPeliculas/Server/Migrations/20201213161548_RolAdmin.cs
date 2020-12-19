using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorPeliculas.Server.Migrations
{
    public partial class RolAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'89086180-b978-4f90-9dbd-a7040bc93f41', N'admin', N'admin');
");    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete AspNetRoles
             where id = '89086180-b978-4f90-9dbd-a7040bc93f41'");
        }
    }
}

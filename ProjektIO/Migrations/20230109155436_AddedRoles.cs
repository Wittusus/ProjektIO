using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIO.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE VIEW UserRolesIndex AS SELECT aspnetroles.Name, aspnetusers.UserName, aspnetuserroles.RoleId, aspnetuserroles.UserId FROM aspnetuserroles INNER JOIN aspnetroles ON aspnetroles.Id = aspnetuserroles.RoleId INNER JOIN aspnetusers ON aspnetuserroles.UserId = aspnetusers.Id;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW UserRolesIndex;");
        }
    }
}

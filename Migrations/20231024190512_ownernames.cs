using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_il_mio_fotoalbum.Migrations
{
    public partial class ownernames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerID",
                table: "Message",
                newName: "OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Message",
                newName: "OwnerID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerManagementHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_cusomters",
                table: "cusomters");

            migrationBuilder.RenameTable(
                name: "cusomters",
                newName: "customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "cusomters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cusomters",
                table: "cusomters",
                column: "_id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahmed_Task.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedseedreceptionrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b", "9a1c6b5e-2d3f-4b6b-91c8-3f2e1c1e5a6g", "Receptionist", "RECEPTIONIST" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b");
        }
    }
}

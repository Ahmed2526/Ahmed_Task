using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahmed_Task.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedseedreceptionuserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b", "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e5a6c5f2-1s4a-4f9d-9b83-2f69c11a8f3b", "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d" });
        }
    }
}

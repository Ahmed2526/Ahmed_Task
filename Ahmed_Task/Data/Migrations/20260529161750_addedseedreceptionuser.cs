using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahmed_Task.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedseedreceptionuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d", 0, "1e8as2a5-6f2e-4f9f-9a31-6c4e6d1a8a4f", "receptionist@receptionist.test", true, false, null, "RECEPTIONIST@RECEPTIONIST.TEST", "RECEPTIONIST", "AQAAAAIAAYagAAAAEBnyKXu3Y3Gi3XA25xx6ZKdZPoQl6YFtZrJva8U7EFX8WysYVFR8283XucvWa+4TnA==", null, false, "5b8a8ab5-0a34-4d40-9d18-8d2d6b2d9c2a", false, "receptionist" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c0d1b7a-3a8f-4d7c-4s2a-3b3b3e0c8f5d");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ahmed_Task.Data.Migrations
{
    /// <inheritdoc />
    public partial class seeddefaultadminandrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1", "f4d5a2b3-7c4f-4e32-8b3f-0ef2b7a0d9c1", "Admin", "ADMIN" },
                    { "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b", "9a1c7b5e-2d3f-4a6b-91b8-3f2d1c0e7a6b", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d", 0, "1e8aa2a5-6f6e-4f9f-9b31-6c4e6c1a8f4f", "admin@admin.test", true, false, null, "ADMIN@ADMIN.TEST", "ADMIN", "AQAAAAIAAYagAAAAEBnyKXu3Y3Gi3XA25xx6ZKdZPoQl6YFtZrJva8U7EFX8WysYVFR8283XucvWa+4TnA==", null, false, "5b8a8bb5-0a44-4d30-9d18-8d0d6b2a9c2a", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1", "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d" },
                    { "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b", "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1", "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b", "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3d4c7b0-7d9b-4b5d-9e54-3b29b6d7f2a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5a6c5f2-1b4a-4f9d-9a83-2f29c11c8f3b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c0d1b7a-3b8f-4c7c-8e2a-3b3b3e0c8f5d");
        }
    }
}

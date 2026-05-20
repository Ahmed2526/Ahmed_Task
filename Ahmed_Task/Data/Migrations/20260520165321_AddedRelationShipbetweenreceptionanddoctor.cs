using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahmed_Task.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationShipbetweenreceptionanddoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceptionistDoctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ReceptionistId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionistDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionistDoctors_AspNetUsers_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionistDoctors_ReceptionistId",
                table: "ReceptionistDoctors",
                column: "ReceptionistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceptionistDoctors");
        }
    }
}

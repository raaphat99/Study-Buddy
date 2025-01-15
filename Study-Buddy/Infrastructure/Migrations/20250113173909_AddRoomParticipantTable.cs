using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomParticipantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rooms",
                newName: "HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                newName: "IX_Rooms_HostId");

            migrationBuilder.CreateTable(
                name: "RoomParticipant",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomParticipant", x => new { x.RoomId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_RoomParticipant_AspNetUsers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RoomParticipant_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomParticipant_ParticipantId",
                table: "RoomParticipant",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_HostId",
                table: "Rooms",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_HostId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomParticipant");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "Rooms",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HostId",
                table: "Rooms",
                newName: "IX_Rooms_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

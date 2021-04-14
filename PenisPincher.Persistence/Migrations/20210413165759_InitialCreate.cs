using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenisPincher.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordServerConfiguration",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StreamNotificationChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LogToServer = table.Column<bool>(type: "bit", nullable: false),
                    ServerLogChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServerConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoredStream",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StreamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwningServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    NotificationTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationRoleIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredStream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoredStream_DiscordServerConfiguration_OwningServerId",
                        column: x => x.OwningServerId,
                        principalTable: "DiscordServerConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionRole",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    EmoteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    OwningServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRole_DiscordServerConfiguration_OwningServerId",
                        column: x => x.OwningServerId,
                        principalTable: "DiscordServerConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredStream_OwningServerId",
                table: "MonitoredStream",
                column: "OwningServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRole_OwningServerId",
                table: "ReactionRole",
                column: "OwningServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredStream");

            migrationBuilder.DropTable(
                name: "ReactionRole");

            migrationBuilder.DropTable(
                name: "DiscordServerConfiguration");
        }
    }
}

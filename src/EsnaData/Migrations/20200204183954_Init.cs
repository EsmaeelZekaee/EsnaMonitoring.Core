using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsnaData.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    BaudRate = table.Column<int>(nullable: false),
                    Timeout = table.Column<int>(nullable: false),
                    DataBits = table.Column<int>(nullable: false),
                    Parity = table.Column<int>(nullable: false),
                    PortName = table.Column<string>(nullable: true),
                    StopBits = table.Column<int>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    MacAddress = table.Column<string>(nullable: true),
                    FirstRegister = table.Column<byte>(nullable: false),
                    Offset = table.Column<byte>(nullable: false),
                    UnitId = table.Column<byte>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    ExteraInfornamtion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    DeviceId = table.Column<long>(nullable: false),
                    Function = table.Column<byte>(nullable: false),
                    Executed = table.Column<bool>(nullable: false),
                    ExecutedOnUtc = table.Column<bool>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Command_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recorde",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    DeviceId = table.Column<long>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true),
                    ConfigurationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recorde", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recorde_Configuration_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recorde_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Command_DeviceId",
                table: "Command",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Recorde_ConfigurationId",
                table: "Recorde",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Recorde_DeviceId",
                table: "Recorde",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropTable(
                name: "Recorde");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}

namespace EsnaData.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Init : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Command");

            migrationBuilder.DropTable("Recorde");

            migrationBuilder.DropTable("Configuration");

            migrationBuilder.DropTable("Device");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Configuration",
                table => new
                             {
                                 Id = table.Column<long>().Annotation("Sqlite:Autoincrement", true),
                                 CreatedOnUtc = table.Column<DateTime>(),
                                 BaudRate = table.Column<int>(),
                                 Timeout = table.Column<int>(),
                                 DataBits = table.Column<int>(),
                                 Parity = table.Column<int>(),
                                 PortName = table.Column<string>(nullable: true),
                                 StopBits = table.Column<int>(),
                                 Mode = table.Column<int>(),
                                 Active = table.Column<bool>()
                             },
                constraints: table => { table.PrimaryKey("PK_Configuration", x => x.Id); });

            migrationBuilder.CreateTable(
                "Device",
                table => new
                             {
                                 Id = table.Column<long>().Annotation("Sqlite:Autoincrement", true),
                                 CreatedOnUtc = table.Column<DateTime>(),
                                 MacAddress = table.Column<string>(nullable: true),
                                 FirstRegister = table.Column<byte>(),
                                 Offset = table.Column<byte>(),
                                 UnitId = table.Column<byte>(),
                                 Code = table.Column<string>(nullable: true),
                                 ExteraInfornamtion = table.Column<string>(nullable: true)
                             },
                constraints: table => { table.PrimaryKey("PK_Device", x => x.Id); });

            migrationBuilder.CreateTable(
                "Command",
                table => new
                             {
                                 Id = table.Column<long>().Annotation("Sqlite:Autoincrement", true),
                                 CreatedOnUtc = table.Column<DateTime>(),
                                 DeviceId = table.Column<long>(),
                                 Function = table.Column<byte>(),
                                 Executed = table.Column<bool>(),
                                 ExecutedOnUtc = table.Column<bool>(),
                                 Data = table.Column<byte[]>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Command", x => x.Id);
                        table.ForeignKey(
                            "FK_Command_Device_DeviceId",
                            x => x.DeviceId,
                            "Device",
                            "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateTable(
                "Recorde",
                table => new
                             {
                                 Id = table.Column<long>().Annotation("Sqlite:Autoincrement", true),
                                 CreatedOnUtc = table.Column<DateTime>(),
                                 DeviceId = table.Column<long>(),
                                 Data = table.Column<byte[]>(nullable: true),
                                 ConfigurationId = table.Column<long>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Recorde", x => x.Id);
                        table.ForeignKey(
                            "FK_Recorde_Configuration_ConfigurationId",
                            x => x.ConfigurationId,
                            "Configuration",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            "FK_Recorde_Device_DeviceId",
                            x => x.DeviceId,
                            "Device",
                            "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateIndex("IX_Command_DeviceId", "Command", "DeviceId");

            migrationBuilder.CreateIndex("IX_Recorde_ConfigurationId", "Recorde", "ConfigurationId");

            migrationBuilder.CreateIndex("IX_Recorde_DeviceId", "Recorde", "DeviceId");
        }
    }
}
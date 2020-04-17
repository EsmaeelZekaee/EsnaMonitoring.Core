namespace EsnaData.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Init1 : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("IsActive", "Device");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsActive", "Device", nullable: false, defaultValue: false);
        }
    }
}
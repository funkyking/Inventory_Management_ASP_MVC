using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvMng_InfTech.Migrations
{
    /// <inheritdoc />
    public partial class PartsMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartsMaster",
                columns: table => new
                {
                    PartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinNew = table.Column<int>(type: "int", nullable: true),
                    MinUsed = table.Column<int>(type: "int", nullable: true),
                    Bin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsMaster", x => x.PartID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartsMaster");
        }
    }
}

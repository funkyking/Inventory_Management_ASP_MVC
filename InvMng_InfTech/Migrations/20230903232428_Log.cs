using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvMng_InfTech.Migrations
{
    /// <inheritdoc />
    public partial class Log : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogMaster",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockInOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExistingStockNew = table.Column<int>(type: "int", nullable: true),
                    ExistingStockUsed = table.Column<int>(type: "int", nullable: true),
                    StockNew = table.Column<int>(type: "int", nullable: true),
                    StockUsed = table.Column<int>(type: "int", nullable: true),
                    LocationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubLocationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogMaster", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogMaster");
        }
    }
}

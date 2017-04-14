using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPIIKcom.Data.Migrations
{
    public partial class AddedStaticPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaticContents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Namn = table.Column<string>(nullable: true),
                    Text1 = table.Column<string>(nullable: true),
                    Text10 = table.Column<string>(nullable: true),
                    Text2 = table.Column<string>(nullable: true),
                    Text3 = table.Column<string>(nullable: true),
                    Text4 = table.Column<string>(nullable: true),
                    Text5 = table.Column<string>(nullable: true),
                    Text6 = table.Column<string>(nullable: true),
                    Text7 = table.Column<string>(nullable: true),
                    Text8 = table.Column<string>(nullable: true),
                    Text9 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticContents", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaticContents");
        }
    }
}

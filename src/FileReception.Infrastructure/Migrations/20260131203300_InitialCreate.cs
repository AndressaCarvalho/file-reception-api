using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FileReception.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileLayoutFieldType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLayoutFieldType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileLayout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileLayout_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileLayoutId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_FileLayout_FileLayoutId",
                        column: x => x.FileLayoutId,
                        principalTable: "FileLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_FileStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FileStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileLayoutField",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileLayoutId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Start = table.Column<int>(type: "int", nullable: false),
                    End = table.Column<int>(type: "int", nullable: false),
                    FileLayoutFieldTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLayoutField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileLayoutField_FileLayoutFieldType_FileLayoutFieldTypeId",
                        column: x => x.FileLayoutFieldTypeId,
                        principalTable: "FileLayoutFieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileLayoutField_FileLayout_FileLayoutId",
                        column: x => x.FileLayoutId,
                        principalTable: "FileLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    FilePathBackup = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileProcess_File_FileId",
                        column: x => x.FileId,
                        principalTable: "File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FileLayoutFieldType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ALFA" },
                    { 2, "NUM" }
                });

            migrationBuilder.InsertData(
                table: "FileStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Não Recepcionado" },
                    { 2, "Recepcionado" },
                    { 3, "Erro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_FileLayoutId",
                table: "File",
                column: "FileLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_File_StatusId",
                table: "File",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLayout_CompanyId",
                table: "FileLayout",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLayoutField_FileLayoutFieldTypeId",
                table: "FileLayoutField",
                column: "FileLayoutFieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLayoutField_FileLayoutId",
                table: "FileLayoutField",
                column: "FileLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLayoutFieldType_Name",
                table: "FileLayoutFieldType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileProcess_FileId",
                table: "FileProcess",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileStatus_Name",
                table: "FileStatus",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileLayoutField");

            migrationBuilder.DropTable(
                name: "FileProcess");

            migrationBuilder.DropTable(
                name: "FileLayoutFieldType");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "FileLayout");

            migrationBuilder.DropTable(
                name: "FileStatus");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}

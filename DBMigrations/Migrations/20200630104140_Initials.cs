using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBMigrations.Migrations
{
    public partial class Initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    OperationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Roles = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    ObjectDescription = table.Column<string>(type: "jsonb", nullable: true),
                    ObjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    EntityType = table.Column<int>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SrbacRolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Permission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrbacRolePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "true"),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    NameFirst = table.Column<string>(maxLength: 20, nullable: false),
                    NameSecond = table.Column<string>(maxLength: 20, nullable: false),
                    NamePatronymic = table.Column<string>(maxLength: 20, nullable: true, defaultValueSql: "null"),
                    Phone = table.Column<string>(maxLength: 20, nullable: true, defaultValueSql: "null"),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Roles = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DateExpiration = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    ReasonId = table.Column<int>(nullable: false),
                    UserModelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Codes_Users_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    CreatorId = table.Column<Guid>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DateDelete = table.Column<DateTime>(nullable: true),
                    AppVersion = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true, defaultValueSql: "null"),
                    Token = table.Column<string>(nullable: false),
                    PushToken = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_ObjectId",
                table: "Audits",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_OperationType",
                table: "Audits",
                column: "OperationType");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Roles",
                table: "Audits",
                column: "Roles");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Status",
                table: "Audits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_Code",
                table: "Codes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_UserModelId",
                table: "Codes",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_EntityId",
                table: "Files",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SrbacRolePermissions_Role_Permission",
                table: "SrbacRolePermissions",
                columns: new[] { "Role", "Permission" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "SrbacRolePermissions");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

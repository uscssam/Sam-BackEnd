using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using SAM.Repositories.Database.Context;
using System.Text;

#nullable disable

namespace SAM.Repositories.Database.Migrations
{
    /// <inheritdoc />
    public partial class EncryptUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Users_IdTechnician",
                table: "OrderServices");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "IdTechnician",
                table: "OrderServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Users_IdTechnician",
                table: "OrderServices",
                column: "IdTechnician",
                principalTable: "Users",
                principalColumn: "IdUser");

            // Obter todos os usuários
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            using var context = new MySqlContext(configuration);
            var users = context.User.Select(u => new { u.Id, u.Password }).ToList();

            foreach (var user in users)
            {
                var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt());
                migrationBuilder.Sql($"UPDATE `Users` SET `Password` = '{encryptedPassword}' WHERE `IdUser` = {user.Id}");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Users_IdTechnician",
                table: "OrderServices");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<int>(
                name: "IdTechnician",
                table: "OrderServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Users_IdTechnician",
                table: "OrderServices",
                column: "IdTechnician",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

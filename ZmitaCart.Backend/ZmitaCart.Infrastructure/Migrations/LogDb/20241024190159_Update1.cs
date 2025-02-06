using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Migrations.LogDb
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_Timestamp",
                table: "UserLogs",
                column: "Timestamp")
                .Annotation("SqlServer:Include", new[] { "Id", "Action", "IsSuccess", "Details", "IpAddress", "UserAgent", "UserId", "UserEmail" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLogs_Timestamp",
                table: "UserLogs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAttendencePolicyActionRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeAttendenceId",
                table: "AttendencePolicyActions");

            migrationBuilder.CreateIndex(
                name: "IX_AttendencePolicyActions_AttendenceId",
                table: "AttendencePolicyActions",
                column: "AttendenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendencePolicyActions_EmployeeAttendences_AttendenceId",
                table: "AttendencePolicyActions",
                column: "AttendenceId",
                principalTable: "EmployeeAttendences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendencePolicyActions_Policies_PolicyId",
                table: "AttendencePolicyActions",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendencePolicyActions_EmployeeAttendences_AttendenceId",
                table: "AttendencePolicyActions");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendencePolicyActions_Policies_PolicyId",
                table: "AttendencePolicyActions");

            migrationBuilder.DropIndex(
                name: "IX_AttendencePolicyActions_AttendenceId",
                table: "AttendencePolicyActions");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeAttendenceId",
                table: "AttendencePolicyActions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AttendencePolicyActions_EmployeeAttendenceId",
                table: "AttendencePolicyActions",
                column: "EmployeeAttendenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendencePolicyActions_EmployeeAttendences_EmployeeAttenden~",
                table: "AttendencePolicyActions",
                column: "EmployeeAttendenceId",
                principalTable: "EmployeeAttendences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendencePolicyActions_Policies_PolicyId",
                table: "AttendencePolicyActions",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

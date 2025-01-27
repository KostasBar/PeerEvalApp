using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PeerEvalAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "InsertedAt", "Text" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 26, 23, 42, 39, 627, DateTimeKind.Local).AddTicks(8714), "Self-Confidence" },
                    { 2, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9080), "Dedication" },
                    { 3, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9088), "Job Knowledge" },
                    { 4, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9092), "Quality and Accuracy of Work" },
                    { 5, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9096), "Ability to Meet Deadlines" },
                    { 6, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9099), "Independence" },
                    { 7, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9102), "Commitment" },
                    { 8, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9105), "Attention to Detail" },
                    { 9, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9108), "Ability to Work with Others" },
                    { 10, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9112), "Communication Skills" },
                    { 11, new DateTime(2025, 1, 26, 23, 42, 39, 642, DateTimeKind.Local).AddTicks(9115), "Performs Assigned Duties Under Pressure" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "GroupId", "LastName", "ManagerId", "Password", "Role" },
                values: new object[] { 1, "admin@example.com", "Admin", 1, "User", null, "$2b$10$V9/sbgVrXGjb7a/ZZwZFueDoGD0hC8GPVyqjWH53yEnkysAXqfnvy", "Admin" });
        }
    }
}

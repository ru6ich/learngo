using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace learngo.Migrations
{
    /// <inheritdoc />
    public partial class CreateRailwaySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "Topics",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "SectionName" },
                values: new object[,]
                {
                    { 1, "Основы Go" },
                    { 2, "Конкурентность в Go" },
                    { 3, "Веб-разработка на Go" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "AuthorEmail", "AuthorName", "created_at", "Description", "Difficulty", "SectionId", "TimeLimit", "TopicName" },
                values: new object[,]
                {
                    { 1, "alex@example.com", "Алексей Смирнов", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Основы синтаксиса и установка Go", 1, 1, 60, "Введение в Go" },
                    { 2, "maria@example.com", "Мария Петрова", new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Работа с переменными и базовыми типами в Go", 1, 1, 45, "Переменные и типы данных" },
                    { 3, "dmitry@example.com", "Дмитрий Иванов", new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Использование структур и интерфейсов в Go", 2, 1, 90, "Структуры и интерфейсы" },
                    { 4, "elena@example.com", "Елена Соколова", new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Основы конкурентности с использованием горутин", 2, 2, 75, "Горутины" },
                    { 5, "sergey@example.com", "Сергей Кузнецов", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Синхронизация с помощью каналов в Go", 3, 2, 90, "Каналы" },
                    { 6, "olga@example.com", "Ольга Николаева", new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Реализация пула работников с горутинами", 3, 2, 120, "Паттерн Worker Pool" },
                    { 7, "igor@example.com", "Игорь Васильев", new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Разработка RESTful API с пакетом net/http", 2, 3, 120, "Создание REST API" },
                    { 8, "tatiana@example.com", "Татьяна Морозова", new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Создание веб-приложений с фреймворком Gin", 2, 3, 90, "Работа с Gin" },
                    { 9, "pavel@example.com", "Павел Соколов", new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Использование промежуточного ПО в веб-приложениях", 3, 3, 105, "Middleware в Go" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "Topics",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}

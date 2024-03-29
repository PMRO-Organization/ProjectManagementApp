﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Databases.App.Migrations;

public partial class Init : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "TodoLists",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_TodoLists", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "Tasks",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
				Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
				DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
				Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
				TodoListModelId = table.Column<int>(type: "int", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Tasks", x => x.Id);
				table.ForeignKey(
					name: "FK_Tasks_TodoLists_TodoListModelId",
					column: x => x.TodoListModelId,
					principalTable: "TodoLists",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Tasks_TodoListModelId",
			table: "Tasks",
			column: "TodoListModelId");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Tasks");

		migrationBuilder.DropTable(
			name: "TodoLists");
	}
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Databases.App.Migrations;

public partial class updateforstatustypetokeepitasstring : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Status",
			table: "Tasks",
			type: "nvarchar(max)",
			nullable: false,
			oldClrType: typeof(int),
			oldType: "int");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<int>(
			name: "Status",
			table: "Tasks",
			type: "int",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)");
	}
}

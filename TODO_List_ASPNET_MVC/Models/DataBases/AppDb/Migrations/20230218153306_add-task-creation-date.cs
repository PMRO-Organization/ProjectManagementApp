﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TODO_List_ASPNET_MVC.Models.DataBase.Migrations
{
    public partial class addtaskcreationdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Tasks");
        }
    }
}

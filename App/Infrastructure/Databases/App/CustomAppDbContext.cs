﻿using App.Common.Helpers;
using App.Features.Tags.Common;
using App.Features.Tasks.Common;
using App.Features.Tasks.Common.TaskTags.Common;
using App.Features.TodoLists.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static App.Features.Tasks.Common.TaskStatusHelper;

namespace App.Infrastructure.Databases.App;

/// <summary>
/// Custom Db Context for main app data.
/// </summary>
public class CustomAppDbContext : DbContext
{
	private readonly ILogger<CustomAppDbContext>? _logger;

	/// <summary>
	/// Initilizes object and Ensures that database is created.
	/// </summary>
	public CustomAppDbContext(DbContextOptions<CustomAppDbContext> options, ILogger<CustomAppDbContext> logger) : base(options)
	{
		_logger = logger;
	}

	public CustomAppDbContext()
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<TodoListModel>()
		.HasMany(list => (ICollection<TaskModel>)list.Tasks)
		.WithOne()
		.HasForeignKey(x => x.Id);

		modelBuilder.Entity<TodoListModel>()
			.ToTable("TodoLists");

		modelBuilder.Entity<TaskModel>()
			.HasMany(task => (ICollection<TaskTagModel>)task.TaskTags)
			.WithOne()
			.HasForeignKey(x => x.TaskId)
			.HasForeignKey(x => x.TagId);

		modelBuilder.Entity<TaskModel>()
			.HasOne(task => (TodoListModel?)task.TodoList)
			.WithMany(x => (ICollection<TaskModel>)x.Tasks)
			.HasForeignKey(x => x.TodoListId);

		modelBuilder.Entity<TaskModel>()
			.ToTable("Tasks");

		modelBuilder.Entity<TagModel>()
			.HasMany(tag => (ICollection<TaskTagModel>)tag.TaskTags)
			.WithOne(x => (TagModel)x.Tag)
			.HasForeignKey(x => x.TagId)
			.HasForeignKey(x => x.TaskId);

		modelBuilder.Entity<TagModel>()
			.ToTable("Tags");

		modelBuilder.Entity<TaskTagModel>()
			.HasOne(taskTag => (TaskModel)taskTag.Task)
			.WithMany(x => (ICollection<TaskTagModel>)x.TaskTags)
			.HasForeignKey(x => x.TaskId);

		modelBuilder.Entity<TaskTagModel>()
			.HasOne(taskTag => (TagModel)taskTag.Tag)
			.WithMany(x => (ICollection<TaskTagModel>)x.TaskTags)
			.HasForeignKey(x => x.TagId);

		modelBuilder.Entity<TaskTagModel>()
			.ToTable("TaskTags").HasKey("TaskId", "TagId");

		var todoItemBuilder = modelBuilder.Entity<TaskModel>();
		todoItemBuilder.Property(x => x.Status)
			.HasConversion(new EnumToStringConverter<TaskStatusType>());

		_logger?.LogInformation(MessagesPacket.LogBuildingSucceed, nameof(OnModelCreating), nameof(CustomAppDbContext));
	}
}
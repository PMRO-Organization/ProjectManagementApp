﻿using App.Features.Tasks.Common.Interfaces;
using App.Features.Tasks.Common.TaskTags.Common.Interfaces;
using App.Features.TodoLists.Common.Interfaces;
using App.Features.TodoLists.Common.Models;
using App.Infrastructure.Databases.App.Interfaces;
using App.Infrastructure.Databases.Common;
using App.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Infrastructure.Databases.App;

///<inheritdoc />
public class TodoListRepository : GenericRepository<TodoListModel>, ITodoListRepository
{
	private readonly CustomAppDbContext _dbContext;
	private readonly ILogger<TodoListRepository> _logger;
	private readonly ITaskEntityFactory _taskEntityFactory;
	private readonly ITodoListFactory _todoListFactory;

	public TodoListRepository(CustomAppDbContext dbContext, ILogger<TodoListRepository> logger, ITaskEntityFactory taskEntityFactory, ITodoListFactory todoListFactory) : base(dbContext, logger)
	{
		_dbContext = dbContext;
		_logger = logger;
		_taskEntityFactory = taskEntityFactory;
		_todoListFactory = todoListFactory;
	}

	///<inheritdoc />
	public async Task<bool> CheckThatAnyWithSameNameExistAsync(string todoListName)
	{
		ExceptionsService.ThrowWhenArgumentIsInvalid(nameof(CheckThatAnyWithSameNameExistAsync), todoListName, nameof(todoListName), _logger);

		return await _dbContext.Set<TodoListModel>()
			.AnyAsync(todoList => todoList.Title == todoListName);
	}

	///<inheritdoc />
	public async Task DuplicateWithDetailsAsync(int todoListId)
	{
		ExceptionsService.ThrowExceptionWhenIdLowerThanBottomBoundry(nameof(DuplicateWithDetailsAsync), todoListId, nameof(todoListId), _logger);

		ITodoListModel? todoListWithDetails = await _dbContext
			.Set<TodoListModel>()
			.Where(todoList => todoList.Id == todoListId)
			.Include(todoList => todoList.Tasks)
			.SingleOrDefaultAsync();

		if (todoListWithDetails is null)
			ExceptionsService.ThrowEntityNotFoundInDb(nameof(DuplicateWithDetailsAsync), typeof(ITodoListModel).Name, todoListId.ToString(), _logger);

		var duplicatedTasks = todoListWithDetails!.Tasks.Select(originTask => CreateNewTaskObject(originTask)).ToList();
		var duplicatedTodoList = CreateNewTodoListObject(todoListWithDetails, duplicatedTasks);
		
		await AddAsync((TodoListModel)duplicatedTodoList);
	}


	#region LOCAL FUNCTIONS FOR DUPLICATE OPERATION

	private ITaskModel CreateNewTaskObject(ITaskModel originTask)
	{
		var newTask = _taskEntityFactory.CreateModel();
		newTask.UserId = originTask.UserId;
		newTask.Title = originTask.Title;
		newTask.Description = originTask.Description;
		newTask.DueDate = originTask.DueDate;
		newTask.ReminderDate = originTask.ReminderDate;
		newTask.Status = originTask.Status;

		newTask.TaskTags = originTask.TaskTags.Select(originTaskTag => CreateNewTaskTagObject(originTaskTag)).ToList();

		return newTask;
	}

	private ITaskTagModel CreateNewTaskTagObject(ITaskTagModel originTaskTag)
	{
		var newTaskTag = _taskEntityFactory.CreateTaskTagModel();
		newTaskTag.TagId = originTaskTag.TagId;
		newTaskTag.TaskId = originTaskTag.TaskId;

		return newTaskTag;
	}

	private ITodoListModel CreateNewTodoListObject(ITodoListModel originTodoList, ICollection<ITaskModel> newTasks)
	{
		TodoListModel newTodoList = _todoListFactory.CreateModel();
		newTodoList.Title = originTodoList.Title;
		newTodoList.Tasks = newTasks;
		newTodoList.UserId = originTodoList.UserId;

		return newTodoList;
	}

	#endregion


	///<inheritdoc />
	public async Task<ICollection<TodoListModel>> GetAllWithDetailsAsync(string userId)
	{
		ExceptionsService.ThrowExceptionWhenArgumentIsNullOrEmpty(nameof(GetAllWithDetailsAsync), userId, nameof(userId), _logger);

		ICollection<TodoListModel> allTodoListsWithDetails = await _dbContext
			.Set<TodoListModel>()
			.Where(todoList => todoList.UserId == userId)
			.Include(todoList => todoList.Tasks)
			.ToListAsync();

		return allTodoListsWithDetails;
	}

	///<inheritdoc />
	public async Task<ICollection<TodoListModel>> GetAllWithDetailsByFilterAsync(Expression<Func<TodoListModel, bool>> filter)
	{
		ExceptionsService.ThrowWhenFilterExpressionIsNull(filter, nameof(GetAllWithDetailsByFilterAsync), _logger);

		ICollection<TodoListModel> entities = await _dbContext
			.Set<TodoListModel>()
			.Where(filter)
			.Include(todoList => todoList.Tasks)
			.ToListAsync();

		return entities;
	}

	///<inheritdoc />
	public async Task<TodoListModel?> GetWithDetailsAsync(int todoListId)
	{
		ExceptionsService.ThrowWhenArgumentIsInvalid(nameof(GetWithDetailsAsync), todoListId, nameof(todoListId), _logger);

		TodoListModel? todoListFromDb = await _dbContext
			.Set<TodoListModel>()
			.Where(todoList => todoList.Id == todoListId)
			.Include(todoList => todoList.Tasks)
			.SingleOrDefaultAsync();

		return todoListFromDb;
	}
}

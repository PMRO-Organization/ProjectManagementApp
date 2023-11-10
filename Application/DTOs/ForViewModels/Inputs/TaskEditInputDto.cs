﻿using Application.DTOs.ViewModels.Inputs.Abstract;
using Domain.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.ForViewModels.Inputs;

public class TaskEditInputDto : ITaskEditInputDto
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = AttributesHelper.DataFormat, ApplyFormatInEditMode = true)]
	public DateTime DueDate { get; set; }

	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = AttributesHelper.DataFormat, ApplyFormatInEditMode = true)]
	public DateTime? ReminderDate { get; set; }
	public TaskStatusHelper.TaskStatusType Status { get; set; }
	public int TodoListId { get; set; }
	public string UserId { get; set; } = string.Empty;

	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = AttributesHelper.DataFormat, ApplyFormatInEditMode = true)]
	public DateTime? LastModificationDate { get; set; }
}

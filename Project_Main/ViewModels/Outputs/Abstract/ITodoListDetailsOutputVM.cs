﻿using Application.DTOs.Entities;

namespace Web.ViewModels.Outputs.Abstract
{
	public interface ITodoListDetailsOutputVM
	{
		int Id { get; set; }
		string Name { get; set; }
		IEnumerable<ITaskDto> TasksCompleted { get; set; }
		IEnumerable<ITaskDto> TasksExpired { get; set; }
		IEnumerable<ITaskDto> TasksForToday { get; set; }
		IEnumerable<ITaskDto> TasksNotCompleted { get; set; }
		string UserId { get; set; }
	}
}
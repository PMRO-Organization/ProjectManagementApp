﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Project_DomainEntities.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Project_Main.Models.Outputs.ViewModels
{
    public interface ITaskEditOutputVM
    {
        string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AttributesHelper.DataFormat, ApplyFormatInEditMode = true)]
        DateTime DueDate { get; set; }
        int Id { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = AttributesHelper.DataFormat, ApplyFormatInEditMode = true)]
		DateTime? ReminderDate { get; set; }
        TaskStatusHelper.TaskStatusType Status { get; set; }
        SelectList? StatusSelector { get; set; }
        string Title { get; set; }
        int TodoListId { get; set; }
        SelectList? TodoListSelector { get; set; }
        string UserId { get; set; }
    }
}
﻿using Project_DomainEntities;
using Project_Main.Models.DTOs;
using Project_Main.Models.Inputs.DTOs;
using Project_Main.Models.Inputs.ViewModels;

namespace Project_Main.Services.DTO
{
    public interface ITodoListMapper
    {
		ICollection<ITodoListDto> TransferToDto(ICollection<ITodoListModel> todoLists);
        ITodoListDto TransferToDto(ITodoListModel todoListModel, IDictionary<object, object>? mappedObjects = null);
        ITodoListDto TransferToDto(ITodoListCreateInputVM createInputVM);
        ITodoListEditInputDto TransferToDto(ITodoListEditInputVM editInputVM);
        ITodoListModel TransferToModel(ITodoListDto todoListDto, IDictionary<object, object>? mappedObjects = null);
        void UpdateModel(ITodoListModel todoListDbModel, ITodoListEditInputDto taskEditInputDto);
    }
}
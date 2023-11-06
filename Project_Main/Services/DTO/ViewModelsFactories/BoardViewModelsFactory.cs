﻿using Project_Main.Infrastructure.DTOs.Entities;
using Project_Main.Models.ViewModels.OutputModels;

namespace Project_Main.Services.DTO.ViewModelsFactories
{
    public class BoardViewModelsFactory : IBoardViewModelsFactory
    {
        public IBoardAllOutputVM CreateAllOutputVM(ICollection<ITodoListDto> todolistDto)
        {
            return new BoardAllOutputVM()
            {
                TodoLists = todolistDto
            };
        }

        public IBoardBrieflyOutputVM CreateBrieflyOutputVM(ICollection<ITodoListDto> todolistDto)
        {
            return new BoardBrieflyOutputVM()
            {
                TodoLists = todolistDto
            };
        }
    }
}

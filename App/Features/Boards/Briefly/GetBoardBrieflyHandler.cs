﻿using App.Features.Boards.Common;
using App.Features.TodoLists.Common.Interfaces;
using App.Features.TodoLists.Common.Models;
using App.Features.Users.Common.Interfaces;
using App.Infrastructure.Databases.App.Interfaces;
using App.Infrastructure.Helpers;
using MediatR;

namespace App.Features.Boards.Briefly;

public class GetBoardBrieflyHandler : IRequestHandler<GetBoardBrieflyQuery, IBoardBrieflyOutputVM>
{
	private readonly ILogger<GetBoardBrieflyHandler> _logger;
	private readonly IBoardViewModelsFactory _boardsVMFactory;
	private readonly ITodoListMapper _todoListMapper;
	private readonly ITodoListRepository _todoListRepository;
	private readonly string _signedInUserId;

	public GetBoardBrieflyHandler(IBoardViewModelsFactory boardsVMFactory, ITodoListMapper todoListMapper, ITodoListRepository todoListRepository, IUserService userService, ILogger<GetBoardBrieflyHandler> logger)
	{
		_logger = logger;
		_boardsVMFactory = boardsVMFactory;
		_todoListMapper = todoListMapper;
		_todoListRepository = todoListRepository;
		_signedInUserId = userService.GetSignedInUserId();

		ExceptionsService.WhenPropertyIsNullOrEmptyThrowCritical("Constructing " + nameof(GetBoardBrieflyHandler), _signedInUserId, nameof(_signedInUserId), _logger);
	}

	public async Task<IBoardBrieflyOutputVM> Handle(GetBoardBrieflyQuery request, CancellationToken cancellationToken)
	{
		ICollection<TodoListModel> todoListModels = await _todoListRepository.GetAllWithDetailsByFilterAsync(todoList => todoList.UserId == _signedInUserId);
		var todoListDtos = _todoListMapper.TransferToDto(todoListModels);
		IBoardBrieflyOutputVM boardBrieflyOutputVM = _boardsVMFactory.CreateBrieflyOutputVM(todoListDtos);

		return boardBrieflyOutputVM;
	}
}
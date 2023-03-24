﻿using Microsoft.EntityFrameworkCore;
using TODO_Domain_Entities;
using TODO_List_ASPNET_MVC.Infrastructure.Helpers;

namespace TODO_List_ASPNET_MVC.Models.DataBases.Common.Helpers
{
	public static class DbContextValidators
	{
		public static void CheckDbSetIfNullThrowException<T>(DbSet<T> dbSet, ILogger _logger, string operationName) where T : class
		{
			if (dbSet == null)
			{
				_logger.LogCritical(Messages.ParamNullOrEmptyLogger, operationName, typeof(T).Name);
				throw new InvalidOperationException(Messages.DbSetNullEx);
			}
		}

		public static async Task CheckItemExistsIfNotThrowException<T>(DbSet<T> dbSet, string dbSetName, int id, string operationName, ILogger _logger) where T : BasicModelAbstract
		{
			if (await dbSet.AnyAsync(x => x.Id == id) is false)
			{
				_logger.LogInformation(Messages.EntityNotFoundInDbSetLogger, operationName, dbSetName, id);
				throw new InvalidOperationException(Messages.ItemNotFoundInDb);
			}
		}
	}
}

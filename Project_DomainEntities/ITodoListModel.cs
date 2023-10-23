﻿namespace Project_DomainEntities
{
	public interface ITodoListModel
	{
		int Id { get; set; }
		IEnumerable<ITaskModel> Tasks { get; set; }
		string Title { get; set; }
		string UserId { get; set; }

		bool Equals(object? obj);
		int GetHashCode();
		bool IsTheSame(ITodoListModel obj);
	}
}
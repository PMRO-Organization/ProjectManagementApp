using Project_DomainEntities;

namespace Project_Main.Models.DataBases.Repositories
{
    public interface ITodoListRepository : IRepository<TodoListModel>
    {
        Task<TodoListModel> GetWithDetailsAsync(int id);

        Task<IEnumerable<TodoListModel>> GetAllWithDetailsAsync();

        Task DuplicateWithDetailsAsync(int id);

        Task<bool> DoesAnyExistWithSameNameAsync(string name);
    }
}

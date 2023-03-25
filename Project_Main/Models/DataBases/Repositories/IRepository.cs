using System.Linq.Expressions;

namespace Project_Main.Models.DataBases.Repositories
{
    public interface IRepository<T> where T : class
    {
        IRepository<T> Repository { get; set; }
        Task Add(T entity);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>>? filter = null);
		Task Update(T entity);
		Task Remove(T entity);
        Task<IEnumerable<T>> GetAllAsync();
    }
}

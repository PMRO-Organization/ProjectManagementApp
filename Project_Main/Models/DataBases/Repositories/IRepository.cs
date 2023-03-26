using System.Linq.Expressions;

namespace Project_Main.Models.DataBases.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        Task<T?> GetAsync(object id);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>>? filter = null);
		void Update(T entity);
		void Remove(T entity);
        Task<IEnumerable<T>> GetAllAsync();
    }
}

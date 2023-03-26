using Project_IdentityDomainEntities;

namespace Project_Main.Models.DataBases.Repositories
{
	public interface IUserRepository : IRepository<UserModel>
	{
		Task<UserModel> GetWithDetailsAsync(int id);
		Task<UserModel> GetByNameAndPasswordAsync(string name, string password);
		Task<bool> IsNameTakenAsync(string name);
	}
}

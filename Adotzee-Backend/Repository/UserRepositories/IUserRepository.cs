using Adotzee_Backend.Models;

namespace Adotzee_Backend.Repository.UserRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUser(int id);
        Task<User?> GetByEmail(string email);

        Task Add(User user);
        Task Update(User user);

        Task<bool> Delete(int id);
        Task<bool?> ToggleBlock(int id);
    }
}

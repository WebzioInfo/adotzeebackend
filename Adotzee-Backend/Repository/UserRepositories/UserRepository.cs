using Adotzee_Backend.Data;
using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Repository.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /* ------------ GET ALL ------------ */

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        /* ------------ GET BY ID ------------ */

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        /* ------------ GET BY EMAIL ------------ */

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        /* ------------ ADD USER ------------ */

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await Save();
        }

        /* ------------ UPDATE USER ------------ */

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await Save();
        }

        /* ------------ DELETE USER ------------ */

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            // Replacing hard delete with soft block since Role/User references might exist
            user.IsBlocked = true; // Using IsBlocked as the soft-delete flag

            // _context.Users.Remove(user); // Removed hard delete
            await Save();
            return true;
        }

        /* ------------ BLOCK / UNBLOCK ------------ */

        public async Task<bool?> ToggleBlock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.IsBlocked = !user.IsBlocked;
            await Save();

            return user.IsBlocked;
        }

        /* ------------ SAVE ------------ */

        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

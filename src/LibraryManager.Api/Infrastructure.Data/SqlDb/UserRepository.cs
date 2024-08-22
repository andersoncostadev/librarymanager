using Core.Entities.v1;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.SqlDb
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> AddAsync(UserEntity entity)
        {
            _context.Users!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Users!.FindAsync(id);

            if (entity == null)
                return false;

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _context.Users!.ToListAsync();
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            return await _context.Users!.FindAsync(id);
        }

        public async Task<UserEntity> UpdateAsync(UserEntity entity)
        {
            _context.Users!.Update(entity);

            await _context.SaveChangesAsync();

            return entity!;
        }
    }
}

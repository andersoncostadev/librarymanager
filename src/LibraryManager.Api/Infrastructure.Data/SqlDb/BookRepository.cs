using Core.Entities.v1;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.SqlDb
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookEntity> AddAsync(BookEntity entity)
        {
            _context.Books!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Books!.FindAsync(id);

            if (entity == null)
                return false;
    
            _context.Books.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<BookEntity>> GetAllAsync()
        {
            return await _context.Books!.ToListAsync();
        }

        public async Task<BookEntity> GetByIdAsync(Guid id)
        {
            return await _context.Books!.FindAsync(id);
        }

        public async Task<BookEntity> UpdateAsync(BookEntity entity)
        {
            _context.Books!.Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}

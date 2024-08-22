using Core.Entities.v1;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.SqlDb
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoanEntity> AddAsync(LoanEntity entity)
        {
            _context.Loans!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Loans!.FindAsync(id);

            if (entity == null)
                return false;

            _context.Loans.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<LoanEntity>> GetAllAsync()
        {
           return await _context.Loans!.ToListAsync();
        }

        public async Task<LoanEntity> GetByIdAsync(Guid id)
        {
            return await _context.Loans!.FindAsync(id);
        }

        public async Task<LoanEntity> UpdateAsync(LoanEntity entity)
        {
            _context.Loans!.Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBookingRepository : IBaseRepository<BookingEntity>
{
    Task<RepositoryResult<IEnumerable<BookingEntity>>> GetByEmailAsync(string email);
}

public class BookingRepository(DataContext context) : BaseRepository<BookingEntity>(context), IBookingRepository
{
    public override async Task<RepositoryResult<IEnumerable<BookingEntity>>> GetAllAsync()
    {
        try
        {
            var entities = await _table
                .Include(x => x.BookingOwner)
                .ThenInclude(x => x!.Address)
                .ToListAsync();
            return new RepositoryResult<IEnumerable<BookingEntity>> { Success = true, Result = entities };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<IEnumerable<BookingEntity>>
            {
                Success = false,
                Error = ex.Message,
            };

        }
    }

    public override async Task<RepositoryResult<BookingEntity?>> GetAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        try
        {
            var entity = await _table
                .Include(x => x.BookingOwner)
                .ThenInclude(x => x!.Address)
                .FirstOrDefaultAsync(expression) ?? throw new Exception("Not Found.");
            return new RepositoryResult<BookingEntity?> { Success = true, Result = entity };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<BookingEntity?>
            {
                Success = false,
                Error = ex.Message,
            };
        }
    }

    public async Task<RepositoryResult<IEnumerable<BookingEntity>>> GetByEmailAsync(string email)
    {
        try
        {
            var bookings = await _table
                .Include(x => x.BookingOwner)
                .ThenInclude(x => x!.Address)
                .Where(b => b.BookingOwner.Email == email)
                .ToListAsync();

            return new RepositoryResult<IEnumerable<BookingEntity>>
            {
                Success = true,
                Result = bookings
            };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<IEnumerable<BookingEntity>>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }


}


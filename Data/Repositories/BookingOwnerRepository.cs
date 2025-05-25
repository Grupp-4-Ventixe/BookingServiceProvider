using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IBookingOwnerRepository : IBaseRepository<BookingOwnerEntity>
{
  
}

public class BookingOwnerRepository(DataContext context) : BaseRepository<BookingOwnerEntity>(context), IBookingOwnerRepository
{
    public override async Task<RepositoryResult> DeleteAsync(BookingOwnerEntity entity)
    {
        var fullEntity = await _table
            .Include(o => o.Address)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        if (fullEntity == null)
            return new RepositoryResult { Success = false, Error = "Owner not found" };

        _table.Remove(fullEntity);
        await _context.SaveChangesAsync();

        return new RepositoryResult { Success = true };
    }
}

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IBookingAddressRepository : IBaseRepository<BookingAddressEntity>
{

}

public class BookingAddressRepository(DataContext context) : BaseRepository<BookingAddressEntity>(context), IBookingAddressRepository
{
    public override async Task<RepositoryResult> DeleteAsync(BookingAddressEntity entity)
    {
        var fullEntity = await _table
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        if (fullEntity == null)
            return new RepositoryResult { Success = false, Error = "Address not found" };

        _table.Remove(fullEntity);
        await _context.SaveChangesAsync();

        return new RepositoryResult { Success = true };
    }
}

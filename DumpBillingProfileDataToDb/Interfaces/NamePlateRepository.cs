using DumpBillingProfileDataToDb.DBContext;
using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using Microsoft.EntityFrameworkCore;

public class NamePlateRepository : INamePlateRepository
{
    private readonly IDbContextFactory<PostGresqlDBContext> _context;

    public NamePlateRepository(IDbContextFactory<PostGresqlDBContext> context)
    {
        _context = context;
    }

    public async Task<List<NamePlate>> GetNamePlates()
    {
        using var dbContext = _context.CreateDbContext();

        return await dbContext.NamePlateWithRegister
            .FromSqlRaw("SELECT meternumber, metercategory FROM meter_nameplate")
            .AsNoTracking()
            .ToListAsync();
    }
}
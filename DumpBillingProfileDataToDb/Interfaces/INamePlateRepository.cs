using DumpBillingProfileDataToDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface INamePlateRepository
{
    Task<List<NamePlate>> GetNamePlates();
}

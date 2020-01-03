using Microsoft.EntityFrameworkCore;
using Im.Access.EntityFramework.Entities;

namespace Im.Access.EntityFramework.Interfaces
{
    public interface IAdminLogDbContext
    {
        DbSet<Log> Logs { get; set; }
    }
}

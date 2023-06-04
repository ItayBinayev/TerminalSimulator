using Microsoft.EntityFrameworkCore;
using TerminalProjectApi.Models;

namespace TerminalProjectApi.Database
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Flight> Flights { get; set; }

    }
}

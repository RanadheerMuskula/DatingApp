using Microsoft.EntityFrameworkCore;
using datingApp.API.Models;
namespace datingApp.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {}
            public DbSet<Value> Values {get;set;}
        
    }
}
using Microsoft.EntityFrameworkCore;
using MandezcaTest.Models;

namespace MandezcaTest.Database
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Client = Set<Client>();
            Perfil = Set<Perfil>();
            Address = Set<Address>();
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}

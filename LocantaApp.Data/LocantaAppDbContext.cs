using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocantaApp.Core;

namespace LocantaApp.Data
{
    public class LocantaAppDbContext: DbContext
    {

        // passing options from Startup.cs to DbContext class
        public LocantaAppDbContext(DbContextOptions<LocantaAppDbContext> options) :base (options)
        {
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TestDatabase.db");
            base.OnConfiguring(optionsBuilder);
        }
        */
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}

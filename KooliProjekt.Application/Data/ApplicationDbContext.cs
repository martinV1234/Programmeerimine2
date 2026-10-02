using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<InVoice> Invoices { get; set; }

        public DbSet<InvoiceLine> InvoiceLines { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<CarManufacturer> CarManufacturers { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<Car> Cars { get; set; }
    }
}

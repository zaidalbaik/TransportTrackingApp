using Microsoft.EntityFrameworkCore;

namespace TTSAPI.Models
{
    public class ApplicationDbContext : DbContext
    {

        //DbContextOptions<ApplicationDbContext> options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var connectionString = configuration.GetConnectionString("MyConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Driver>().HasIndex(d => d.Email).IsUnique();



            //modelBuilder.Entity<Bus>().ToTable("Bus");
            //modelBuilder.Entity<Driver>().ToTable("Driver");

            //modelBuilder.Entity<Driver>().HasKey(dr => new { dr.DriverID, dr.Email });

            //modelBuilder.Entity<User>().HasKey(us => new { us.UserID, us.Email });

            // modelBuilder.Entity<Driver>()
            //.HasOne(b => b.Bus)
            //.WithOne(i => i.Driver)
            //.HasForeignKey<Bus>(b => b.DriverID);
        }


        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Line> Lines { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Notification> Notifications { get; set; }

    }
}

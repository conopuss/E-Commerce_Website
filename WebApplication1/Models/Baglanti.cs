using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Baglanti:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:ProjemConnection"]);


        }

		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            // Additional configurations if any

            base.OnModelCreating(modelBuilder);
        }

		public DbSet<Sp_Search> Sp_Searches { get; set; } //stored procedure (arama)
		public DbSet<Vw_MyOrder>Vw_MyOrders { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Setting>? Settings { get; set; }
        public DbSet<Status>? Statuses { get; set; }
        public DbSet<Supplier>? Suppliers { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Message>? Messages { get; set; }
    



    }
}

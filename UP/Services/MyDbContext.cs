using Microsoft.EntityFrameworkCore;
using EKZ.Models;
using Microsoft.Extensions.Logging;

namespace EKZ.Services
{
    /// <summary>
    /// Контекст базы данных для управления сущностями.
    /// </summary>
    public class MyDbContext : DbContext
    {
        // Сущности
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Настройка подключения к базе данных.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=testing;Username=postgres;Password=123")
                .EnableSensitiveDataLogging();    // Логирование данных (только для разработки)
        }

        /// <summary>
        /// Конфигурация моделей и их отношений.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация сущности User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            });

            // Связи между сущностями
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Requests)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientID);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientID);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Request)
                .WithMany()
                .HasForeignKey(r => r.RequestID);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Service)
                .WithMany()
                .HasForeignKey(r => r.ServiceID);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Employee)
                .WithMany()
                .HasForeignKey(r => r.EmployeeID);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Car)
                .WithMany()
                .HasForeignKey(s => s.CarID);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientID);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeID);
        }

        /// <summary>
        /// Метод для заполнения базы данных тестовыми данными.
        /// </summary>
        public void SeedData()
        {
            if (Cars.Any() || Clients.Any() || Requests.Any() || Services.Any() || Employees.Any() || Repairs.Any() || Inventories.Any() || Sales.Any())
            {
                return; // Если данные уже существуют, метод завершает выполнение.
            }

            // Заполнение данными
            var cars = Enumerable.Range(1, 20).Select(i => new Car
            {
                Brand = "Brand" + i,
                Model = "Model" + i,
                Year = 2000 + i,
                Price = 200000 + (i * 10000),
                Availability = i % 2 == 0,
                VIN = "VIN" + i.ToString("D17")
            }).ToList();

            var clients = Enumerable.Range(1, 20).Select(i => new Client
            {
                FullName = $"Client {i}",
                Phone = $"+7900123456{i:D2}",
                Email = $"client{i}@example.com",
                Address = $"{i} Example St, City{i}"
            }).ToList();

            var requests = Enumerable.Range(1, 20).Select(i => new Request
            {
                CreationDate = DateTime.UtcNow.AddDays(-i),
                ClientID = i,
                Type = i % 2 == 0 ? "purchase" : "service",
                Status = i % 3 == 0 ? "completed" : (i % 2 == 0 ? "in progress" : "new")
            }).ToList();

            var services = Enumerable.Range(1, 20).Select(i => new Service
            {
                Name = $"Service {i}",
                Description = $"Description for service {i}",
                Cost = 50 + i * 10
            }).ToList();

            var employees = Enumerable.Range(1, 20).Select(i => new Employee
            {
                FullName = $"Employee {i}",
                Position = i % 2 == 0 ? "Technician" : "Manager",
                Phone = $"+7900123456{i:D2}",
                Email = $"employee{i}@example.com"
            }).ToList();

            var repairs = Enumerable.Range(1, 20).Select(i => new Repair
            {
                RequestID = i,
                ServiceID = i,
                EmployeeID = (i % 10) + 1,
                Date = DateTime.UtcNow.AddDays(-i),
                Cost = 100 + (i * 5)
            }).ToList();

            var inventories = Enumerable.Range(1, 20).Select(i => new Inventory
            {
                Name = $"Inventory {i}",
                Quantity = 10 + i,
                PricePerUnit = 5.00M + i
            }).ToList();

            var sales = Enumerable.Range(1, 20).Select(i => new Sale
            {
                CarID = (i % 10) + 1,
                ClientID = i,
                SaleDate = DateTime.UtcNow.AddDays(-i),
                TotalAmount = 200000 + (i * 10000),
                EmployeeID = (i % 10) + 1
            }).ToList();

            // Добавление данных в базу
            Cars.AddRange(cars);
            Clients.AddRange(clients);
            Requests.AddRange(requests);
            Services.AddRange(services);
            Employees.AddRange(employees);
            Repairs.AddRange(repairs);
            Inventories.AddRange(inventories);
            Sales.AddRange(sales);

            // Сохранение изменений
            SaveChanges();
        }
    }
}

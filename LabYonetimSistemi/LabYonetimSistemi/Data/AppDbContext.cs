using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design; // Bu yeni eklendi
using LabApi.Models;

namespace LabApi.Data;

// 1. ANA CONTEXT SINIFI
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Lab> Labs { get; set; }
    public DbSet<Computer> Computers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Issue> Issues { get; set; }
}

// 2. FABRİKA SINIFI (Hata almanı engelleyecek olan kısım burası)
// Bu sınıf, migration komutlarını çalıştırdığında devreye girer ve 
// appsettings.json dosyasını bulamasa bile veritabanı yolunu buradan okur.
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // Buradaki dosya isminin appsettings.json içindekiyle aynı olması yeterlidir.
        optionsBuilder.UseSqlite("Data Source=LabDatabase.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}


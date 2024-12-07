using System.Windows;
using EKZ.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EKZ;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CreateDbOrExistsCheck();
    }

    public static void CreateDbOrExistsCheck()
    {
        using (var dbContext = new MyDbContext())
        {
            if (!dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                dbContext.Database.EnsureCreated();
                dbContext.SeedData();
            }
            
            ApplyMigrations(dbContext);
        }
    }
    private static void ApplyMigrations(MyDbContext dbContext)
    {
        try
        {
            dbContext.Database.GetMigrations();
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Ошибка применения миграций: {ex.Message}");
        }
    }
}
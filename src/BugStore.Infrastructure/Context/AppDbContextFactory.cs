namespace BugStore.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var conn =
                Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionStrings__Default")
                ?? "Data Source=bugstore.db";

            optionsBuilder.UseSqlite(conn, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

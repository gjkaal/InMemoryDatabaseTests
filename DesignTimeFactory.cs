using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace FakeDbTestModel;

/// <summary>
/// The system context design time factory provides the ef code migration
/// service with a SystemContext connected to a local database.
/// </summary>
internal class DesignTimeFactory : IDesignTimeDbContextFactory<MyAwesomeDb>
{
    private const string ConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={nameof(MyAwesomeDb)};Integrated Security=true;";

    /// <summary>
    /// CreateMyAwesomeDb the system context
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of SystemContext</returns>
    public MyAwesomeDb CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyAwesomeDb>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new MyAwesomeDb(optionsBuilder.Options, null);
    }
}

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FakeDbTestModel;

/// <summary>
/// This class is used to crate a fake db using an in memory database with Sqlite.
/// </summary>
public class FakeDbContextFactory : IDataContextFactory, IDisposable
{
    private readonly ILogger<FakeDbContextFactory> logger;
    private readonly IDatabaseRulePolicy databaseRulePolicy;
    private SqliteConnection? connection;

    public FakeDbContextFactory(ILogger<FakeDbContextFactory> logger, IDatabaseRulePolicy databaseRulePolicy)
    {
        this.logger = logger;
        this.databaseRulePolicy = databaseRulePolicy;
    }

    public void Dispose()
    {
        logger.LogInformation("Disposing FakeDefinitionContextFactory");
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        logger.LogInformation("Inner disposing FakeDefinitionContextFactory");
        if (!disposedValue)
        {
            connection?.Dispose();
            disposedValue = true;
        }
    }

    /// <summary>
    /// <para>Define a fake database context for tests that need an operation definition context </para>
    /// <para>Fake objects actually have working implementations, but usually take some shortcut which makes them not suitable for production (an in memory database is a good example).</para>
    /// </summary>
    /// <returns></returns>
    public IMyAwesomeDb CreateMyAwesomeDb()
    {
        connection = OpenConnection();
        var controlOptions = new DbContextOptionsBuilder<MyAwesomeDb>()
            .UseSqlite(connection)
            .Options;

        var context = new MyAwesomeDb(controlOptions, databaseRulePolicy);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    private SqliteConnection OpenConnection()
    {
        SqliteConnection connection;
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }
}

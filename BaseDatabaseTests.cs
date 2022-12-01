using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace FakeDbTestModel;

public abstract class BaseDatabaseTests : IDisposable
{
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                dbContext?.Dispose();
            }
            disposedValue = true;
        }
    }

    protected readonly Mock<IDatabaseRulePolicy> rules = new Mock<IDatabaseRulePolicy>();
    protected readonly Mock<IUserContext> adminUser = new Mock<IUserContext>();
    protected readonly IServiceCollection Sc = new ServiceCollection();
    protected IMyAwesomeDb? dbContext;
    protected bool disposedValue;
    protected IServiceProvider? Sp;

    protected void SeedData(IMyAwesomeDb db)
    {
        // Seed the database befgore executing tests
    }

    public void ConfigureTests()
    {
        Sc.AddScoped<IDataContextFactory, FakeDbContextFactory>();
        Sc.AddScoped((sp) => rules.Object);
        Sc.AddLogging(opt => opt.AddConsole());
        Sc.AddMemoryCache();
        Sp = Sc.BuildServiceProvider();

        var factory = Sp.GetService<IDataContextFactory>();
        Assert.IsNotNull(factory);

        using (var db = factory.CreateMyAwesomeDb())
        {
            SeedData(db);
            db.CompleteAsync(adminUser.Object).GetAwaiter().GetResult();
        }

        dbContext = Sp.GetService<IDataContextFactory>()?.CreateMyAwesomeDb();
    }
}

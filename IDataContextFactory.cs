namespace FakeDbTestModel;

public interface IDataContextFactory : IDisposable
{
    IMyAwesomeDb CreateMyAwesomeDb();
}

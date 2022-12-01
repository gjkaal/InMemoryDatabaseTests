using Microsoft.EntityFrameworkCore;

namespace FakeDbTestModel;

public interface IMyAwesomeDb : IUnitOfWork, IDisposable
{
    DbSet<Entities.Foo> Foo { get; }
}

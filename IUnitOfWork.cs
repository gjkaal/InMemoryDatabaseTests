namespace FakeDbTestModel;

public interface IUnitOfWork
{
    Task<int> CompleteAsync(IUserContext userContext, CancellationToken cancellationToken = default);
}

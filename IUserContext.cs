using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FakeDbTestModel;

public interface IUserContext
{
    Guid Uuid { get; }
    string UserName { get; }
    string DisplayName { get; }
    bool IsAuthenticated { get; }

    bool IsInRole(string roleName);

    bool IsOperationAllowed(ChangeTracker tracker);
}

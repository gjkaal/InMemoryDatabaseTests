using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FakeDbTestModel;

/// <summary>
/// Rule validator for database operations
/// </summary>
public interface IDatabaseRulePolicy
{
    bool IsOperationAllowed(IUserContext userContext, ChangeTracker changeTracker);
}

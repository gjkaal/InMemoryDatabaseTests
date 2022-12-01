using FakeDbTestModel.Core;
using FakeDbTestModel.Entities;
using FakeDbTestModel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FakeDbTestModel;

/// <summary>
/// Define the database conmtext, using an interface to define which parts of
/// the database context should be used by other classes.
/// </summary>
public class MyAwesomeDb : DbContext, IMyAwesomeDb
{
    private readonly IDatabaseRulePolicy? databaseRuleValidator;

    public MyAwesomeDb(DbContextOptions<MyAwesomeDb> options, IDatabaseRulePolicy? databaseRuleValidator)
        : base(options)
    {
        this.databaseRuleValidator = databaseRuleValidator;
    }

    public DbSet<Foo> Foo { get; set; }

    /// <summary>
    /// Finalize the current changes (unit of work) in the current database.
    /// This method can be the focal point for any of the folowwing:
    /// <list type="bullet">
    /// <item>Validation</item>
    /// <item>Logging</item>
    /// <item>Modification of basic data columns (such as 'UpdatedBy')</item>
    /// </list>
    /// </summary>
    /// <param name="userContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="AccessViolationException"></exception>
    public async Task<int> CompleteAsync(IUserContext userContext, CancellationToken cancellationToken = default)
    {
        var hasChanges = ChangeTracker.HasChanges();
        if (!hasChanges)
        {
            // Should we warn when there are no changhes?
            return 0;
        }
        if (databaseRuleValidator.IsOperationAllowed(userContext, ChangeTracker))
        {
            foreach (var e in ChangeTracker.Entries())
            {
                if (e.Entity is DbRecord dbRecord)
                {
                    dbRecord.ModifiedBy = userContext.UserName;
                    dbRecord.Modified = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            ChangeTracker.Clear();
            throw new UpdateNotAllowedException();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Entities.Foo.CreateModel(modelBuilder);
    }
}

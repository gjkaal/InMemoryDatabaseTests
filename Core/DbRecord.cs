using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FakeDbTestModel.Core;

public abstract class DbRecord
{
    /// <summary>
    /// The primary key
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Using a GUID is perfect for creating (external) identifiers, but not for database
    /// primary keys. They can be generated externally and used as references without the need
    /// for waiting until a record is created/stored in the database.
    /// </summary>
    public Guid Uuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Add a timestamp for optimistic concurrency.
    /// </summary>
    [Timestamp]
    public byte[]? TimeStamp { get; set; }

    /// <summary>
    /// Property to set the last user that modified this record.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>
    /// Keep track of the last change date and time.
    /// </summary>
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates the Record model.
    /// </summary>
    /// <param name="mb">The model builder.</param>
    protected static void CreateModel<TR>([NotNull] ModelBuilder mb) where TR : DbRecord
    {
        var modelName = typeof(TR).Name;

        mb.Entity<TR>().HasKey(m => m.Id);
        mb.Entity<TR>().Property(m => m.Id).IsRequired();
        mb.Entity<TR>().Property(m => m.Uuid).IsRequired();

        mb.Entity<TR>().HasIndex(m => m.Uuid, $"UQ_{modelName}_UuidIndex").IsUnique();

        /* Other fields, such as sceated/deleted and user identifier fields could be added to a base record. */
        mb.Entity<TR>().HasIndex(m => m.ModifiedBy, $"IX_{modelName}_ModifiedBy");
        mb.Entity<TR>().HasIndex(m => m.Modified, $"IX_{modelName}_Modified");

        /*
        mb.Entity<TR>().HasIndex(m => m.Created, $"IX_{modelName}_Created");
        mb.Entity<TR>().HasIndex(m => m.IsDeleted, $"IX_{modelName}_IsDeleted");
        mb.Entity<TR>().HasIndex(m => m.CreatedBy, $"IX_{modelName}_CreatedBy");

        mb.Entity<TR>().Property(m => m.IsDeleted).HasDefaultValue(false);
        mb.Entity<TR>().Property(m => m.IsLocked).HasDefaultValue(false);
        mb.Entity<TR>().Property(m => m.Created).HasDefaultValueSql("getdate()");
        mb.Entity<TR>().Property(m => m.LastModified).HasDefaultValueSql("getdate()");
        */
    }
}

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moq;

namespace FakeDbTestModel;

[TestClass]
public class MyAwesomeDatabaseTests : BaseDatabaseTests
{
    [TestMethod]
    public void TestInsertRecord()
    {
        Assert.IsNotNull(dbContext);

        const int ExpectedUpdateCount = 1;
        dbContext.Foo.Add(new Entities.Foo { Name = "TestInsertRecord" });
        var updateCount = dbContext.CompleteAsync(adminUser.Object).GetAwaiter().GetResult();
        Assert.AreEqual(ExpectedUpdateCount, updateCount);
    }

    [TestMethod]
    public void BaseRecordUuidIsDefinedExternally()
    {
        // arrange
        Assert.IsNotNull(dbContext);
        var recordId = Guid.NewGuid();
        dbContext.Foo.Add(new Entities.Foo { Uuid = recordId, Name = "TestInsertRecord" });

        // act
        dbContext.CompleteAsync(adminUser.Object).GetAwaiter().GetResult();

        // assert
        var userNAme = adminUser.Object.UserName;
        var foo2 = dbContext.Foo.FirstOrDefault(m => m.Uuid == recordId);
        Assert.IsNotNull(foo2);
    }

    [TestMethod]
    public void BaseRecordGetUserinfoWhenInserted()
    {
        // arrange
        Assert.IsNotNull(dbContext);
        var recordId = Guid.NewGuid();
        dbContext.Foo.Add(new Entities.Foo { Uuid = recordId, Name = "TestInsertRecord" });

        // act
        dbContext.CompleteAsync(adminUser.Object).GetAwaiter().GetResult();

        // assert
        var userNAme = adminUser.Object.UserName;
        var foo2 = dbContext.Foo.Single(m => m.Uuid == recordId);
        Assert.AreEqual(userNAme, foo2.ModifiedBy);
    }

    [TestInitialize]
    public void TestInitialize()
    {
        adminUser.SetupGet(m => m.UserName).Returns("administrator");
        rules.Setup(m => m.IsOperationAllowed(It.Is<IUserContext>(m => m.UserName == "administrator"), It.IsAny<ChangeTracker>())).Returns(true);

        ConfigureTests();
    }
}

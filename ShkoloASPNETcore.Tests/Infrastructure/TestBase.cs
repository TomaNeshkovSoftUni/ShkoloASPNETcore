using ShkoloASPNETcore.Infrastructure.Data;

namespace ShkoloASPNETcore.Tests.Infrastructure;

public abstract class TestBase
{
    protected ShkoloDbContext Context = null!;

    [SetUp]
    public void BaseSetUp()
    {
        Context = DbFactory.Create(Guid.NewGuid().ToString());
    }

    [TearDown]
    public void BaseTearDown()
    {
        Context.Dispose();
    }
}
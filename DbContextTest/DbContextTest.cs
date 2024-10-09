using DbContextSetting;
using Microsoft.EntityFrameworkCore;

namespace DbContextTest;

[TestClass]
public class DbContextTest
{
    [ClassInitialize]
    public static async Task TestInitialize(TestContext _)
    {
        using (var context = new TestDbContext())
        {
            await context.Database.MigrateAsync();
        
            await context.SampleEntities.AddRangeAsync(new List<SampleEntity>()
            {
                new SampleEntity() {
                    Name = "Test entity with RLS flag",
                    RLS = true,
                },
                new SampleEntity() {
                    Name = "Test entity with no RLS flag",
                    RLS = false,
                },
            });
            await context.SaveChangesAsync();
        }
    }

    [TestMethod]
    public async Task ThreadSafeDbContext_Works_With_IgnoreQueryFilters()
    {
        using (var context = new TestDbContext())
        {
            var query = context.SampleEntities
                .IgnoreQueryFilters();
            var count = await query.CountAsync();
            Assert.AreEqual(2, count);
        }
    }

    [TestMethod]
    public async Task ThreadSafeDbContext_Works_Without_IgnoreQueryFilters()
    {
        using (var context = new TestDbContext())
        {
            var query = context.SampleEntities;
            var count = await query.CountAsync();
            Assert.AreEqual(1, count);
        }
    }
}
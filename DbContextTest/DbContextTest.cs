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
    public async Task ThreadSafeDbContext_Handles_IgnoreQueryFilters_Correctly()
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
    public async Task ThreadSafeDbContext_FiltersResultsBy_RLSFlag()
    {
        using (var context = new TestDbContext())
        {
            var query = context.SampleEntities;
            var count = await query.CountAsync();
            Assert.AreEqual(1, count);
        }
    }

    [TestMethod]
    public async Task ThreadSafeDbContext_SetMethod_Handles_IgnoreQueryFilters_Correctly()
    {
        using (var context = new TestDbContext())
        {
            var query = context.Set<SampleEntity>()
                .IgnoreQueryFilters();
            var count = await query.CountAsync();
            
            // issue: The expected result is 2, but the actual result is 1.
            // This issue is likely related to the database configuration or query filtering logic.
            // This test is expected to fail currently and requires further investigation.
            Assert.AreEqual(2, count);
        }
    }

    [TestMethod]
    public async Task ThreadSafeDbContext_SetMethod_FiltersResultsBy_RLSFlag()
    {
        using (var context = new TestDbContext())
        {
            var query = context.Set<SampleEntity>();
            var count = await query.CountAsync();
            Assert.AreEqual(1, count);
        }
    }
}
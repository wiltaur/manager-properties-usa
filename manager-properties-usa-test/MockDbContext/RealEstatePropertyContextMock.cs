using manager_properties_usa.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace manager_properties_usa_test.MockDbContext
{
    public class RealEstatePropertyContextMock
    {
        public static Mock<RealEstatePropertyContext> GetDbContext()
        {
            var dbName = Guid.NewGuid().ToString();
            var dbOptions = new DbContextOptionsBuilder<RealEstatePropertyContext>()
                        .UseInMemoryDatabase(dbName)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .EnableSensitiveDataLogging(true)
                        .Options;
            return new Mock<RealEstatePropertyContext>(dbOptions);
        }

        public static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            return GetMockDbSet(sourceList).Object;
        }

        public static Mock<DbSet<T>> GetMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet;
        }
    }
}

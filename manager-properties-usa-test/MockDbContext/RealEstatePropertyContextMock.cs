using manager_properties_usa.Models.Context;
using manager_properties_usa.Models.Model;
using manager_properties_usa_test.Helpper;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;

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

        public static Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> introLst) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(introLst.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(introLst.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(introLst.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(introLst.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => introLst.GetEnumerator());
            return mockSet;
        }
    }
}

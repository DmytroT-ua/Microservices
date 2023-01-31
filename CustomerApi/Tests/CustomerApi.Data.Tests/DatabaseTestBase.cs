using CustomerApi.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Data.Tests
{
    public class DatabaseTestBase : IDisposable
    {
        protected readonly DatabaseContext Context;

        public DatabaseTestBase()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            Context = new DatabaseContext(options);

            Context.Database.EnsureCreated();

            DatabaseInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}

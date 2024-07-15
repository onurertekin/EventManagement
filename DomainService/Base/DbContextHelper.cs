using DatabaseModel;

namespace DomainService.Base
{
    public class DbContextHelper
    {
        protected readonly MainDbContext dbContext;

        public DbContextHelper(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TEntity SaveEntity<TEntity>(TEntity entity)
        {
            if (dbContext.Database.CurrentTransaction == null) // Eğer aktif bir veritabanı işlemi (transaction) yoksa
                dbContext.Database.BeginTransaction();

            dbContext.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }
        public TEntity UpdateEntity<TEntity>(TEntity entity)
        {
            if (dbContext.Database.CurrentTransaction == null)
                dbContext.Database.BeginTransaction();

            dbContext.Update(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public TEntity DeleteEntity<TEntity>(TEntity entity)
        {
            if (dbContext.Database.CurrentTransaction == null)
                dbContext.Database.BeginTransaction();

            dbContext.Remove(entity);
            dbContext.SaveChanges();
            return entity;
        }

    }
}

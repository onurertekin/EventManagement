using DatabaseModel;

namespace Host.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, MainDbContext mainDbContext)
        {
            try
            {
                await _next(context);

                if (mainDbContext.Database.CurrentTransaction != null)
                    await mainDbContext.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await mainDbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

    }
}

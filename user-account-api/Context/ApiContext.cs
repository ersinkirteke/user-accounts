using Microsoft.EntityFrameworkCore;


namespace user_account_api.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using service_harness.Models;

namespace token_api.Context
{
    public class EmailTokenContext : DbContext
    {
        public EmailTokenContext(DbContextOptions<EmailTokenContext> options)
            : base(options)
        {
            
        }

        public DbSet<EmailToken> EmailTokens { get; set; }
    }
}

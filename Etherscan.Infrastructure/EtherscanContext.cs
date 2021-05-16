using Etherscan.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Etherscan.Infrastructure
{
    public class EtherscanContext : DbContext
    {
        public EtherscanContext(DbContextOptions<EtherscanContext> options) : base(options)
        {
        }
        
        public DbSet<Token> Tokens { get; set; }
    }
}
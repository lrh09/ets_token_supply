using System.Collections.Generic;
using System.Linq;
using Etherscan.Domain.Models;

namespace Etherscan.Infrastructure.Repositories
{
    public class TokenRepository : GenericRepository<Token>
    {
        public TokenRepository(EtherscanContext context) : base(context)
        {
        }
    }
}
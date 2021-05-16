using System.Collections.Generic;
using System.Linq;
using Etherscan.Domain.Models;
using Etherscan.Infrastructure.Repositories;
using WebApi.Models;

namespace WebApi.Services
{
    public class TokenSupplyService
    {
        private readonly IRepository<Token> _tokenRepository;

        public TokenSupplyService(IRepository<Token> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public List<TokenAndSupply> GetTotalSupplyByName()
        {
            var tokens = _tokenRepository.GetAll();
            var list = tokens
                .GroupBy(m => m.Name)
                .Select(g => new TokenAndSupply()
                {
                    Name = g.Key,
                    TotalSupply = g.Sum(k => k.TotalSupply)
                })
                .ToList();
            
            return list;
        }
    }
}
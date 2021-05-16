using System.ComponentModel.DataAnnotations.Schema;

namespace Etherscan.Domain.Models
{
    [Table("token")]
    public class Token
    {
        [Column("id")] public int Id { get; set; }
        [Column("symbol")] public string Symbol { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("total_supply")] public long TotalSupply { get; set; }
        [Column("contract_address")] public string ContractAddress { get; set; }
        [Column("total_holders")] public int TotalHolders { get; set; }
    }
}
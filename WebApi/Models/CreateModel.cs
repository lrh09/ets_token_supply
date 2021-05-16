using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CreateModel
    {
        [Required] [MaxLength(5, ErrorMessage = "Max Length is 5")] public string Symbol { get; set; }
        [Required] [MaxLength(50, ErrorMessage = "Max Length is 50")] public string Name { get; set; }
        [Required] public long TotalSupply { get; set; }
        [Required] [MaxLength(66, ErrorMessage = "Max Length is 66")] public string ContractAddress { get; set; }
        [Required] public int TotalHolders { get; set; }
    }
}
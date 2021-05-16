using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UpdateModel
    {
        [Required] public int Id { get; set; }
        [MaxLength(5, ErrorMessage = "Max Length is 5")] public string Symbol { get; set; }
        [MaxLength(50, ErrorMessage = "Max Length is 50")] public string Name { get; set; }
        public long TotalSupply { get; set; }
        [MaxLength(66, ErrorMessage = "Max Length is 66")] public string ContractAddress { get; set; }
        public int TotalHolders { get; set; }
    }
}
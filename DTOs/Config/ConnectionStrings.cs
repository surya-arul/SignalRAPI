using System.ComponentModel.DataAnnotations;

namespace SignalRAPI.DTOs.Config
{
    public class ConnectionStrings
    {
        [Required]
        public string DbConnection { get; set; } = null!;
    }
}

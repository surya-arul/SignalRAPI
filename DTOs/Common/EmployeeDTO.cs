using System.ComponentModel.DataAnnotations;

namespace SignalRAPI.DTOs.Common
{
    public class EmployeeDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Department { get; set; } = null!;
    }
}

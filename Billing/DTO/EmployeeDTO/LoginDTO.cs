using System.ComponentModel.DataAnnotations;

namespace Billing.DTO.EmployeeDTO
{
    public class LoginDTO
    {
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

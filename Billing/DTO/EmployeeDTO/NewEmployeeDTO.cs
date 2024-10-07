using System.ComponentModel.DataAnnotations;

namespace Billing.DTO.EmployeeDTO
{
    public class NewEmployeeDTO
    {
        public string Id { get; set; }
        [Required]
        public string EmployeeName {  get; set; }
        [Required]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
       
    }
}

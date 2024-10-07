using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Billing.DTO.CompanyDTO
{
    [Index(nameof(CompanyName), IsUnique = true)]
    public class CompanyDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }
        public string? Notes { get; set; }
        

    }
}

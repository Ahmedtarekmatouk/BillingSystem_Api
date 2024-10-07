using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Billing.DTO.UnitsDTOS
{
    [Index(nameof(Name), IsUnique = true)]
    public class UnitDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UNIT NAME is Required")]
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}

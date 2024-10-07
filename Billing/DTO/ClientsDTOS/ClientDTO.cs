using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Billing.DTO.ClientsDTOS
{
    [Index(nameof(Name), IsUnique = true)]
    public class ClientDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Client Name is required")]
  
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string Phone { get; set; }

        public int Number { get; set; }

        public string Address { get; set; }
    }
}

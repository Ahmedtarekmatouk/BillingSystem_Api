using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Models
{
    public class Stocks
    {
        public int Id { get; set; }
        public int Quntity { get; set; } = 0;
        [ForeignKey("Item")]
        public int ItemsId { get; set; }
        public virtual  Item Items { get; set; }

    }
}

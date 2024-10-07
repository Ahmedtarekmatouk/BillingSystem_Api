namespace Billing.DTO
{
    public class InvoiceItemDTO
    {
        public int? Id { get; set; } // For update
        public int ItemId { get; set; }
        public int UnitId { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}

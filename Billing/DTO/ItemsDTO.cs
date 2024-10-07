using Billing.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.DTO
{
    public class ItemsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int CompanyId { get; set; }
        public int? TypeId { get; set; }
        public int? UnitId {  get; set; } 
        public string? UnitName { get; set; }
        public int OpeningBalance { get; set; }
    }
}

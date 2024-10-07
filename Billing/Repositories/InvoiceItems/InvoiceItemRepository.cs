using Billing.Models;
using System.Linq;

namespace Billing.Repositories.InvoiceItems
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly BillingContext context;
        public InvoiceItemRepository(BillingContext context)
        {
            this.context = context;
        }
        public InvoiceItem Add(InvoiceItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var entry = context.Add(item);
            Save();
            return entry.Entity;
        }

        public InvoiceItem Update(InvoiceItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var entry = context.Update(item);
            Save();
            return entry.Entity;
        }

        public void Delete(int id)
        {
            var invoiceItem = GetById(id);
            if (invoiceItem!= null)
            {
                invoiceItem.IsDeleted = true;
                context.Update(invoiceItem);
                Save();
            }

        }

        public InvoiceItem GetById(int id)
        {
            return context.InvoiceItems.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
        }

        public List<InvoiceItem> GetAll()
        {
            return context.InvoiceItems.Where(u => !u.IsDeleted).ToList();
        }

        public List<InvoiceItem> GetAllByInvoiceId(int invoiceId)
        {
            return context.InvoiceItems.Where(i => i.InvoiceId == invoiceId && !i.IsDeleted).ToList();
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public List<InvoiceItem> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}

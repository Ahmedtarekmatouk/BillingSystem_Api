using Billing.Models;
using Microsoft.EntityFrameworkCore;


namespace Billing.Repositories.Invoices
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BillingContext context;
        public InvoiceRepository(BillingContext context)
        {
            this.context = context;
        }
        public Invoice Add(Invoice item)
        {
            if (item == null) {throw new ArgumentNullException("item"); }
            var dbObj = context.Invoices.Add(item);
            return dbObj.Entity;
        }
        public Invoice GetById(int id)
        {
            return context.Invoices
                .Include(i => i.InvoiceItems.Where(i => i.IsDeleted == false))
                .FirstOrDefault(i => i.Id == id && i.IsDeleted == false);
        }


        public Invoice Update(Invoice item)
        {
            if (item == null) { throw new ArgumentNullException("item"); }

            var dbObj = context.Invoices.Update(item);
  
            return dbObj.Entity;
        }

        public void Delete(int id)
        {
            Invoice item = GetById(id);
            item.IsDeleted = true;

        }

        public List<Invoice> GetAll()
        {
            return context.Invoices
                .Where(i => i.IsDeleted == false)
                .Include(i => i.InvoiceItems.Where(ii => ii.IsDeleted == false))
                .ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Invoice> GetByName(string name)
        {
            throw new NotImplementedException();
        }
       
    }
}


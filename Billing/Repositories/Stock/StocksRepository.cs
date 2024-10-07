using Billing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Billing.Repositories.Stock
{
    public class StocksRepository : IStocksRepository
    {
        BillingContext _billingContext;
        public StocksRepository(BillingContext billingContext) 
        { 
            this ._billingContext = billingContext;
        }
        public Stocks Add(Stocks entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var entry= _billingContext.Add(entity);
            Save();
            return entry.Entity;

        }

        public void Delete(int id)
        {
            var stocks=GetById(id);
            _billingContext.Remove(stocks);
            Save();
        }

        public List<Stocks> GetAll()
        {
            return _billingContext.Stocks
                          .Include(s => s.Items) 
                          .ToList();
        }

        public Stocks GetById(int id)
        {
            return _billingContext.Stocks
                         .Include(s => s.Items) 
                         .FirstOrDefault(s => s.Id == id);
        }

        public List<Stocks> GetByName(string name)
        {
            return _billingContext.Stocks
                         .Include(s => s.Items) 
                         .Where(s => s.Items.Name.Contains(name))
                         .ToList();
        }

        public Stocks Update(Stocks entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));
            var entry = _billingContext.Update(entity);
            Save();
            return entry.Entity;
        }
        public void Save()
        {
            _billingContext.SaveChanges();
        }
    }
}

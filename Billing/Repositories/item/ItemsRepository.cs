using Billing.DTO;
using Billing.Models;
using Microsoft.EntityFrameworkCore;


using System.Collections.Generic;

namespace Billing.Repositories.item
{
    public class ItemsRepository : IItemsRepository
    {
        BillingContext billingContext;
        public ItemsRepository(BillingContext billingContext)
        {
            this.billingContext = billingContext;
        }
        
            public Item Add(Item entity)
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                
                var item = CheckByName(entity.Name);
                if (item != null)
                {
                    throw new Exception("This item already exists, please check the name.");
                }

                var entry = billingContext.Items.Add(entity);
                save();
                return entry.Entity;
            }
        

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                item.IsDeleted = true;
                billingContext.Items.Update(item);
                save();
            }


        }

        public List<Item> GetAll()
        {
            return billingContext.Items.Include(i =>  i.Unit)
                .Include(s => s.Stock)
                .Where(p => p.IsDeleted == false).ToList();
        }

        public Item GetById(int id)
        {
            return billingContext.Items.Include(i=>i.Unit)
                .Include(s => s.Stock)
                .Where(p => p.Id == id && p.IsDeleted == false).FirstOrDefault();
        }

        public List<Item> GetItemsByTypeID(int id)
        {
            return billingContext.Items
    .Include(s => s.Unit)
    .Include(s => s.Stock)
    .Where(p => p.TypeId == id)
    .ToList();
        }

        public Item Update(Item entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var item = CheckByName(entity.Name);
            if (item != null) { throw new Exception("this is already excist please check the name  "); }
            var entry = billingContext.Items.Update(entity);
            save();
            return entry.Entity;
        }
        public void save()
        {
            billingContext.SaveChanges();
        }
        public List<Item> GetByName(string name)
        {
            throw new NotImplementedException();
        }
        
        public Item CheckByName(string name)
        {
            var item = billingContext.Items.Where(i => i.Name.ToLower() == name.ToLower() && i.IsDeleted==false).FirstOrDefault();
            if(item != null)
            {
                return item;
            }
            return null;
        }




    }
}

using Billing.Models;


namespace Billing.Repositories.Clients
{
    public class ClientRepository : IClientRepository
    {
        BillingContext BillingContext;
        public ClientRepository(BillingContext billingContext)
        {
            this.BillingContext = billingContext;
        }
        public Client Add(Client entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var client = CheckByName(entity.Name);
            if (client != null) throw new Exception("This item already exists, please check the name.");

            entity.Number = BillingContext.Clients.Max(c => (int?)c.Number) + 1 ?? 1;

            var clients = BillingContext.Clients.Add(entity);
            Save();
            return clients.Entity;
        }

        public void Delete(int id)
        {
            var client = GetById(id);
            if (client != null)
            {
                client.IsDeleted = true;
                BillingContext.Clients.Update(client);
                Save();
            }
        }

        public List<Client> GetAll()
        {
            return BillingContext.Clients.Where(c => !c.IsDeleted).ToList();
        }

        public Client GetById(int id)
        {
            var client = BillingContext.Clients.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            return client;


        }

        public Client Update(Client entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var client = CheckByName(entity.Name);
            if (client != null) throw new Exception("This item already exists, please check the name.");
            var clients = BillingContext.Clients.Update(entity);
            Save();
            return clients.Entity;
        }
        public List<Client> GetByName(string name)
        {
            return BillingContext.Clients
                .Where(c => c.Name.Contains(name) && !c.IsDeleted)
                .ToList();
        }

        public void Save()
        {
            BillingContext.SaveChanges();
        }
        public Client CheckByName(string name)

        {
            var client = BillingContext.Clients.Where(i => i.Name.ToLower() == name.ToLower() && i.IsDeleted == false).FirstOrDefault();
            if (client != null)
            {
                return client;
            }
            return null; 
        }


    }
}

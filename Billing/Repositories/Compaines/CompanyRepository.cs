using Billing.Models;
using System.Data;

namespace Billing.Repositories.Compaines
{

    public class CompanyRepository : ICompanyRepository
    {
        BillingContext _billingContext;

        public CompanyRepository(BillingContext billingContext)
        {
            this._billingContext = billingContext;
        }

        public Company Add(Company entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));
            var company = CheckByName(entity.Name);
            if (company != null) throw new Exception("This item already exists, please check the name.");
            var entry = _billingContext.Companies.Add(entity);
            Save();
            return entry.Entity;
        }

        public void Delete(int id)
        {
            var comp = GetById(id);
            if (comp != null)
            {
                comp.IsDeleted = true; 
                _billingContext.Companies.Update(comp); 
                Save();
            }
        }

        public List<Company> GetAll()
        {
            return _billingContext.Companies
                .Where(c => !c.IsDeleted) 
                .ToList();
        }

        public Company GetById(int id)
        {
            return _billingContext.Companies.FirstOrDefault(x => x.Id == id && !x.IsDeleted); 
        }

        public Company Update(Company entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));
            var company = CheckByName(entity.Name);
            if (company != null) throw new Exception("This item already exists, please check the name.");
            var entry = _billingContext.Companies.Update(entity);
            Save();
            return entry.Entity;
        }
        public List<Company> GetByName(string name)
        {
            return _billingContext.Companies
                .Where(c => c.Name.Contains(name) && !c.IsDeleted)
                .ToList();
        }

        public void Save()
        {
            _billingContext.SaveChanges();
        }
        public Company CheckByName(string name)
        {

            var company = _billingContext.Companies.Where(i => i.Name.ToLower() == name.ToLower() && i.IsDeleted == false).FirstOrDefault();
            if (company != null)
            {
                return company;
            }
            return null;
        }
    }

}

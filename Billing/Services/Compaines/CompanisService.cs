using Billing.Models;
using Billing.Repositories.Compaines;

namespace Billing.Services.Compaines
{
    public class CompanisService : ICompanyService
    {
        ICompanyRepository CompRepo;
        public CompanisService(ICompanyRepository CompRepo ) 
        {
            this.CompRepo = CompRepo;
        }
        public Company Create(Company entity)
        {
            return CompRepo.Add( entity );
            
        }

        public void Delete(int id)
        {
            CompRepo.Delete( id );
        }

        public List<Company> GetAll()
        {
            return CompRepo.GetAll();
        }

        public Company GetById(int id)
        {
            return CompRepo.GetById( id );
        }

        public Company Update(Company entity)
        {
           return CompRepo.Update ( entity );
        }
        public List<Company> GetByName(string name)
        {
            return CompRepo.GetByName( name );
        }
    }
}

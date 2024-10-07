using Billing.Models;
using Billing.Repositories.Units;

namespace Billing.Services.Units
{
    public class UnitService : IUnitService
    {
        IUnitRepository UnitRepository;
        public UnitService( IUnitRepository unitRepository) 
        {
        this.UnitRepository = unitRepository;
        }
        public Unit Create(Unit entity)
        {
         return UnitRepository.Add(entity);   
        }

        public void Delete(int id)
        {
            UnitRepository.Delete(id);      
        }

        public List<Unit> GetAll()
        {
            return UnitRepository.GetAll();
        }

        public Unit GetById(int id)
        {
           return   UnitRepository.GetById(id); 
        }

        public List<Unit> GetByName(string name)
        {
            return UnitRepository.GetByName(name);  
        }

        public Unit Update(Unit entity)
        {
            return UnitRepository.Update(entity);   
        }
    }
}

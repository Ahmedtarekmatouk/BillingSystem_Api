using Billing.Models;


namespace Billing.Repositories.Units
{
    public class UnitRepository : IUnitRepository
    {
        BillingContext _billingContext;
        public UnitRepository( BillingContext billingContext) 
        {
            this._billingContext = billingContext;
        }
        public Unit Add(Unit entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var unit= CheckByName(entity.Name);
            if (unit != null) new Exception("This item already exists, please check the name.");
            var entry =_billingContext.Units.Add(entity);
            Save();
            return entry.Entity;
        }

        public void Delete(int id)
        {
            var unit =GetById(id);
            if (unit != null) 
            {
                unit.IsDeleted = true;
                _billingContext.Units.Update(unit);
                Save();
            }

        }

        public List<Unit> GetAll()
        {
            return _billingContext.Units.Where(u => !u.IsDeleted).ToList();
        }

        public Unit GetById(int id)
        {
            return _billingContext.Units.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
        }

        public List<Unit> GetByName(string name)
        {
            return _billingContext.Units
               .Where(u => u.Name.Contains(name) && !u.IsDeleted)
               .ToList();
        }

        public Unit Update(Unit entity)
        {
            if (entity == null) throw new ArgumentException(nameof(entity));
            var unit = CheckByName(entity.Name);
            if (unit != null) new Exception("This item already exists, please check the name.");
            var entry = _billingContext.Units.Update(entity);
            Save();
            return entry.Entity;
        }
        public void Save()
        {
            _billingContext.SaveChanges();
        }
        public Unit CheckByName(string name)

        {
            var unit = _billingContext.Units.Where(i => i.Name.ToLower() == name.ToLower() && i.IsDeleted == false).FirstOrDefault();
            if (unit != null)
            {
                return unit;
            }
            return null;


            return null;
        }
    }
}

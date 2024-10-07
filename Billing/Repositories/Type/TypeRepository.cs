using Billing.Models;
using Billing.Repositories;
using Microsoft.EntityFrameworkCore;


public class TypeRepository : ITypeRepository
{
    private readonly BillingContext _context;

    public TypeRepository(BillingContext context)
    {
        _context = context;
    }

    public List<Billing.Models.Type> GetAll()
    {
        return _context.Types.Where(t => !t.IsDeleted).Include(t => t.Company).ToList();
    }

    public Billing.Models.Type GetById(int id)
    {
        return _context.Types.FirstOrDefault(t => t.Id == id && !t.IsDeleted);
    }

    public Billing.Models.Type Add(Billing.Models.Type entity)
    {
        if (entity == null) throw new ArgumentException(nameof(entity));
        var type = CheckByName(entity.Name);
        if (type != null) throw new Exception("This item already exists, please check the name.");
        var entry = _context.Types.Add(entity);
        _context.SaveChanges();
        return entry.Entity;
    }

    public Billing.Models.Type Update(Billing.Models.Type entity)
    {
       
        var existingType = _context.Types.Find(entity.Id);
        if (entity == null) throw new ArgumentException(nameof(entity));
        var company = CheckByName(entity.Name);
        if (company != null) throw new Exception("This item already exists, please check the name.");
        var entry = _context.Update(entity);
        _context.SaveChanges();
        return entry.Entity;
    }


    public void Delete(int id)
    {
        var type = _context.Types.FirstOrDefault(t => t.Id == id);
        if (type != null)
        {
            type.IsDeleted = true;
            _context.SaveChanges();
        }
    }

    public bool IsTypeNameUnique(string typeName)
    {
        throw new NotImplementedException();
    }

    public List<Billing.Models.Type> GetByName(string name)
    {
        throw new NotImplementedException();
    }
    public Billing.Models.Type CheckByName(string name)
    {
        var type  = _context.Types.Where(i => i.Name.ToLower() == name.ToLower() && i.IsDeleted == false).FirstOrDefault();
        if (type != null)
        {
            return type;
        }
        return null;
    }
}

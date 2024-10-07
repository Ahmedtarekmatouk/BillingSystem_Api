namespace Billing.Services
{
    public interface ICRUDService<T> : IService
    {
        List<T> GetAll();
        T GetById(int id);
        T Create(T entity);
        T Update(T entity);
        void Delete(int id);
        List<T> GetByName(string name);
    }
}

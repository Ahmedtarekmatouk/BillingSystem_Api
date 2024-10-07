using Billing.Models;
using Billing.Repositories.Clients;
namespace Billing.Services.Clients
{
    public class ClientService : IClientService
    {
        IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
           this._clientRepository = clientRepository;
        }
        public Client Create(Client entity)
        {
            return _clientRepository.Add(entity);
        }

        public void Delete(int id)
        {
            _clientRepository.Delete(id);
        }

        public List<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public Client GetById(int id)
        {
            return _clientRepository.GetById(id);
        }

        public Client Update(Client entity)
        {
            return _clientRepository.Update(entity);
        }
        public List<Client> GetByName(string name)
        {
            return _clientRepository.GetByName(name);
        }
    }
}

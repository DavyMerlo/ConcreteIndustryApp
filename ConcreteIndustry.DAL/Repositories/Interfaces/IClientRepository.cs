using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client?> GetClientByIdAsync(long id);
        Task<int> AddClientAsync(Client client);
        Task<bool> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(long id);
        Task<bool> DoesClientExist(long id);
    }
}

using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.Clients;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public ClientService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger, 
            ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<ClientDTO>> GetAll()
        {
            try
            {
                var cachedClients = cacheService.GetData<IEnumerable<Client>>(CacheSettings.Key.Clients);
                if(cachedClients != null && cachedClients.Any())
                {
                    return mapper.Map<IEnumerable<ClientDTO>>(cachedClients);
                }

                var clients = await unitOfWork.Clients.GetClientsAsync();
                if (!clients.Any())
                {
                    logger.LogWarning("{Service} No clients found", typeof(ClientService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Client), null);
                }

                cacheService.SetData(CacheSettings.Key.Clients, clients, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<ClientDTO>>(clients);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get All function error", typeof(ClientService));
                throw;
            }
        }

        public async Task<ClientDTO> GetById(long id)
        {
            try
            {
                var cachedClient = cacheService.GetData<Client>(CacheSettings.Key.Client);
                if(cachedClient != null)
                {
                    return mapper.Map<ClientDTO>(cachedClient);
                }

                var client = await unitOfWork.Clients.GetClientByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(Client), id);

                cacheService.SetData(CacheSettings.Key.Client, client, CacheSettings.CacheExpirationTime);
                return mapper.Map<ClientDTO>(client);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(ClientService));
                throw;
            }
        }
    }
}

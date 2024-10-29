using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.Addresses;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public AddressService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger, 
            ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<AddressDTO>> GetAll()
        {
            try
            {
                var cachedAddresses = cacheService.GetData<IEnumerable<Address>>(CacheSettings.Key.Addresses);
                if(cachedAddresses != null && cachedAddresses.Any())
                {
                    return mapper.Map<IEnumerable<AddressDTO>>(cachedAddresses);    
                }

                var addresses = await unitOfWork.Addresses.GetAddressesAsync();
                if (!addresses.Any()) 
                {
                    logger.LogWarning("{Service} No addresses found", typeof(AddressService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Address), null);
                }

                cacheService.SetData(CacheSettings.Key.Addresses, addresses, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<AddressDTO>>(addresses);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get All function error", typeof(AddressService));
                throw;
            }
        }

        public async Task<AddressDTO> GetById(long id)
        {
            try
            {
                var cachedAddress = cacheService.GetData<Address>(CacheSettings.Key.Address);
                if(cachedAddress != null)
                {
                    return mapper.Map<AddressDTO>(cachedAddress);
                }
                var address = await unitOfWork.Addresses.GetAddressByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(Address), id);

                cacheService.SetData(CacheSettings.Key.Address, address, CacheSettings.CacheExpirationTime);
                return mapper.Map<AddressDTO>(address);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(AddressService));
                throw;
            }
        }
    }
}

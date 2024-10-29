using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.Orders;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public OrderService(
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

        public async Task<IEnumerable<OrderDTO>> GetAll()
        {
            try
            {
                var cachedOrders = cacheService.GetData<IEnumerable<Order>>(CacheSettings.Key.Orders);
                if(cachedOrders != null && cachedOrders.Any())
                {
                    return mapper.Map<IEnumerable<OrderDTO>>(cachedOrders);
                }

                var orders = await unitOfWork.Orders.GetOrdersAsync();
                if (!orders.Any())
                {
                    logger.LogWarning("{Service} No orders found", typeof(OrderService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Order), null);
                }

                cacheService.SetData(CacheSettings.Key.Orders, orders, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<OrderDTO>>(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get All function error", typeof(OrderService));
                throw;
            }
        }

        public async Task<OrderDTO> GetById(long id)
        {
            try
            {
                var cachedOrder = cacheService.GetData<Order>(CacheSettings.Key.Order);
                if(cachedOrder != null)
                {
                    return mapper.Map<OrderDTO>(cachedOrder);
                }

                var order = await unitOfWork.Orders.GetOrderByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(Order), id);

                cacheService.SetData(CacheSettings.Key.Order, order, CacheSettings.CacheExpirationTime);
                return mapper.Map<OrderDTO>(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(OrderService));
                throw;
            }
        }
    }
}

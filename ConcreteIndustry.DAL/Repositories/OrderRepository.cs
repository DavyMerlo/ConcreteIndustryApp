using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public OrderRepository(IDataConnection connection, ILogger logger)
        {
            this.dataConnection = connection;
            this.logger = logger;   
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            try
            {
                var query = SqlHelper<OrderColumn>.CreateSelectAllQuery(TableName.Orders);

                return await dataConnection.ExecuteAsync(query, reader => new Order
                {
                    Id = reader.GetInt64((int)OrderColumn.OrderID),
                    ProjectID = reader.GetInt64((int)OrderColumn.ProjectID),
                    ClientID = reader.GetInt64((int)OrderColumn.ClientID),
                    ConcreteMixID = reader.GetInt64((int)OrderColumn.ConcreteMixID),
                    Quantity = reader.GetDecimal((int)OrderColumn.Quantity),
                    OrderDate = reader.GetDateTime((int)OrderColumn.OrderDate),
                    DeliveryDate = reader.GetDateTime((int)OrderColumn.DeliveryDate),
                    CreatedAt = reader.GetDateTime((int)OrderColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)OrderColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)OrderColumn.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(OrderRepository).ToString()} Get All {nameof(Order)} Error", typeof(OrderRepository));
                throw;
            }
        }

        public async Task<Order?> GetOrderByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper<OrderColumn>.CreateSelectByQuery(TableName.Orders, OrderColumn.OrderID);

                var parameters = SqlHelper<OrderColumn>.CreateParameters(
                   (OrderColumn.OrderID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Order
                {
                    Id = reader.GetInt64((int)OrderColumn.OrderID),
                    ProjectID = reader.GetInt64((int)OrderColumn.ProjectID),
                    ClientID = reader.GetInt64((int)OrderColumn.ClientID),
                    ConcreteMixID = reader.GetInt64((int)OrderColumn.ConcreteMixID),
                    Quantity = reader.GetDecimal((int)OrderColumn.Quantity),
                    OrderDate = reader.GetDateTime((int)OrderColumn.OrderDate),
                    DeliveryDate = reader.GetDateTime((int)OrderColumn.DeliveryDate),
                    CreatedAt = reader.GetDateTime((int)OrderColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)OrderColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)OrderColumn.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(OrderRepository).ToString()} Get {nameof(Order)} by Id Error", typeof(OrderRepository));
                throw;
            }
        }

        public async Task<int> AddOrderAsync(Order order)
        {
            try
            {
                var columns = new[]
                {
                    OrderColumn.ProjectID,
                    OrderColumn.ClientID,
                    OrderColumn.ConcreteMixID,
                    OrderColumn.Quantity,
                    OrderColumn.OrderDate,
                    OrderColumn.DeliveryDate,
                };

                var query = SqlHelper<OrderColumn>.CreateInsertQuery(TableName.Orders, OrderColumn.OrderID, columns);

                var parameters = SqlHelper<OrderColumn>.CreateParameters(
                     (OrderColumn.ProjectID, SqlDbType.BigInt, order.ProjectID),
                     (OrderColumn.ClientID, SqlDbType.BigInt, order.ClientID),
                     (OrderColumn.ConcreteMixID, SqlDbType.BigInt, order.ConcreteMixID),
                     (OrderColumn.Quantity, SqlDbType.Decimal, order.Quantity),
                     (OrderColumn.OrderDate, SqlDbType.Date, order.OrderDate),
                     (OrderColumn.DeliveryDate, SqlDbType.Date, order.DeliveryDate)

                );
                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(OrderRepository).ToString()} Add {nameof(Order)} Error", typeof(OrderRepository));
                throw;
            }
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                var columns = new[]
                {
                    OrderColumn.ProjectID,
                    OrderColumn.ClientID,
                    OrderColumn.ConcreteMixID,
                    OrderColumn.Quantity,
                    OrderColumn.OrderDate,
                    OrderColumn.DeliveryDate,
                };

                var query = SqlHelper<OrderColumn>.CreateUpdateQuery(TableName.Orders, OrderColumn.OrderID, columns);

                var parameters = SqlHelper<OrderColumn>.CreateParameters(
                     (OrderColumn.OrderID, SqlDbType.BigInt, order.Id),
                     (OrderColumn.ProjectID, SqlDbType.BigInt, order.ProjectID),
                     (OrderColumn.ClientID, SqlDbType.BigInt, order.ClientID),
                     (OrderColumn.ConcreteMixID, SqlDbType.BigInt, order.ConcreteMixID),
                     (OrderColumn.Quantity, SqlDbType.Decimal, order.Quantity),
                     (OrderColumn.OrderDate, SqlDbType.Date, order.OrderDate),
                     (OrderColumn.DeliveryDate, SqlDbType.Date, order.DeliveryDate)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(OrderRepository).ToString()} Update {nameof(Order)} Error", typeof(OrderRepository));
                throw;
            }
        }

        public async Task<bool> DeleteOrderAsync(long id)
        {
            try
            {
                var query = SqlHelper<OrderColumn>.CreateDeleteQuery(TableName.Orders, OrderColumn.OrderID);

                var parameters = SqlHelper<OrderColumn>.CreateParameters(
                    (OrderColumn.OrderID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(OrderRepository).ToString()} Delete {nameof(Order)} Error", typeof(OrderRepository));
                throw;
            }
        }
    }
}

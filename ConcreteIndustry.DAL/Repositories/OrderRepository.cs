using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
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
                var query = SqlHelper.CreateSelectAllQuery(Table.Orders);

                return await dataConnection.ExecuteAsync(query, reader => new Order
                {
                    Id = reader.GetInt64(Column.Order.OrderID),
                    ProjectID = reader.GetInt64(Column.Order.ProjectID),
                    ClientID = reader.GetInt64(Column.Order.ClientID),
                    ConcreteMixID = reader.GetInt64(Column.Order.ConcreteMixID),
                    Quantity = reader.GetDecimal(Column.Order.Quantity),
                    OrderDate = reader.GetDateTime(Column.Order.OrderDate),
                    DeliveryDate = reader.GetDateTime(Column.Order.DeliveryDate),
                    CreatedAt = reader.GetDateTime(Column.Order.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Order.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Order.DeletedAt),
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
                var query = SqlHelper.CreateSelectByQuery(Table.Orders, Column.Order.OrderID);

                var parameters = SqlHelper.CreateParameters(
                   (Column.Order.OrderID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Order
                {
                    Id = reader.GetInt64(Column.Order.OrderID),
                    ProjectID = reader.GetInt64(Column.Order.ProjectID),
                    ClientID = reader.GetInt64(Column.Order.ClientID),
                    ConcreteMixID = reader.GetInt64(Column.Order.ConcreteMixID),
                    Quantity = reader.GetDecimal(Column.Order.Quantity),
                    OrderDate = reader.GetDateTime(Column.Order.OrderDate),
                    DeliveryDate = reader.GetDateTime(Column.Order.DeliveryDate),
                    CreatedAt = reader.GetDateTime(Column.Order.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Order.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Order.DeletedAt),
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
                    Column.Order.ProjectID,
                    Column.Order.ClientID,
                    Column.Order.ConcreteMixID,
                    Column.Order.Quantity,
                    Column.Order.OrderDate,
                    Column.Order.DeliveryDate,
                };

                var query = SqlHelper.CreateInsertQuery(Table.Orders, Column.Order.OrderID, columns);

                var parameters = SqlHelper.CreateParameters(
                     (Column.Order.ProjectID, SqlDbType.BigInt, order.ProjectID),
                     (Column.Order.ClientID, SqlDbType.BigInt, order.ClientID),
                     (Column.Order.ConcreteMixID, SqlDbType.BigInt, order.ConcreteMixID),
                     (Column.Order.Quantity, SqlDbType.Decimal, order.Quantity),
                     (Column.Order.OrderDate, SqlDbType.Date, order.OrderDate),
                     (Column.Order.DeliveryDate, SqlDbType.Date, order.DeliveryDate)

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
                    Column.Order.ProjectID,
                    Column.Order.ClientID,
                    Column.Order.ConcreteMixID,
                    Column.Order.Quantity,
                    Column.Order.OrderDate,
                    Column.Order.DeliveryDate,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.Orders, Column.Order.OrderID, columns);

                var parameters = SqlHelper.CreateParameters(
                     (Column.Order.OrderID, SqlDbType.BigInt, order.Id),
                     (Column.Order.ProjectID, SqlDbType.BigInt, order.ProjectID),
                     (Column.Order.ClientID, SqlDbType.BigInt, order.ClientID),
                     (Column.Order.ConcreteMixID, SqlDbType.BigInt, order.ConcreteMixID),
                     (Column.Order.Quantity, SqlDbType.Decimal, order.Quantity),
                     (Column.Order.OrderDate, SqlDbType.Date, order.OrderDate),
                     (Column.Order.DeliveryDate, SqlDbType.Date, order.DeliveryDate)
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
                var query = SqlHelper.CreateDeleteQuery(Table.Orders, Column.Order.OrderID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Order.OrderID, SqlDbType.BigInt, id)
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

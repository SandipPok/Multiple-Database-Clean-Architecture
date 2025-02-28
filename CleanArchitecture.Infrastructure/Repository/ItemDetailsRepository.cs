using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using Dapper;
using Domain_Layer.Modal;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class ItemDetailsRepository : IItemDetailsRepository
    {
        private readonly DbContext _dbContext;

        public ItemDetailsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ItemDetail>> GetAllDetailsAsync()
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"SELECT 
                                    detail_id AS DetailId,
                                    item_id AS ItemId,
                                    detail_description AS DetailDescription
                                    FROM ItemDetails";

                    var result = await connection.QueryAsync<ItemDetail>(query);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ItemDetail>> GetDetailsByItemIdAsync(int itemId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"SELECT 
                                    detail_id AS DetailId,
                                    item_id AS ItemId,
                                    detail_description AS DetailDescription
                                    FROM ItemDetails 
                                    WHERE item_id = @ItemId";
                    var parameters = new { ItemId = itemId };
                    var result = await connection.QueryAsync<ItemDetail>(query, parameters);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

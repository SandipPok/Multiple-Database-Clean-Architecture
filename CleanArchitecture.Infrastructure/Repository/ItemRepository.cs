using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using Dapper;
using Domain_Layer.Modal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DbContext _dbContext;

        public ItemRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"INSERT INTO Items 
                                  (item_name, item_description) 
                                  VALUES 
                                  (@ItemName, @ItemDescription); 
                                  SELECT CAST(SCOPE_IDENTITY() as int)";
                    return await connection.QuerySingleAsync<int>(query, new { item.ItemName, item.ItemDescription });
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = "SELECT * FROM Items";
                    return await connection.QueryAsync<Item>(query);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Item> GetItemByIdAsync(int itemId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = "SELECT * FROM Items WHERE item_id = @ItemId";
                    return await connection.QuerySingleOrDefaultAsync<Item>(query, new { ItemId = itemId });
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = "UPDATE Items SET item_name = @ItemName, item_description = @ItemDescription WHERE item_id = @ItemId";
                    return await connection.ExecuteAsync(query, new { item.ItemName, item.ItemDescription, item.ItemId });
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> DeleteItemAsync(int itemId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = "DELETE FROM Items WHERE item_id = @ItemId";
                    return await connection.ExecuteAsync(query, new { ItemId = itemId });
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

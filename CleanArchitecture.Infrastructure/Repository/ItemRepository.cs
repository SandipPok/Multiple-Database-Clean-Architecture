using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using Dapper;
using Domain_Layer.Modal;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
                    var query = @"SELECT 
                                    item_id AS ItemId,
                                    item_name AS ItemName,
                                    item_description AS ItemDescription
                                    FROM Items";
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
                    var query = @"SELECT 
                                    item_id AS ItemId,
                                    item_name AS ItemName,
                                    item_description AS ItemDescription
                                    FROM Items WHERE item_id = @ItemId";
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
                    StringBuilder query = new StringBuilder();
                    query.Append("UPDATE Items SET ");

                    var parameters = new DynamicParameters();

                    bool hasValidUpdate = false;

                    if (!string.IsNullOrEmpty(item.ItemName))
                    {
                        query.Append("item_name = @ItemName, ");
                        parameters.Add("ItemName", item.ItemName);
                        hasValidUpdate = true;
                    }

                    if (!string.IsNullOrEmpty(item.ItemDescription))
                    {
                        query.Append("item_description = @ItemDescription, ");
                        parameters.Add("ItemDescription", item.ItemDescription);
                        hasValidUpdate = true;
                    }

                    if (!hasValidUpdate)
                    {
                        return 0;
                    }

                    if (query.ToString().EndsWith(", "))
                    {
                        query.Length -= 2;
                    }

                    query.Append(" WHERE item_id = @ItemId");
                    parameters.Add("ItemId", item.ItemId);

                    return await connection.ExecuteAsync(query.ToString(), parameters);
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
                    // Start a transaction to ensure atomicity of delete operations
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var deleteItemQuery = "DELETE FROM Items WHERE item_id = @ItemId";
                            var rowsAffected = await connection.ExecuteAsync(deleteItemQuery, new { ItemId = itemId }, transaction);

                            if (rowsAffected == 0)
                            {
                                return 0;
                            }

                            transaction.Commit();
                            return rowsAffected;
                        }
                        catch (SqlException sqlEx) when (sqlEx.Number == 547)
                        {
                            try
                            {
                                var deleteDetailsQuery = "DELETE FROM ItemDetails WHERE item_id = @ItemId";
                                var rowAffected = await connection.ExecuteAsync(deleteDetailsQuery, new { ItemId = itemId }, transaction);

                                var deleteItemQuery = "DELETE FROM Items WHERE item_id = @ItemId";
                                await connection.ExecuteAsync(deleteItemQuery, new { ItemId = itemId }, transaction);

                                transaction.Commit();
                                return rowAffected;
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task CreateItemWithDetails(Item item, List<ItemDetail> itemDetails)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertItemQuery = @"
                    INSERT INTO Items (item_name, item_description)
                    VALUES (@ItemName, @ItemDescription);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        item.ItemId = (await connection.QueryAsync<int>(insertItemQuery, item, transaction)).FirstOrDefault();

                        string insertDetailsQuery = @"
                    INSERT INTO ItemDetails (item_id, detail_description)
                    VALUES (@ItemId, @DetailDescription);";

                        foreach (var detail in itemDetails)
                        {
                            detail.ItemId = item.ItemId;
                            connection.Execute(insertDetailsQuery, detail, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

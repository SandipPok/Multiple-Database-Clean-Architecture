using Domain_Layer.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interface
{
    public interface IItemRepository
    {
        Task<int> CreateItemAsync(Item item);
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int itemId);
        Task<int> UpdateItemAsync(Item item);
        Task<int> DeleteItemAsync(int itemId);
    }
}

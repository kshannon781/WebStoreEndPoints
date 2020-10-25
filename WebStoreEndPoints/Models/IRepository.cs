using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebStoreEndPoints.Models
{
    public interface IRepository
    {
        IEnumerable<StoreItem> StoreItem { get; }
        StoreItem this[int id] { get; }
        StoreItem AddStoreItem(StoreItem storeItems);
        StoreItem UpdateStoreItem(StoreItem storeItems);
        void DeleteStoreItem(int id);
        int GetMax(string name);
        List<string> GetListItems();
        IEnumerable<StoreItem> GetMaxGroup();
    }
}

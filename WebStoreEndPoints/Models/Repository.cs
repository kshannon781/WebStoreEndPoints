using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebStoreEndPoints.Models
{
    public class Repository : IRepository
    {
        private Dictionary<int, StoreItem> items;

        public Repository()
        {
            items = new Dictionary<int, StoreItem>();
            //new List<StoreItem>
            //{
            //    new StoreItem {Id = 1, ItemName="Table", Cost=20},
            //    new StoreItem {Id = 2, ItemName="Table", Cost=2},
            //    new StoreItem {Id = 3, ItemName="Chair", Cost=96},
            //    new StoreItem {Id = 4, ItemName="Lamp", Cost=25},
            //}.ForEach(r => AddStoreItem(r));
        }

        public StoreItem this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<StoreItem> StoreItem => items.Values;

        public StoreItem AddStoreItem(StoreItem storeItem)
        {
           if(storeItem.Id == 0)
            {
                int key = items.Count == 0 ? 1 : items.Count();
                while (items.ContainsKey(key)) { key++; };
                storeItem.Id = key;
            }

            items[storeItem.Id] = storeItem;
            return storeItem;
        }

        public int GetMax(string max)
        {
            try
            {
                var x = items.Values.Where(s => String.Equals(s.ItemName, max, StringComparison.CurrentCultureIgnoreCase)).OrderByDescending(t => t.Cost).First();
                return x.Cost;
            }
            catch (Exception)
            {

                return 0;
            }
           
        }


        public IEnumerable<StoreItem> GetMaxGroup()
        {
               var query = from item in items.Values
                           group item by item.ItemName into itemGroup
                           select new
                           {
                               ItemName = itemGroup.Key.ToString(),
                               Cost = itemGroup.Sum(t => t.Cost),
                           };
            int count = 1;
            items = new Dictionary<int, StoreItem>();
            foreach (var q in query)
            {
                new List<StoreItem>
            {
                new StoreItem {Id = count, ItemName=q.ItemName, Cost=q.Cost}

            }.ForEach(r => AddStoreItem(r));
                count++;
            }
            return items.Values;


        }

        public void DeleteStoreItem(int id) => items.Remove(id);

        public StoreItem UpdateStoreItem(StoreItem storeItem) => AddStoreItem(storeItem);

        public List<string> GetListItems()
        {
            var query = from item in items.Values
                        group item by item.ItemName into itemGroup
                        select itemGroup.Key;
          
            return query.ToList();
        }
    }
}

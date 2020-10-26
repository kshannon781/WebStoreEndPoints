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
        private Dictionary<int, StoreItemTemp> itemsTemp;

        public Repository()
        {
            itemsTemp = new Dictionary<int, StoreItemTemp>();
            items = new Dictionary<int, StoreItem>();
            new List<StoreItem>
            {
                new StoreItem {Id = 1, ItemName="ITEM 1", Cost=100},
                new StoreItem {Id = 2, ItemName="ITEM 2", Cost=200},
                new StoreItem {Id = 3, ItemName="ITEM 1", Cost=250},
                new StoreItem {Id = 4, ItemName="ITEM 3", Cost=300},
                new StoreItem {Id = 5, ItemName="ITEM 4", Cost=50},
                new StoreItem {Id = 6, ItemName="ITEM 4", Cost=40},
                new StoreItem {Id = 7, ItemName="ITEM 2", Cost=200},
            }.ForEach(r => AddStoreItem(r));
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
                var x = items.Values.Where(s => String.Equals(s.ItemName, max, StringComparison.CurrentCultureIgnoreCase)).OrderByDescending(t => t.Cost);
                var newVal = 0;
                foreach (var num in x)
                {
                    newVal += num.Cost;
                }
                return newVal;
            }
            catch (Exception)
            {

                return 0;
            }
           
        }


        public IEnumerable<StoreItemTemp> GetMaxGroup()
        {
               var query = from item in items.Values
                           group item by item.ItemName into itemGroup
                           select new
                           {
                               ItemName = itemGroup.Key.ToString(),
                               Cost = itemGroup.Sum(t => t.Cost),
                           };
            int count = 1;
            itemsTemp = new Dictionary<int, StoreItemTemp>();
            foreach (var q in query)
            {
                new List<StoreItemTemp>
            {
                new StoreItemTemp {Id = count, ItemName=q.ItemName, Cost=q.Cost}

            }.ForEach(r => addToTemp(r));

                count++;
            }
            return itemsTemp.Values;


        }

        private void addToTemp(StoreItemTemp storeItem)
        {
            if (storeItem.Id == 0)
            {
                int key = items.Count == 0 ? 1 : items.Count();
                while (items.ContainsKey(key)) { key++; };
                storeItem.Id = key;
            }

            itemsTemp[storeItem.Id] = storeItem;
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

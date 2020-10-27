using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebStoreEndPoints.Models;
using System.Text.RegularExpressions;

namespace WebStoreEndPointTest
{
    class WebStoreServiceFake : IRepository
    {
        private Dictionary<int, StoreItem> items;
        private Dictionary<int, StoreItemTemp> itemsTemp;
        public WebStoreServiceFake()
        {
            items = new Dictionary<int, StoreItem>();
            new List<StoreItem>
            {
                new StoreItem {Id = 1, ItemName="ITEM 1", Cost="100"},
                new StoreItem {Id = 2, ItemName="ITEM 2", Cost="200"},
                new StoreItem {Id = 3, ItemName="ITEM 1", Cost="250"},
                new StoreItem {Id = 4, ItemName="ITEM 3", Cost="300"},
                new StoreItem {Id = 5, ItemName="ITEM 4", Cost="50"},
                new StoreItem {Id = 6, ItemName="ITEM 4", Cost="40"},
                new StoreItem {Id = 7, ItemName="ITEM 2", Cost="200"},
            }.ForEach(r => AddStoreItem(r));
        }

        public StoreItem this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<StoreItem> StoreItem => items.Values;

        public StoreItem AddStoreItem(StoreItem storeItem)
        {
            if (storeItem.Id == 0)
            {
                int key = items.Count;
                while (items.ContainsKey(key)) { key++; };
                storeItem.Id = key;
            }

            items[storeItem.Id] = storeItem;
            return storeItem;
        }

        public double IntCheckNoResponse(string stringToPass)
        {
            try
            {
                return Convert.ToDouble(stringToPass);
            }
            catch
            { return 0; }
        }

        public string GetMax(string max)
        {
            try
            {
                double sumVal = 0;
                var query = from item in items.Values
                            group item by item.ItemName.ToLower() into itemGroup
                            where String.Equals(itemGroup.Key,max, StringComparison.CurrentCultureIgnoreCase)
                            select itemGroup.Sum(t => IntCheckNoResponse(t.Cost.Replace(",", "").Replace("$", "").Trim()));

                foreach (var num in query)
                {
                    sumVal = sumVal + num;
                }
                return sumVal.ToString("c2");
            }
            catch (Exception ex)
            {

                return 0.ToString("c2");
            }

        }


        public void DeleteStoreItem(int id) => items.Remove(id);

        public StoreItem UpdateStoreItem(StoreItem storeItem) => AddStoreItem(storeItem);

        public List<string> GetListItems()
        {
            var query = from item in items.Values
                        group item by item.ItemName.ToLower() into itemGroup
                        select itemGroup.Key;

            return query.ToList();
        }
        public IEnumerable<StoreItemTemp> GetMaxGroup()
        {
            var query = from item in items.Values
                        group item by item.ItemName.ToLower() into itemGroup
                        select new
                        {
                            ItemName = itemGroup.Key.ToString().ToUpper(),
                            Cost = itemGroup.Sum(t => Convert.ToInt32(Regex.Match(t.Cost.Replace(",", "").Trim(), @"\d+").Value)),
                        };
            int count = 1;
            itemsTemp = new Dictionary<int, StoreItemTemp>();
            foreach (var q in query)
            {
                new List<StoreItemTemp>
            {
                new StoreItemTemp {Id = count, ItemName=q.ItemName, Cost=q.Cost.ToString()}

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
    }
}

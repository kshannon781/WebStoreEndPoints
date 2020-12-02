using Microsoft.AspNetCore.Mvc;
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
           if(storeItem.Id == 0)
            {
                int key = items.Count == 0 ? 1 : items.Count();
                while (items.ContainsKey(key)) { key++; };
                storeItem.Id = key;
            }

            items[storeItem.Id] = storeItem;
            items[storeItem.Id].Cost = String.Format("{0:C2}", IntCheckNoResponse(storeItem.Cost.Replace(",", "").Replace("$", "").Trim()));//String.Format(storeItem.ItemName.ToString("c2");
            return storeItem;
        }

        public string GetMax(string max)
        {
            double sumVal = 0f;
            try
            {
               
                var query = from item in items.Values
                            group item by item.ItemName.ToLower() into itemGroup
                            where String.Equals(itemGroup.Key, max, StringComparison.CurrentCultureIgnoreCase)
                            select itemGroup.Sum(t => IntCheckNoResponse(t.Cost.Replace(",", "").Replace("$", "").Trim()));
                           
                foreach (var num in query)
                {
                    sumVal = sumVal + num;
                }
               
            }
            catch (Exception ex)
            {

               
            }
            return sumVal.ToString("c2");
        }

     
        public IEnumerable<StoreItemTemp> GetMaxGroup()
        {
               var query = from item in items.Values
                           group item by item.ItemName.ToLower() into itemGroup
                           select new
                           {
                               ItemName = itemGroup.Key.ToString().ToUpper(),//Regex.Match(t.Cost.Replace(",", "").Trim(), @"\d+").Value)),
                               Cost = itemGroup.Sum(t => IntCheckNoResponse(t.Cost.Replace(",", "").Replace("$", "").Trim())),
                           };
            int count = 1;
            itemsTemp = new Dictionary<int, StoreItemTemp>();
            foreach (var q in query)
            {
                new List<StoreItemTemp>
            {
                new StoreItemTemp {Id = count, ItemName=q.ItemName, Cost=q.Cost.ToString("c2")}

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
                        group item by item.ItemName.ToLower() into itemGroup
                        select itemGroup.Key.ToUpper();
            var tempList = query.Distinct().ToList();
            return tempList;
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

    }

    
      
    
}

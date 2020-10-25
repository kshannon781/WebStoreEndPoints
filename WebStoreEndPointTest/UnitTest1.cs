using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStoreEndPoints.Controllers;
using WebStoreEndPoints.Models;

namespace WebStoreEndPointTest
{
    public class Tests
    {
        WebStoreController _controller;
        IRepository _service;
        IWebHostEnvironment _webHostEnvironment;

        public Tests()
        {
            _service = new WebStoreServiceFake();
            _controller = new WebStoreController(_service, _webHostEnvironment);
        }

        [Test]
        public void Single_FindsExistingRecord_ByIdTest()
        {
            var repo = _controller.Get().FirstOrDefault().Id;
            Assert.AreEqual(1, repo);
        }

        [Test]
        public void AddStoreItemTest()
        {
            var testItem = "Table"; var testCost = 20;
             StoreItem testSI = new StoreItem { Id = 0, ItemName = testItem, Cost = testCost };
            var returnObj = _service.AddStoreItem(testSI);
            Assert.AreEqual(testItem, returnObj.ItemName);
            Assert.AreEqual(testCost, returnObj.Cost);
        }

        [Test]
        public void EditStoreItemTest()
        {
            var repo = _controller.Get().FirstOrDefault();
            StoreItem testSI = new StoreItem { Id = repo.Id, ItemName = "NewTestName", Cost = repo.Cost };
            var returnObj = _service.UpdateStoreItem(testSI);
            Assert.AreNotEqual(repo.ItemName, returnObj.ItemName);
            Assert.AreEqual(repo.Cost, returnObj.Cost);
            Assert.AreEqual(repo.Id, returnObj.Id);
        }

        [Test]
        public void DeleteStoreItemTest()
        {
            var repo = _controller.Get();
            var testItemName = repo.First().ItemName;
            int testCount = repo.Count();
            _service.DeleteStoreItem(repo.First().Id);

            Assert.AreNotEqual(testItemName, repo.Where(x => x.ItemName == testItemName).FirstOrDefault());
            Assert.AreNotEqual(testCount, repo.Count());
        }

        [Test]
        public void MaxCostStoreItemTest()
        {
            StoreItem testSI = new StoreItem { Id = int.MaxValue, ItemName = "testItem", Cost = 100 };
            var returnObj = _service.AddStoreItem(testSI);
            var retInt = _service.GetMax("testItem");
            _service.DeleteStoreItem(int.MaxValue);
            Assert.AreEqual(retInt, 100);
        }

        [Test]
        public void MaxGroupTableStoreItemTest()
        {
               /* new StoreItem { Id = 1, ItemName = "Table", Cost = 20 },
                new StoreItem { Id = 2, ItemName = "Table", Cost = 2 },
                new StoreItem { Id = 3, ItemName = "Chair", Cost = 96 },
                new StoreItem { Id = 4, ItemName = "Lamp", Cost = 25 },*/

            var returnString = _service.GetMaxGroup().ToList();

            Assert.AreEqual(returnString.First().Cost,22);
        }
    }
}
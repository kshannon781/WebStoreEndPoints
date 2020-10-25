using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebStoreEndPoints.Models;

namespace WebStoreEndPoints.Controllers
{
    public class HomeController : Controller
    {
        //public string Index()
        //{
        //    return "API Running...";
        //}
        public async Task<IActionResult> Index()
        {
            List<StoreItem> storeList = new List<StoreItem>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore");
                string apiResponse = await response.Content.ReadAsStringAsync();
                storeList = JsonConvert.DeserializeObject<List<StoreItem>>(apiResponse);
            }
            return View(storeList);
        }

        [HttpPost]
        public async Task<IActionResult> Get(int id)
        {
            StoreItem storeItem = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                storeItem = JsonConvert.DeserializeObject<StoreItem>(apiResponse);
            }
            return View(storeItem);
        }

        public ViewResult GetStoreItem() => View();
        [HttpPost]
        public async Task<IActionResult> GetStoreItem(int id)
        {
            StoreItem storeItem = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                storeItem = JsonConvert.DeserializeObject<StoreItem>(apiResponse);
            }
            return View(storeItem);
        }

        
        public ViewResult GetMaxPrice() => View();
        [HttpPost]
        public async Task<IActionResult> GetMaxPrice(string itemName)
        {
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/getmaxprice/" + itemName);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Max = (apiResponse);
            }
            return View();
        }


        public ViewResult GetMaxPriceGroup() => View();
        [HttpPost]
        public async Task<IActionResult> GetMaxPriceGroup(StoreItem storeItem)
        {
            List<StoreItem> storeList = new List<StoreItem>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/getmaxgroup");
                string apiResponse = await response.Content.ReadAsStringAsync();
                storeList = JsonConvert.DeserializeObject<List<StoreItem>>(apiResponse);
            }
            return View(storeList);

        }
        //[HttpGet]
        //[Produces("application/json")]
        //[Route("[action]")]
        //public IEnumerable<StoreItem> GetMaxGroup() => repository.GetMaxGroup();

        public ViewResult AddStoreItem() => View();

        [HttpPost]
        public async Task<IActionResult> AddStoreItem(StoreItem storeItem)
        {
            StoreItem receivedReservation = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                
                StringContent content = new StringContent(JsonConvert.SerializeObject(storeItem), Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    receivedReservation = JsonConvert.DeserializeObject<StoreItem>(apiResponse);
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewBag.Result = apiResponse;
                    return View();
                }
            }
            return View(receivedReservation);
        }

        public async Task<IActionResult> UpdateStoreItem(int id)
        {
            StoreItem reservation = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reservation = JsonConvert.DeserializeObject<StoreItem>(apiResponse);

            }
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStoreItem(StoreItem storeItem)
        {
            StoreItem recievedReservation = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(storeItem.Id.ToString()), "Id" },
                    { new StringContent(storeItem.ItemName), "Name" },
                    { new StringContent(storeItem.Cost.ToString()), "Cost" }
                };

                using var response = await httpClient.PutAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                recievedReservation = JsonConvert.DeserializeObject<StoreItem>(apiResponse);
            }
            return View(recievedReservation);
        }

        public async Task<IActionResult> UpdateStoreItemPatch(int id)
        {
            StoreItem storeItem = new StoreItem();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                storeItem = JsonConvert.DeserializeObject<StoreItem>(apiResponse);
            }
            return View(storeItem);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStoreItemPatch(int id, StoreItem storeItem)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id),
                    Method = new HttpMethod("Patch"),
                    Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"itemName\", \"value\": \"" + storeItem.ItemName + "\"},{ \"op\": \"replace\", \"path\": \"cost\", \"value\": \"" + storeItem.Cost + "\"}]", Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStoreItem(int StoreItemId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + StoreItemId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteStore(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://webstoreendpoints20201024213721.azurewebsites.net/api/webstore/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

    }
}

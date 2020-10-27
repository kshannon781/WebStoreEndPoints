using WebStoreEndPoints.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebStoreEndPoints.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebStoreController : ControllerBase
    {
        private IRepository repository;

        private IWebHostEnvironment webHostEnvironment;

        public WebStoreController(IRepository repo, IWebHostEnvironment environment)
        {
            repository = repo;
            webHostEnvironment = environment;
        }

        [HttpGet("StoreItem.{format}"), FormatFilter]
        public IEnumerable<StoreItem> StoreItems() => repository.StoreItem;
        
      
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<StoreItem> Get()
        {
            return repository.StoreItem;
        }//pull back all records


        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{name}")]
        public string GetMaxPrice(string name) => repository.GetMax(name);

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public List<string>GetListItem() => repository.GetListItems();

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public IEnumerable<StoreItemTemp> GetMaxGroup() => repository.GetMaxGroup();

        [HttpGet("{id}")]
        public StoreItem Get(int id) => repository[id]; //get by single id 

        //[HttpPost]
        //public IActionResult Post([FromBody] StoreItem res)
        //{
        //    return Ok(repository.AddStoreItem(new StoreItem
        //    {
        //        ItemName = res.ItemName,
        //        Cost = res.Cost,
        //    }));
        //}
        [HttpPost]
        public StoreItem Post([FromBody] StoreItem res) =>
         repository.AddStoreItem(new StoreItem
         {
             ItemName = res.ItemName,
             Cost = res.Cost
         });

        [HttpPut]
        public StoreItem Put([FromForm] StoreItem res) => repository.UpdateStoreItem(res);

        [HttpPut("{id}")]
        public StoreItem PutID(int id, [FromForm] StoreItem res) => repository.UpdateStoreItem(res);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<StoreItem> patch)
        {
            StoreItem res = Get(id);
            if(res != null)
            {
                patch.ApplyTo(res);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteStoreItem(id);

       
    }
}

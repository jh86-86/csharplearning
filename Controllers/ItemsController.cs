using System;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;


namespace Catalog.Controllers
{
    [ApiController]//bring in default behaviours for controller class
    [Route("items")] //defines http route or [controller]
    public class ItemContoller: ControllerBase
    {
        private readonly InMemItemsRespository repository; //not ideal

        public ItemContoller()
        {
            repository = new InMemItemsRespository();
        }


        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items = repository.GetItems();
            return items;
        }

        [HttpGet("{id}")]
        
            public ActionResult<Item> GetItem(Guid id)   //actionresult allows us to return more than one type
            {
                    var item= repository.GetItem(id);

                    if(item is null)
                    {
                        return NotFound(); 
                    }
                    return Ok(item); //return item in an okay status code
            }
            
        }
 }
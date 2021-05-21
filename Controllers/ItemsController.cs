using System;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;
using System.Linq;
using Catalog.Dtos;

namespace Catalog.Controllers
{
    [ApiController]//bring in default behaviours for controller class
    [Route("items")] //defines http route or [controller]
    public class ItemContoller: ControllerBase
    {
        private readonly IItemsRespository repository; // using dependency inversion with interface and dependcy injection

        public ItemContoller(IItemsRespository respository)
        {
            this.repository = respository;
        }


        [HttpGet]
        public IEnumerable<ItemDto> GetItems() //itemsDto is setting up that contract
        {
            var items = repository.GetItems().Select( item => item.AsDto()); //use the extension method and brings item back
            return items;
        }

        [HttpGet("{id}")]
        
            public ActionResult<ItemDto> GetItem(Guid id)   //actionresult allows us to return more than one type
            {
                    var item= repository.GetItem(id);

                    if(item is null)
                    {
                        return NotFound(); 
                    }
                    return Ok(item.AsDto()); //return item in an okay status code
            }
            
        }
 }
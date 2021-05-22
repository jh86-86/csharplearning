using System;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;
using System.Linq;
using Catalog.Dtos;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<ItemDto>> GetItemsAsync() //itemsDto is setting up that contract
        {
            //the parenthesis below added around async, tells to complete this task first then do the select after it has completed
            var items = (await repository.GetItemsAsync()).
                        Select( item => item.AsDto()); //use the extension method and brings item back
            return items;
        }

        [HttpGet("{id}")]
        
            public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)   //actionresult allows us to return more than one type
            {
                    var item= await repository.GetItemAsync(id);

                    if(item is null)
                    {
                        return NotFound(); 
                    }
                    return Ok(item.AsDto()); //return item in an okay status code
            }

        [HttpPost] //post/items

        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto) 
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name= itemDto.Name,
                Price = itemDto.Price,
                CreateDate= DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new{id= item.Id}, item.AsDto());
            //created at returns 201 response , gets name of item and id,  and return ItemDto
            
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem= await repository.GetItemAsync(id);

            if(existingItem is null)
            {
                return NotFound("Are you passing correct id?");
            }

            Item updatedItem = existingItem with //comes from record types, take exisitng item,makes copy with properties modified
            {
                Name= itemDto.Name,
                Price= itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent(); //convention to return no content

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem= await repository.GetItemAsync(id);

            if(existingItem is null)
            {
                return NotFound("Are you passing correct id?");
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }
            
        }
 }
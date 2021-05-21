using System;
using Catalog.Dtos;
using Catalog.Entities;

namespace Catalog
{
    //for extension methods you have to use public static class
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item) //this retuns item Dto
        {
            return new ItemDto
            {
                Id= item.Id,
                Name = item.Name,
                Price= item.Price,
                CreateDate = item.CreateDate
            };
        }
    }
}
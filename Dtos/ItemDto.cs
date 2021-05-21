using System;
using Catalog.Entities;

//this is where our contracts are

namespace Catalog.Dtos
{
    public record ItemDto
    {
        public Guid Id {get; init;}
        public string Name {get; init;}

        public decimal Price {get; init; }

        public DateTimeOffset CreateDate { get; init; }
    }
}
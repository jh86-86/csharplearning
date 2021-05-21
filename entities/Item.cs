using System;

namespace Catalog.Entities
{
    public record Item 
    {
        public Guid Id { get; init;}   //init-only properties, 
        public string Name {get; init;}

        public decimal Price {get; init; }
        public DateTimeOffset CreateDate {get; init;} 

    }
    
}

/*record types: 
-use for immutable objects
-with-expressions support
-value-based equality support
*/
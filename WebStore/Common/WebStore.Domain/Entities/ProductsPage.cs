﻿using System.Collections.Generic;

namespace WebStore.Domain.Entities
{
    public record ProductsPage(IEnumerable<Product> Products, int totalCount);    
}

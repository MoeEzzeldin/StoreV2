using System;
using System.Collections.Generic;

namespace Store.Models.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string Brand { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Price { get; set; }

    public string PictureUrl { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}

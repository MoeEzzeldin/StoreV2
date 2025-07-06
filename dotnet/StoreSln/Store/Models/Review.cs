using System;
using System.Collections.Generic;

namespace Store.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public string Reviewer { get; set; } = null!;

    public int Rating { get; set; }

    public string Title { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

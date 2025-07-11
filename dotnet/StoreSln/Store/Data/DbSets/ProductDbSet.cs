using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Data.DbSets
{
    public class ProductDbSet
    {
        public virtual DbSet<Product> Products { get; set; }
        public ProductDbSet(DbContext context)
        {
            Products = context.Set<Product>();
        }
    }
}

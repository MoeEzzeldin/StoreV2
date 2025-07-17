using Microsoft.EntityFrameworkCore;
using Store.Models.Entities;

namespace Store.Data.DbSets
{
    public class ReviewDbSet
    {
        public virtual DbSet<Review> Reviews { get; set; }

        public ReviewDbSet(DbContext context)
        {
            Reviews = context.Set<Review>();
        }
    }
}

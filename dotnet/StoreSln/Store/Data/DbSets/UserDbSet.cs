using Microsoft.EntityFrameworkCore;
using Store.Models.Entities;

namespace Store.Data.DbSets
{
    public class UserDbSet
    {
        public virtual DbSet<User> Users { get; set; }
        public UserDbSet(DbContext context)
        {
            Users = context.Set<User>();
        }
    }
}

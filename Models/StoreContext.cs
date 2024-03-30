using Microsoft.EntityFrameworkCore;

namespace MVCExam.Models
{
    public class StoreContext: DbContext
    {

        public DbSet<Goods> Goods { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }

        public StoreContext(DbContextOptions opt) : base(opt)
        {

        }





    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCExam.Models
{
    public class BillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        [Required]
        public int? GoodsId { get; set; }

        //public string ItemName { get; set; }
        //[Required]
        //public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal ItemTotal => this.Goods is null ? 0 : this.Goods.Price * this.Quantity;


        public int? BillId { get; set; }

        public Goods? Goods { get; set; }
        public Bill? Bill { get; set; }









    }
}

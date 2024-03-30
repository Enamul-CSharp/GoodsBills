using System.ComponentModel.DataAnnotations.Schema;

namespace MVCExam.Models
{
    public class Goods
    {


        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string? GoodsImage { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

        public IList<BillItem> Items { get; set; } = new List<BillItem>();




    }
}

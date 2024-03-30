using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCExam.Models
{
    public class Bill
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BillDate { get; set; } = DateTime.Now;

        [Required]

        public string CustomerName { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }

        public List<BillItem> Items { get; set; } = new List<BillItem>();








    }
}

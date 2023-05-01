using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApp.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }

        public string Name { get; set; }

        public int Cid { get; set; }
        
        [ForeignKey("Cid")]
        public virtual Category category { get; set; }

    }
}

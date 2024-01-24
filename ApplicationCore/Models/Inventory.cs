using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public Guid? ProductId { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm tt MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAcquired { get; set; }
        public decimal ResalePrice { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

    }
}

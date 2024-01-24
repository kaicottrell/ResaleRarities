using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm tt MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CompletionDateTime { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        [ForeignKey("OrderStatusId")]
        public virtual OrderStatus? OrderStatus { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StatusDescription { get; set; } = string.Empty;
    }
}

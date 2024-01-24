using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int ConditionId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [ForeignKey("ConditionId")]
        public virtual Condition? Condition { get; set; }

    }
}

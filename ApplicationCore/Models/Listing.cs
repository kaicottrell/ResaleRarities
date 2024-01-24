using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Listing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public int ListingStatusId { get; set; }
       
     //   [Required]
      //  public string ApplicationUserId { get; set; } = string.Empty;

        public Guid? ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh:mm tt MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTimePosted { get; set; }
        public decimal? Compensation { get; set; }
      //  [ForeignKey("ApplicationUserId")]
      //  public virtual ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("ListingStatusId")]
        public virtual ListingStatus? ListingStatus { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Models
{
	public class ApplicationUser // registration
    {
		[Key]
		[Required]
		public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;

		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

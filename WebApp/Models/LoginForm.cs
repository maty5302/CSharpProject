using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApp.Models
{
	public class LoginForm
	{
		public int id { get; set; }
        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno je povinné")]
        public string? FirstName { get; set; }
		[Display(Name = "Příjmení")]
		[Required(ErrorMessage = "Příjmení je povinné")]
		public string LastName { get; set; }
		[Display(Name = "Email")]
		[Required(ErrorMessage = "Email je povinný")]
		[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email není ve správném formátu")]
		public string Email { get; set; }
		[Display(Name = "Heslo")]
		[Required(ErrorMessage = "Heslo je povinné")]
		public string Password { get; set; }
		[Display(Name = "Heslo znovu")]
		[Required(ErrorMessage = "Heslo je povinné")]
		[Compare("Password", ErrorMessage = "Hesla se neshodují")]
		public string PasswordAgain { get; set; }


	}
}

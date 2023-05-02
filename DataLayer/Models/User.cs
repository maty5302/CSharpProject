using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public bool isAdmin { get; set; }

		public User() { }

		public User(string name, string surname, string email, string password, bool isAdmin)
		{
			Name = name;
			Surname = surname;
			Email = email;
			Password = password;
			this.isAdmin = isAdmin;
		}
	}
}

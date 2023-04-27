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
		public string Phone { get; set; }
		public bool isAdmin { get; set; }

		public User(string name, string surname, string email, string password, string phone, bool isAdmin)
		{
			Name = name;
			Surname = surname;
			Email = email;
			Password = password;
			Phone = phone;
			this.isAdmin = isAdmin;
		}
	}
}

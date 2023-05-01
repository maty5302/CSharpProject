using DataLayer.Models;

namespace WebApp
{
	public class LoginManager
	{
		private static LoginManager? instance;
		public User? User = null; 
		public static LoginManager Get
		{
			get
			{
				if (instance is null)
				{
					instance = new LoginManager();
				}
				return instance;
			}
		}
	}
}

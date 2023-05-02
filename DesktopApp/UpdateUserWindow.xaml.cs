using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopApp
{
	public delegate void UpdateUser(User u);
	/// <summary>
	/// Interakční logika pro UpdateUserWindow.xaml
	/// </summary>
	public partial class UpdateUserWindow : Window
	{
		private User user { get; set; }

		public event UpdateUser OnUpdate;

		public UpdateUserWindow(User u)
		{
			InitializeComponent();
			user = u;
			this.DataContext = this.user;
		}

		private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (name.Text == "" || surname.Text == "" || email.Text == "" || heslo.Text == "")
			{
				MessageBox.Show("Vyplň všechny údaje", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
			bool isValid = Regex.IsMatch(email.Text, pattern);
			if(!isValid)
			{
				MessageBox.Show("Zadej platný email", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			this.OnUpdate?.Invoke(user);
			this.Close();
		}
	}
}

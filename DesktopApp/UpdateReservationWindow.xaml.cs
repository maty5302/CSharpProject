using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public delegate void UpdateReservation(TableReservation r);
	/// <summary>
	/// Interakční logika pro UpdateReservationWindow.xaml
	/// </summary>
	public partial class UpdateReservationWindow : Window
	{
		private TableReservation reservation { get; set; }

		public event UpdateReservation OnUpdate;

		public UpdateReservationWindow(TableReservation r)
		{
			InitializeComponent();
			reservation = r;
			this.DataContext = reservation;
		}

		private void UpdateReservationButton_Click(object sender, RoutedEventArgs e)
		{
			if(reservation.NumberOfPeople > reservation.table.NumberOfSeats)
			{
				MessageBox.Show("Počet lidí je vyšší než je počet míst u stolu", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if(reservation.ReservationTime < DateTime.Now)
			{
				MessageBox.Show("Rezervace nemůže být v minulosti.", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if (reserve_times.Text == "")
			{
				MessageBox.Show("Vyber čas rezervace", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			if(people.Text == "")
			{
				MessageBox.Show("Vyber počet lidí ", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			try
			{
				if (Convert.ToInt32(people.Text) < 0 && Convert.ToInt32(people.Text) > 20)
				{
					MessageBox.Show("Vyber platný počet lidí", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
				return;
			}
			string d = date.Text;
			string t = reserve_times.Text;
			DateTime dt = DateTime.Parse(d + " " + t);
			reservation.ReservationTime = dt;
			this.OnUpdate?.Invoke(this.reservation);
			this.Close();
		}
	}
}

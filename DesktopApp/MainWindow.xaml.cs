using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<TableReservation> Reservations {get;set;}
		public List<RTable> Tables { get;set;}
		public List<User> Users { get; set; }
		private Thread t;

		public async Task UpdateTable()
		{

			Reservations.Clear();
			Reservations = DB.SelectAll<TableReservation>();
			Tables.Clear();
			Tables = DB.SelectAll<RTable>();
			Users.Clear();
			Users = DB.SelectAll<User>();
			if(Application.Current == null)
				return;
			await Application.Current.Dispatcher.InvokeAsync(() =>
			{
				// Update the DataGrid with the loaded data
				reservations.ItemsSource = Reservations;
				tables.ItemsSource = Tables;
				users.ItemsSource = Users;
			});
		}		

		public MainWindow()
		{
			InitializeComponent();
			DB.CheckIfDbExists();
			Reservations = DB.SelectAll<TableReservation>();
			Tables = DB.SelectAll<RTable>();
			Users = DB.SelectAll<User>();
			reservations.DataContext = Reservations;	
			tables.DataContext = Tables;
			users.DataContext = Users;

			t = new Thread(async () =>
			{
				while (true)
				{
					await UpdateTable();
					Thread.Sleep(1000);
				}
			});

			t.Start();
		}

		private void delete_reservation(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			TableReservation reservation = btn.DataContext as TableReservation;
			DB.Delete(reservation);
		}

		private void delete_table(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			RTable table = btn.DataContext as RTable;
			try
			{
				DB.Delete(table);
			}
			catch (Exception)
			{
				MessageBox.Show("Nelze smazat stůl, který je obsazený.", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
		}

		private void update_reservation(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			TableReservation reservation = btn.DataContext as TableReservation;
			//make copy of reservation and pass it to the window
			TableReservation copy = new TableReservation(reservation.table, reservation.user, reservation.ReservationTime, reservation.NumberOfPeople);
			UpdateReservationWindow window = new UpdateReservationWindow(copy);
			window.OnUpdate += (r) =>
			{
				reservation.ReservationTime = r.ReservationTime;
				reservation.NumberOfPeople = r.NumberOfPeople;
				DB.Update(reservation);

			};
			window.ShowDialog();
		}

		private void detail_reservation(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			TableReservation reservation = btn.DataContext as TableReservation;
			DetailReservationWindow window = new DetailReservationWindow();
			window.DataContext = reservation;
			window.ShowDialog();
		}

		private void update_table(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			RTable table = btn.DataContext as RTable;
			RTable copy = new RTable(table.NumberOfSeats);
			copy.Id = table.Id;
			UpdateTableWindow window = new UpdateTableWindow(copy);

			window.OnUpdate += (r) =>
			{
				table.NumberOfSeats = r.NumberOfSeats;
				DB.Update(table);
			};
			window.ShowDialog();
			
		}

		private void create_table(object sender, RoutedEventArgs e)
		{
			CreateTableWindow createTableWindow = new CreateTableWindow();
			createTableWindow.OnCreate += (r) =>
			{
				DB.Insert(r);
			};
			createTableWindow.ShowDialog();
		}

		private void Delete_User(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			User u = b.DataContext as User;
			try
			{
				DB.Delete(u);
			}
			catch (Exception)
			{
				MessageBox.Show("Nelze smazat uživatele, který má rezervace.","Pozor!",MessageBoxButton.OK,MessageBoxImage.Warning);
			}

		}

		private void Update_User(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			User u = b.DataContext as User;
			User copy = new User(u.Name, u.Surname, u.Email,u.Password, u.isAdmin);
			copy.Id = u.Id;
			UpdateUserWindow window = new UpdateUserWindow(copy);
			window.OnUpdate += (r) =>
			{
				u.Name = r.Name;
				u.Surname = r.Surname;
				u.Email = r.Email;
				u.Password = r.Password;
				u.isAdmin = r.isAdmin;
				DB.Update(u);
			};
			window.ShowDialog();
		}
	}
}

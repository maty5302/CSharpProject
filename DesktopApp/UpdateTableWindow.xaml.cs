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
	public delegate void UpdateTableDelegate(RTable r);
	/// <summary>
	/// Interakční logika pro UpdateTableWindow.xaml
	/// </summary>
	public partial class UpdateTableWindow : Window
	{
		private RTable table { get; set; }

		public event UpdateTableDelegate OnUpdate;

		public UpdateTableWindow(RTable r)
		{
			InitializeComponent();
			table = r;
			this.DataContext = table;
		}

		private void UpdateTableButton_Click(object sender, RoutedEventArgs e)
		{
			if (people.Text == "")
			{
				MessageBox.Show("Select number of people");
				return;
			}
			try
			{
				if (Convert.ToInt32(people.Text) < 0 && Convert.ToInt32(people.Text) > 20)
				{
					MessageBox.Show("Select valid number of people");
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			this.OnUpdate?.Invoke(table);
			this.Close();
		}
	}
}

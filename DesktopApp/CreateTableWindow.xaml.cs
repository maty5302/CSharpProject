﻿using DataLayer.Models;
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
	public delegate void CreateTable(RTable r);
	/// <summary>
	/// Interakční logika pro CreateTableWindow.xaml
	/// </summary>
	public partial class CreateTableWindow : Window
	{
		public RTable table { get; set; } = new RTable();
		public event CreateTable OnCreate;
		
		public CreateTableWindow()
		{
			InitializeComponent();
			this.DataContext = this.table;
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			if (people.Text == "")
			{
				MessageBox.Show("Vyber počet míst u stolu", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			try
			{
				if (Convert.ToInt32(people.Text) < 0 && Convert.ToInt32(people.Text) > 20)
				{
					MessageBox.Show("Vyber platný počet míst u stolu", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			this.OnCreate?.Invoke(this.table);
			this.Close();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class RTable
	{
		public int Id { get; set; }
		public int NumberOfSeats { get; set; }

		public RTable() { }

		public RTable(int NumberOfSeats)
		{
			this.NumberOfSeats = NumberOfSeats;
		}
	}
}

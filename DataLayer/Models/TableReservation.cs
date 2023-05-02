using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class TableReservation
	{
		public int Id { get; set; }
		public RTable table { get; set; }
		public User user { get; set; }
		public DateTime ReservationTime { get; set; }
		public int NumberOfPeople { get; set; }

		public TableReservation() { }

		public TableReservation(RTable table, User user, DateTime reservationTime, int numberOfPeople, bool isReserved)
		{
			this.table = table;
			this.user = user;
			ReservationTime = reservationTime;
			NumberOfPeople = numberOfPeople;
		}
	}
}

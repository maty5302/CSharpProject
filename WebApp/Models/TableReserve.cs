using DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class TableReserve
	{
		public int TableId { get; set; }
		public int ReserveId { get; set; }
		public int NumberOfPeople { get; set; }
		public DateTime Time { get; set; }

	}
}

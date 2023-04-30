
using DataLayer;
using DataLayer.Models;

namespace idktestorneco
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Testing");
			DB db = new DB();
			DB.CreateTable<User>();
			DB.CreateTable<TableReservation>();
			DB.CreateTable<RTable>();
			Console.WriteLine(DB.solutionDirectory);
			//User u = new User("Matěj", "Lomený", "ml@vsb.cz", "uaC++nebuh", "745452545", false);
			//DB.Insert(u);

			//RTable t = new RTable(6);
			//DB.Insert(t);
			//u.Name = "Martin";

			//TableReservation reserve = new TableReservation(t, u, DateTime.Now.AddHours(2), 8, true);
			//DB.Insert(reserve);

			////DB.Update(u);
			////DB.Delete(u);
			////u = DB.SelectById<User>(1);
			////Console.WriteLine(u.Name + " " + u.Surname);
			////List<TableReservation> a = DB.SelectAll<TableReservation>();

			//foreach (var item in a)
			//{

			//	Console.WriteLine("Id stolu:" + item.table.Id);
			//	Console.WriteLine("Id uživatele: " + item.user.Id);
			//	Console.WriteLine("Počet míst:" + item.table.NumberOfSeats);
			//	Console.WriteLine("Jmeno + prijmeni" + item.user.Name + " " + item.user.Surname);

			//}
			TableReservation a = DB.SelectById<TableReservation>(1);
			a.ReservationTime = DateTime.Now;
			DB.Update(a);

			TableReservation aa = DB.SelectById<TableReservation>(1);
			Console.WriteLine("Id stolu:" + aa.table.Id);
			Console.WriteLine("Id uživatele: " + aa.user.Id);
			Console.WriteLine("Počet míst:" + aa.table.NumberOfSeats);
			Console.WriteLine("Jmeno + prijmeni" + aa.user.Name + " " + aa.user.Surname);


		}
	}
}
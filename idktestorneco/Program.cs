
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
			//User u = new User("Matěj","Lomený","ml@vsb.cz","uaC++nebuh","745452545",false);
			//DB.Insert(u);

			//RTable t = new RTable(6);
			//DB.Insert(t);
			//u.Name = "Martin";

			//TableReservation reserve = new TableReservation(t,u,DateTime.Now.AddHours(2),8,true);
			//DB.Insert(reserve);

			//DB.Update(u);
			//DB.Delete(u);
			User u = DB.SelectById<User>(1);
            Console.WriteLine(u.Name + " "+u.Surname);
            List<TableReservation> a = DB.SelectAll<TableReservation>();

			foreach (var item in a)
			{

            }			

        }
	}
}
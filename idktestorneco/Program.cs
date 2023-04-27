
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
			User u = new User("Matěj","Pipik","mp@vsb.cz","uaC#buh","745879545",true);
			DB.Insert(u);

			RTable t = new RTable(4);
			DB.Insert(t);
			
			

        }
	}
}
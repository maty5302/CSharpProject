
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
        }
	}
}
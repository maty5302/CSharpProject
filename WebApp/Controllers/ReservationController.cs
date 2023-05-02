using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Models;

namespace WebApp.Controllers
{
	//[Authorize]
	public class ReservationController : Controller
	{

		public IActionResult Index()
		{
			List<RTable> tables = DB.SelectAll<RTable>();
            ViewBag.Tables = tables;
			return View();
		}

		public IActionResult Reserve(TableReserve reserve)
		{
			List<RTable> tables = DB.SelectAll<RTable>();
			ViewBag.Tables = tables;
			RTable table = DB.SelectById<RTable>(reserve.TableId);
			List<TableReservation>? r = DB.SelectBy<TableReservation>("tableId", reserve.TableId.ToString());
			if(table is null)
			{
				ModelState.AddModelError("TableId", "Stůl neexistuje");
				return View("Index");
			}
			if(reserve.NumberOfPeople > table.NumberOfSeats)
			{
				ModelState.AddModelError("NumberOfPeople", "Stůl nemá dostatečnou kapacitu");
				return View("Index");
			}
			if(reserve.NumberOfPeople < 1)
			{
				ModelState.AddModelError("NumberOfPeople", "Počet lidí musí být větší než 0");
				return View("Index");
			}
			if(reserve.Time.Hour < 10 || reserve.Time.Hour > 22)
			{
				ModelState.AddModelError("Time", "Nelze rezervovat mimo otevírací dobu");
				return View("Index");
			}

			if(reserve.Time < DateTime.Now)
			{
				ModelState.AddModelError("Time", "Nelze rezervovat v minulosti");
				return View("Index");
			}
			
			if(r!=null)
			{
				foreach (var item in r)
				{
					if(item.ReservationTime==reserve.Time)
					{
						ModelState.AddModelError("Time", "Stůl je v této době již rezervován");
						return View("Index");
					}						
				}
			}
			TableReservation reservation = new TableReservation(table,LoginManager.Get.User,reserve.Time,reserve.NumberOfPeople);
			DB.Insert(reservation);
			return RedirectToAction("UserReservations","Reservation");
		}

		public IActionResult UserReservations()
		{
			List<TableReservation> reserve = DB.SelectBy<TableReservation>("userId", LoginManager.Get.User.Id.ToString());
			ViewBag.Reservations = reserve;
			return View();
		}

		public IActionResult CancelReservation(TableReserve t)
		{
			TableReservation reservation = DB.SelectById<TableReservation>(t.ReserveId);
			if(reservation is null)
			{
				return RedirectToAction("UserReservations");
			}
			if(reservation.user.Id != LoginManager.Get.User.Id)
			{
				return RedirectToAction("UserReservations");
			}
			DB.Delete(reservation);
			return RedirectToAction("UserReservations");
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if(LoginManager.Get.User is null)
			{
				context.Result = new RedirectResult("/Login");
				return;
			}
			base.OnActionExecuting(context);
		}
	}
}

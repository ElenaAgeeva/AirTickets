using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookTickets.Models;

namespace BookTickets.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Route> Listroutes = new List<Route>();
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                Listroutes = db.GetEntityList<Route>();
            }
            return View(Listroutes);
        }

        public ActionResult BuyFromIndex(int routeId)
        {
            Session["RouteID"] = routeId;
            return RedirectToAction("TryBuy");
        }

        public ActionResult BookFromIndex(int routeId)
        {
            Session["RouteID"] = routeId;
            return RedirectToAction("TryBook");
        }

        [HttpGet]
        public ActionResult TryBuy()
        {
            if (Session["LogInUserPassword"] != null && Session["LogInUserName"] != null)
            {
                return RedirectToAction("Buy");
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpPost]
        public ActionResult TryBuy(LogInPerson logperson)
        {
            if (check(logperson.LogName, logperson.Password.GetHashCode().ToString()))
            {
                Session["LogInUserPassword"] = logperson.Password.GetHashCode().ToString();
                Session["LogInUserName"] = logperson.LogName;
                return RedirectToAction("Buy");
                
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpGet]
        public ActionResult Buy()
        {
            Ticket ticket = new Ticket();
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                string s = Session["RouteID"].ToString();
                if (s != null)
                {
                    int r = Convert.ToInt32(s);
                    var rout = db.GetEntityById<Route, int>(r);
                    ticket.Route = rout;
                }
            }
            return View("Buy", ticket);
        }

        [HttpPost]
        public ActionResult Buy(Ticket tick)
        {
            if (tick.NumberOfPlace != 0)
            {
                using (WorkWithDatabase db = new WorkWithDatabase())
                {
                    tick.Condition = "bought";
                    string s1 = Session["LogInUserName"].ToString();
                    string s2 = Session["LogInUserPassword"].ToString();
                    var per = db.GetEntityList<Person>().Where(x => x.LogName.Equals(s1) && x.Password.Equals(s2)).FirstOrDefault();
                    var route = db.GetEntityById<Route, int>(tick.Route.RouteID);
                    tick.Person = per;
                    tick.Route = route;
                    db.Insert<Ticket>(tick);
                    db.Commit();
                }
                return RedirectToAction("TicketInformation");
            }
            else
            {
                return View();
            }
        }

        public ActionResult TicketInformation()
        {
            List<Ticket> tickets = new List<Ticket>();
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                string s1 = Session["LogInUserName"].ToString();
                string s2 = Session["LogInUserPassword"].ToString();
                var t = db.GetEntityList<Person>().Where(x => x.LogName.Equals(s1) && x.Password.Equals(s2)).Select(x => x.PersonID).FirstOrDefault();

                tickets = db.GetEntityList<Ticket>().Where(x => x.Person.PersonID.Equals(t)).ToList();
            }
            return View("InformationAboutTicket", tickets);
        }

        private bool check(string name, string pass)
        {
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                try
                {
                    return db.GetEntityList<Person>().Where(x => x.LogName.Equals(name) && x.Password.Equals(pass)).Count() > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        [HttpGet]
        public ActionResult TryBook()
        {
            if (Session["LogInUserPassword"] != null && Session["LogInUserName"] != null)
            {
                return RedirectToAction("Book");
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpPost]
        public ActionResult TryBook(LogInPerson logperson)
        {
            if (check(logperson.LogName, logperson.Password.GetHashCode().ToString()))
            {
                Session["LogInUserPassword"] = logperson.Password.GetHashCode().ToString();
                Session["LogInUserName"] = logperson.LogName;
                return RedirectToAction("Book");

            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpGet]
        public ActionResult Book()
        {
            Ticket ticket = new Ticket();
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                string s = Session["RouteID"].ToString();
                if (s != null)
                {
                    int r = Convert.ToInt32(s);
                    var rout = db.GetEntityById<Route, int>(r);
                    ticket.Route = rout;
                }
            }
            return View("Book",ticket);
        }

        [HttpPost]
        public ActionResult Book(Ticket tick)
        {
            if (tick.NumberOfPlace != 0)
            {
                List<Ticket> tickets = new List<Ticket>();
                using (WorkWithDatabase db = new WorkWithDatabase())
                {
                    tick.Condition = "booked";
                    string s1 = Session["LogInUserName"].ToString();
                    string s2 = Session["LogInUserPassword"].ToString();
                    var per = db.GetEntityList<Person>().Where(x => x.LogName.Equals(s1) && x.Password.Equals(s2)).FirstOrDefault();
                    tick.Person = per ;
                    db.Insert<Ticket>(tick);
                    db.Commit();
                }
                return RedirectToAction("TicketInformation");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ViewResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Registration(Person regPerson)
        {
            if (ModelState.IsValid)
            {
                Session["LogInUserPassword"] = regPerson.Password.GetHashCode().ToString();
                Session["LogInUserName"] = regPerson.LogName;
                using (WorkWithDatabase db = new WorkWithDatabase())
                {
                    regPerson.Password = regPerson.Password.GetHashCode().ToString();
                    db.Insert<Person>(regPerson);
                    db.Commit();
                }
                return View("Thanks", regPerson);
            }
            else
            {
                return View();
            }
        }

        public ActionResult CancelBook(int ticketId)
        {
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                Ticket tic = db.GetEntityById<Ticket, int>(ticketId);
                tic.IsDeleted = true;
                db.Commit();
            }
            return RedirectToAction("TicketInformation");
        }

        public ActionResult ReturnTicket(int ticketId)
        {
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                Ticket tic = db.GetEntityById<Ticket, int>(ticketId);
                tic.IsDeleted = true;
                db.Commit();
            }
            return RedirectToAction("TicketInformation");
        }

        /*public JsonResult Ajax()//Метод, который выполяется асинхронно при помощи Ajax, может называться как угодно
        {
            return Json(places, JsonRequestBehavior.AllowGet);//Возвращаем список мест в формате Json
        }*/
    }


    public static class Extentions//Статический класс для создания метода расшиения
    {
        public static int[] GetNumbersOfPlaces(this HtmlHelper helper, int routeId)//Метод, расширяющий хелпер @Html
        {
            List<int> freetickets = new List<int>();
            List<int> busytickets = new List<int>();
            using (WorkWithDatabase db = new WorkWithDatabase())
            {
                int RouteId = routeId;
                var countst = db.GetEntityById<Route, int>(RouteId).MaxNumberOfPlace;
                if (countst > 0)
                {
                    int countOfPlace = Convert.ToInt32(countst);
                    int[] masplace = new int[countOfPlace];
                    busytickets = db.GetEntityList<Ticket>().Where(x => x.Route.Id.Equals(RouteId)).Select(x => x.NumberOfPlace).ToList();
                    for (int i = 1; i <= countOfPlace; i++)
                    {
                        if (!busytickets.Contains(i)) { freetickets.Add(i); }
                    }
                }

            }
            return freetickets.ToArray();
        }
    }
}


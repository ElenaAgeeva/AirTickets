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
            using (UserContext db = new UserContext())
            {
                Listroutes = db.Routes.ToList();
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
                return View("Buy");
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpPost]
        public ActionResult TryBuy(LogInPerson logperson)
        {
            if (check(logperson.Name, logperson.Password))
            {
                Session["LogInUserPassword"] = logperson.Password;
                Session["LogInUserName"] = logperson.Name;
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
            /*Ticket ticket = new Ticket();
            using (UserContext db = new UserContext())
            {
                string s = Session["RouteID"].ToString();
                if (s != null)
                {
                    int r = Convert.ToInt32(s);
                    var rout = (from dbroutes in db.Routes
                                where (dbroutes.RouteID.Equals(r))
                                select dbroutes).FirstOrDefault();
                    ticket.Route = rout;
                }
            }*/
            Ticket ticket = new Ticket() { Route = new Route() { RouteID = Convert.ToInt32(Session["RouteID"]) } };
            return View("Buy", ticket);
        }

        [HttpPost]
        public ActionResult Buy(Ticket tick)
        {
            if (tick.NumberOfPlace != 0)
            {
                using (UserContext db = new UserContext())
                {
                    tick.Condition = "bought";
                    string s1 = Session["LogInUserName"].ToString();
                    string s2 = Session["LogInUserPassword"].ToString();
                    var per = (from dbpersons in db.Persons
                               where (dbpersons.Name.Equals(s1) && dbpersons.Password.Equals(s2))
                             select dbpersons.PersonID).FirstOrDefault();
                    tick.Person = new Person { PersonID = per };
                    //tick.Person.PersonID = per;
                    db.Tickets.Add(tick);//Почему он не добавляется?!!!!!!!!!!
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
            using (UserContext db = new UserContext())
            {
                string s1 = Session["LogInUserName"].ToString();
                string s2 = Session["LogInUserPassword"].ToString();
                var t = (from dbpersons in db.Persons
                         where (dbpersons.Name.Equals(s1)
                         && (dbpersons.Password.Equals(s2)))
                         select dbpersons.PersonID).FirstOrDefault();
                tickets = (from dbtickets in db.Tickets where dbtickets.Person.PersonID.Equals(t) select dbtickets).ToList();
            }
            return View("InformationAboutTicket", tickets);
        }

        private bool check(string name, string pass)
        {
            using (UserContext db=new UserContext())
            {
                return db.Persons.Where(x => x.Name.Equals(name) && x.Password.Equals(pass)).Count() > 0;
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
            if (check(logperson.Name, logperson.Password))
            {
                Session["LogInUserPassword"] = logperson.Password;
                Session["LogInUserName"] = logperson.Name;
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
            Ticket ticket = new Ticket() { Route = new Route() { RouteID = Convert.ToInt32(Session["RouteID"]) } };
            return View("Book",ticket);
        }

        [HttpPost]
        public ActionResult Book(Ticket tick)
        {
            if (tick.NumberOfPlace != 0)
            {
                List<Ticket> tickets = new List<Ticket>();
                //places = places.Where(n => n != tick.NumberOfPlace).ToArray();
                using (UserContext db = new UserContext())
                {
                    tick.Condition = "booked";
                    string s1 = Session["LogInUserName"].ToString();
                    string s2 = Session["LogInUserPassword"].ToString();
                    var per = (from dbpersons in db.Persons
                               where (dbpersons.Name.Equals(s1) && dbpersons.Password.Equals(s2))
                               select dbpersons.PersonID).FirstOrDefault();
                    tick.Person = new Person { PersonID = per };
                    tick.Cost = 165;
                    //tick.Person.PersonID = per;
                    db.Tickets.Add(tick);//Почему он не добавляется?!!!!!!!!!!
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
                Session["LogInUserPassword"] = regPerson.Password;
                Session["LogInUserName"] = regPerson.Name;
                using (UserContext db = new UserContext())
                {
                    db.Persons.Add(regPerson);
                }
                //добавить человека в базу
                return View("Thanks", regPerson);
            }
            else
            {
                return View();
            }
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
            using (UserContext db = new UserContext())
            {
                int RouteId = routeId;
                var countst = (from dbroutes in db.Routes
                               where dbroutes.RouteID.Equals(RouteId)
                               select dbroutes.MaxNumberOfPlace).FirstOrDefault();
                if (countst > 0)
                {
                    int countOfPlace = Convert.ToInt32(countst);
                    int[] masplace = new int[countOfPlace];
                    busytickets = (from dbtickets in db.Tickets
                                   where dbtickets.Route.RouteID.Equals(RouteId)
                                   select dbtickets.NumberOfPlace).ToList();
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


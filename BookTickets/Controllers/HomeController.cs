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
        /*public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u)
        {
            //this action is for handle post (login)
            if (ModelState.IsValid)//this is check validity
            {
                using (MyDatabaseEntities dc=new MyDatabaseEntities())
                {
                    var v = dc.Users.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v!=null)
                    {
                        Session["LogedUserID"] = v.UserID.ToString();
                        Session["LogedUserFullname"] = v.FullName.ToString();
                        return RedirectToAction("AfterLogin");
                    }
                }
            }
            return View(u);
        }

        public ActionResult AfterLogin()
        {
            if (Session["LogedUserID"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }*/

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
            //char[] forsplit={' '};
            //string[] inf=button.Split(forsplit);
            Session["RouteID"] = routeId;
            //if (inf[1].Equals("Buy"))
            //{
                return RedirectToAction("TryBuy");
            //}
            //if (inf[1].Equals("Book"))
            //{
            //    return RedirectToAction("TryBook");
            //}
            //return View("Index");
        }

        public ActionResult BookFromIndex(int routeId)
        {
            //char[] forsplit = { ' ' };
            //string[] inf = button.Split(forsplit);
            Session["RouteID"] = routeId;
            //if (inf[1].Equals("Buy"))
            //{
            //    return RedirectToAction("TryBuy");
            //}
            //if (inf[1].Equals("Book"))
            //{
                return RedirectToAction("TryBook");
            //}
            //return View("Index");
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
            Ticket ticket = new Ticket() { Route = new Route() { RouteID = Convert.ToInt32(Session["RouteID"]) } };
            return View("Buy", ticket);
        }

        [HttpPost]
        public ActionResult Buy(Ticket tick)
        {
            if (tick.NumberOfPlace != 0)
            {
                List<Ticket> tickets = new List<Ticket>();
                //находим кол-во мест в самолете
                using (UserContext db = new UserContext())
                {
                    int countst = (from dbroutes in db.Routes
                                 where dbroutes.RouteID.Equals(Convert.ToInt32(Session["RouteID"].ToString()))
                                 select dbroutes.MaxNumberOfPlace).FirstOrDefault();
                    //int count = countst;
                        
                     var y=   (from dbtickets in db.Tickets
                                 where dbtickets.Route == Session["RouteID"]
                                 select dbtickets.NumberOfPlace);

                }
                //places = places.Where(n => n != tick.NumberOfPlace).ToArray();
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
                var t = (from dbtickets in db.Persons
                         where (dbtickets.Name.Equals(Session["LogInUserName"].ToString())
                         && (dbtickets.Password.Equals(Session["LogInUserPassword"].ToString())))
                         select dbtickets.PersonID).FirstOrDefault();
                tickets = (from dbtickets in db.Tickets where dbtickets.Person.Equals(t) select dbtickets).ToList();
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
            return View("Book");
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
                    var t = (from dbtickets in db.Persons
                             where (dbtickets.Name.Equals(Session["LogInUserName"].ToString())
                             && (dbtickets.Password.Equals(Session["LogInUserPassword"].ToString())))
                             select dbtickets.PersonID).FirstOrDefault();
                    tickets = (from dbtickets in db.Tickets where dbtickets.Person.Equals(t) select dbtickets).ToList();
                }
                return View("InformationAboutTicket", tickets);
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
        public ViewResult RsvpForm(Person regPerson)
        {
            if (ModelState.IsValid)
            {
                Session["LogInUserPassword"] = regPerson.Password;
                Session["LogInUserName"] = regPerson.Name;
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
            //находим кол-во мест в самолете
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
                    /*foreach (int u in busytickets2)
                    {
                        busytickets.Add(u);
                    }
                    int[] p= busytickets2.ToArray();*/

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


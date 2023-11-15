using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.DTO;
using zero_hunger.EF;

namespace zero_hunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDTO c)
        {
            var db = new ZeroHungerEntities();
            var checkLogin = db.Employees.Where(x => x.Email.Equals(c.Email) && x.Password.Equals(c.Password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["EmployeeEmail"] = c.Email.ToString();
                Session["EmployeeName"] = checkLogin.Name.ToString();
                Session["EmployeeId"] = checkLogin.Id.ToString();
                return RedirectToAction("Index", "Employee");
            }
            return View(c);
        }


        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(RestaurantDTO r)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<RestaurantDTO, Restaurant>();
                });
                var mapper = new Mapper(config);
                var data = mapper.Map<Restaurant>(r);
                var db = new ZeroHungerEntities();
                db.Restaurants.Add(data);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(r);
        }
    }
}
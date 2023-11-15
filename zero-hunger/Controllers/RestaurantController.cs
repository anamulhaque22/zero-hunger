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
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            var db = new ZeroHungerEntities();

            if(Session["RestaurantId"] == null)
            {
                return RedirectToAction("Login", "Restaurant");
            }
            else
            {
            var restaurantId = Convert.ToInt32(Session["RestaurantId"]);
                var data = db.CollectRequests
                                .Where(cr => cr.Restaurant_ID == restaurantId)
                                .ToList();
                return View(data);
            }
            
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
            var checkLogin = db.Restaurants.Where(x => x.Email.Equals(c.Email) && x.Password.Equals(c.Password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["RestaurantEmail"] = c.Email.ToString();
                Session["RestaurantName"] = checkLogin.Name.ToString();
                Session["RestaurantId"] = checkLogin.Id.ToString();
                return RedirectToAction("Index", "Restaurant");
            }
            return View(c);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RestaurantDTO r) {
            if(ModelState.IsValid)
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

        [HttpGet]
        public ActionResult CreateCollectRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCollectRequest(CollectRequestDTO cr)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                var restaurant = db.Restaurants.SingleOrDefault(c => c.Id == cr.Restaurant_ID);

                var collectRequest = new CollectRequest
                {
                    FoodItem = cr.FoodItem,
                    PreservedTime = cr.PreservedTime,
                    Restaurant = restaurant,
                    RequestedTime = DateTime.Now,
                    Status = "Pending",
                    AssignBy= null,
                    AssignEmployee= null,
                };
                db.CollectRequests.Add(collectRequest);
                db.SaveChanges();
                return RedirectToAction("Index", "Restaurant");
            }
            return View(cr);
        }

        [HttpGet]
        public ActionResult EditCreateCollectRequest(int id) { 
            var db = new ZeroHungerEntities();
            var data = db.CollectRequests.ToList();
            var dbCollectRequestItem = (from p in db.CollectRequests
                             where p.Id == id
                             select p).SingleOrDefault();
            return View(dbCollectRequestItem);
        }

        [HttpPost]
        public ActionResult EditCreateCollectRequest(CollectRequestDTO cr)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                var dbCollectRequest = db.CollectRequests.Find(cr.Id);

             
                    dbCollectRequest.FoodItem = cr.FoodItem;
                    dbCollectRequest.PreservedTime = cr.PreservedTime;
                    dbCollectRequest.RequestedTime = DateTime.Now;

                
                db.SaveChanges();
                return RedirectToAction("Index", "Restaurant");
            }
            return View(cr);
        }

    }
}
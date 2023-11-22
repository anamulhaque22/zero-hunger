using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.Auth;
using zero_hunger.DTO;
using zero_hunger.EF;
using zero_hunger.EF.Models;

namespace zero_hunger.Controllers
{
    public class EmployeeController : Controller
    {
        // collect request data
        public ActionResult Index()
        {
            var db = new ZeroHungerContext();
            var collectRequestList = db.CollectRequests
                .Where(cr => cr.Status == "Pending" || cr.Status == "Approved")
                .ToList();
            return View(collectRequestList);
        }

        //shwoing collect food data
        public ActionResult CollectedFood()
        {
            var db = new ZeroHungerContext();

            if (Session["EmployeeId"] != null)
            {
                string sessionRole = Session["EmployeeId"] as string;
                if (sessionRole == UserRole.Regular.ToString())
                {
                    int userId = Convert.ToInt16(sessionRole);
                    var collectRequestListR = db.CollectRequests
                                                    .Where(cr => cr.Status == "Collected" && cr.AssignedEmployeeId == userId)
                                                    .ToList();
                    return View(collectRequestListR);
                }
                else
                {
                    var collectRequestList = db.CollectRequests
                                    .Where(cr => cr.Status == "Collected")
                                    .ToList();
                    return View(collectRequestList);
                }
            }
            return RedirectToAction("Login", "Employee");
        }

        //collect request where employee is assigned to collect the food
        public ActionResult AssigndEmployeeCollReqList()
        {
            var db = new ZeroHungerContext();
            int loggedInEmp = Convert.ToInt16(Session["EmployeeId"]);
            var dbCollectReqList = db.CollectRequests
                .Where(cr => cr.AssignedEmployeeId == loggedInEmp && cr.Status == "Approved")
                .ToList();
            return View(dbCollectReqList);
        }

        [EmployeeAuth]
        // action to chage the status that the food is collected
        public ActionResult ChangeStatusToCollected()
        {
            var db = new ZeroHungerContext();
            int collectReqId = Convert.ToInt16(Request.Form["Id"]);
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            else
            {
                var dbCollectReq = db.CollectRequests.Find(collectReqId);
                dbCollectReq.Status = "Collected";
                db.SaveChanges();
                return RedirectToAction("CollectedFood", "Employee");
            }
        }

        // assigning employee to distribute the food item

        [HttpGet]
        public ActionResult AssignEmpForDist(int id)
        {
            var db = new ZeroHungerContext();
            List<Employee> employees = db.Employees.ToList();
            var collectReq = db.CollectRequests
                .Where(cr => cr.Id == id && cr.Status == "Collected")
                .FirstOrDefault();
            ViewBag.EmployeeList = employees;
            ViewBag.CollectedFood = collectReq;
            ViewBag.Notification = null;
            return View();
        }

        [HttpPost]
        public ActionResult AssignEmpForDist(Distribution d)
        {
            var db = new ZeroHungerContext();
            var collectReq = db.CollectRequests.Find(d.CollectRequestId);
            var assignEmp = db.Employees.Find(d.DistributedById);

            if (collectReq == null || assignEmp == null)
            {
                ViewBag.Notification = "Food item is no found!";
                return View(d);
            }
            collectReq.Status = "Delivered";
            Distribution newDistributin = new Distribution
            {
                CollectRequest = collectReq,
                DistributedBy = assignEmp,
                Area = d.Area,
            };
            db.Distributions.Add(newDistributin);
            db.SaveChanges();
            return RedirectToAction("DeliveredFoodList", "Employee");
        }

        public ActionResult DeliveredFoodList()
        {
            var db = new ZeroHungerContext();

            if (Session["EmployeeId"] != null)
            {
                string sessionRole = Session["EmployeeId"] as string;
                if (sessionRole == UserRole.Regular.ToString())
                {
                    int userId = Convert.ToInt16(sessionRole);
                    var deliveredList = db.Distributions
                                                    .Include(d => d.CollectRequest)
                                                     .Include(d => d.DistributedBy)
                                                    .Where(cr => cr.DistributedById == userId)
                                                    .ToList();
                    return View(deliveredList);
                }
                else
                {
                    var collectRequestList = db.Distributions
                                    .Include(d => d.CollectRequest)
                                    .Include(d => d.DistributedBy)
                                    .ToList();
                    return View(collectRequestList);
                }
            }
            return RedirectToAction("Login", "Employee");
        }


        public ActionResult EmployeeList()
        {
            var db = new ZeroHungerContext();
            var employeeList = db.Employees.ToList();
            return View(employeeList);
        }


        public ActionResult ChangeEmployeeRole(int id)
        {
            var db = new ZeroHungerContext();
            var employee = db.Employees.Find(id);
            ViewBag.Notification = null;
            return View(employee);
        }

        [EmployeeAuth]
        [HttpPost]
        public ActionResult ChangeEmployeeRole(EmployeeRoleChangeDto r)
        {

            if (Session["EmployeeRole"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            else
            {
                string sessionRole = Session["EmployeeRole"] as string;

                if (sessionRole == UserRole.Admin.ToString())
                {
                    var db = new ZeroHungerContext();
                    var dbEmployee = db.Employees.Find(r.Id);
                    dbEmployee.Role = r.Role;
                    db.SaveChanges();
                    return RedirectToAction("EmployeeList", "Employee");
                }
                else
                {
                    ViewBag.Notification = "You are not permitted to change the employee role!";
                    return RedirectToAction("ChangeEmployeeRole", "Employee");
                }
            }
        }

        [HttpGet]
        public ActionResult AssignEmployeeToCollect(int id)
        {
            var db = new ZeroHungerContext();
            var employeeList = db.Employees.ToList();
            ViewBag.EmployeeList = employeeList;

            var dbCollectReq = db.CollectRequests
                .Include("Restaurant")
                .FirstOrDefault(cr => cr.Id == id);
            ViewBag.Notification = null;
            return View(dbCollectReq);
        }

        [EmployeeAuth]
        [HttpPost]
        public ActionResult AssignEmployeeToCollect()
        {
            var db = new ZeroHungerContext();
            int collectReqId = Convert.ToInt16(Request.Form["Id"]);
            int employeeId = Convert.ToInt16(Request.Form["EmployeeId"]);
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            else
            {
                string sessionRole = Session["EmployeeRole"] as string;
                if (sessionRole == UserRole.Admin.ToString())
                {
                    int sessionId = Convert.ToInt16(Session["EmployeeId"]);
                    var dbCollectReq = db.CollectRequests.Find(collectReqId);
                    var acceptByEmp = db.Employees.Find(sessionId);
                    var assignEmp = db.Employees.Find(employeeId);

                    dbCollectReq.AcceptedByEmployee = acceptByEmp;
                    dbCollectReq.AssignedEmployee = assignEmp;
                    dbCollectReq.Status = "Approved";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ViewBag.Notification = "You are not allow to Approved Collect Request!";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Notification = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDTO c)
        {
            var db = new ZeroHungerContext();

            if (ModelState.IsValid)
            {
                var verifiedUser = db.Employees.Where(x => x.Email.Equals(c.Email) && x.Password.Equals(c.Password)).FirstOrDefault();
                if (verifiedUser != null)
                {
                    Session["EmployeeEmail"] = c.Email.ToString();
                    Session["EmployeeName"] = verifiedUser.Name.ToString();
                    Session["EmployeeId"] = verifiedUser.Id.ToString();
                    Session["EmployeeRole"] = verifiedUser.Role.ToString();
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ViewBag.Notification = "Wrong Email or password";
                    return View();
                }
            }

            return View(c);
        }

        public ActionResult Logout()
        {
            Session.Remove("EmployeeEmail");
            Session.Remove("EmployeeName");
            Session.Remove("EmployeeId");
            Session.Remove("EmployeeRole");
            return RedirectToAction("Login", "Employee");
        }


        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Employee e)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerContext();
                e.Role = UserRole.Regular;
                db.Employees.Add(e);
                db.SaveChanges();
                return RedirectToAction("Login", "Employee");
            }
            return RedirectToAction("Login", "Employee");
        }

        public ActionResult RestaurantList()
        {
            var db = new ZeroHungerContext();
            var restaurantList = db.Restaurants.ToList();
            return View(restaurantList);
        }
    }
}
using AutoMapper;
using System;
using System.Collections.Generic;
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
            var collectRequestList = db.CollectRequests
                .Where(cr => cr.Status == "Collected")
                .ToList();
            return View(collectRequestList);
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
                string sessionRole = Session["EmployeeRole"] as string;
                if (sessionRole == UserRole.Regular.ToString())
                {
                    var dbCollectReq = db.CollectRequests.Find(collectReqId);
                    dbCollectReq.Status = "Collected";
                    db.SaveChanges();
                    return RedirectToAction("CollectedFood", "Employee");
                }
            }
            return RedirectToAction("CollectedFood", "Employee");
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
                return RedirectToAction("Login","Employee");
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
                if(sessionRole == UserRole.Admin.ToString())
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDTO c)
        {
            var db = new ZeroHungerContext();
            var verifiedUser = db.Employees.Where(x => x.Email.Equals(c.Email) && x.Password.Equals(c.Password)).FirstOrDefault();
            if (verifiedUser != null)
            {
                Session["EmployeeEmail"] = c.Email.ToString();
                Session["EmployeeName"] = verifiedUser.Name.ToString();
                Session["EmployeeId"] = verifiedUser.Id.ToString();
                Session["EmployeeRole"] = verifiedUser.Role.ToString();
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
    }
}
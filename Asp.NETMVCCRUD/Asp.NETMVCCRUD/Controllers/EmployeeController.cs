using Asp.NETMVCCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp.NETMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        // this is the home page controller //
        public ActionResult Index()
        {
            return View();
        }

        // this is get controller for getting employee details //
        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Employee> empList = db.Employees.ToList<Employee>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

        // this is get controller for employee details with particular id // 

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
            {

                using (DBModel db = new DBModel())
                {
                    return View(db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>());
                }

            }
        }

        // this is post controller to fill details of employee //

        [HttpPost]
        public ActionResult AddOrEdit(Employee e)
        {

            using (DBModel db = new DBModel())
            {
                if (e.EmployeeID == 0)
                {
                    db.Employees.Add(e);
                    db.SaveChanges();
                    return Json(new { success = true, message = "saved successfully" }, JsonRequestBehavior.AllowGet);
                }

                // this is post controleer for editting details of employee //

                else
                {
                    db.Entry(e).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "updated successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        // this is delete controller for deleting the details //

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Employee e = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                db.Employees.Remove(e);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using Lastt.Data;
using Lastt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lastt.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> objEmployeeList = _db.tbl_emp;
            return View(objEmployeeList);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Employee employee)
        {
            if (_db.tbl_emp.Any(x => x.UserName == employee.UserName))
            {
                ViewBag.Notification = "This account already exixsts.";
                return View();
            }
            else
            {
                _db.tbl_emp.Add(employee);
                _db.SaveChanges();

                //HttpContext.Session.SetString("EmpId", employee.EmpId);
                //HttpContext.Session.SetString("UserName", employee.UserName);



                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Employee emp)
        {
            var chkLogin = _db.tbl_emp.FirstOrDefault(x => x.UserName.Equals(emp.UserName) && x.Password.Equals(emp.Password));

            if (chkLogin != null)
            {
                // Regular user login successful
                HttpContext.Session.SetInt32("UserId", chkLogin.EmpId);
                HttpContext.Session.SetString("UserName", chkLogin.UserName);
                ViewBag.Notification1 = "Ok!";
                return RedirectToAction("UserAction", "Home");
            }
            else if (emp.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase) && emp.Password.Equals("admin"))
            {
                // Admin login successful
                HttpContext.Session.SetString("UserName", emp.UserName);
                ViewBag.Notification2 = "Admin!";
                return RedirectToAction("AdminAction", "Home");
            }
            else
            {
                // Login failed
                ViewBag.Notification = "Wrong Username or Password!!";
                return View();
            }
        }

        public IActionResult ShowAllRelations()
        {
            var allRelations = _db.tbl_rel.Include(r => r.Employee1).Include(r => r.Employee2).ToList();
            return View(allRelations);
        }



    }
}

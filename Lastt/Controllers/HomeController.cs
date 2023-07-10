using Lastt.Data;
using Lastt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lastt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db= db;
        }

        


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminAction()
        {
            IEnumerable<Employee> objEmployeeList = _db.tbl_emp;
            return View(objEmployeeList);
        }
        [HttpPost]
        public IActionResult Delete(int employeeId)
        {
            var employee = _db.tbl_emp.Find(employeeId);

            if (employee != null)
            {
                _db.tbl_emp.Remove(employee);
                _db.SaveChanges();
            }

            return RedirectToAction("AdminAction");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Employee");
        }
        public IActionResult UserAction()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("UserId");

            var employees = _db.tbl_emp
                .Where(emp => emp.EmpId != loggedUserId &&
                               !_db.tbl_rel.Any(rel => rel.EmpId1 == loggedUserId && rel.EmpId2 == emp.EmpId))
                .ToList();

            ViewBag.username = HttpContext.Session.GetString("UserName");
            ViewBag.userId = loggedUserId;
            return View(employees);
        }

        public IActionResult MyFriends()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("UserId");

            var friends = _db.tbl_emp
                .Join(_db.tbl_rel,
                    emp => emp.EmpId,
                    rel => rel.EmpId2,
                    (emp, rel) => new { Employee = emp, Relation = rel })
                .Where(data => data.Relation.EmpId1 == loggedUserId)
                .Select(data => data.Employee)
                .ToList();

            ViewBag.username = HttpContext.Session.GetString("UserName");

            return View(friends);
        }

        [HttpPost]
        public IActionResult AddFriend(int empId1, int empId2)
        {
            var loggedUserId = HttpContext.Session.GetInt32("UserId");

            // Check if the friend relation already exists
            bool friendRelationExists = _db.tbl_rel.Any(rel => rel.EmpId1 == loggedUserId && rel.EmpId2 == empId2);

            if (friendRelationExists)
            {
                // Friend relation already exists, handle accordingly
                // For example, display a message or redirect with an error
                return RedirectToAction("UserAction");
            }

            // Create a new instance of EmployeeRelation
            var friendRelation = new EmployeeRelation
            {
                EmpId1 = loggedUserId.Value,
                EmpId2 = empId2
            };

            // Add the new friend relation to the context and save changes
            _db.tbl_rel.Add(friendRelation);
            _db.SaveChanges();

            // Redirect back to the UserAction view
            return RedirectToAction("UserAction");
        }

        [HttpPost]
        public IActionResult Unfriend(int empId1, int empId2)
        {
            var loggedUserId = HttpContext.Session.GetInt32("UserId");

            // Find the friend relation to remove
            var friendRelation = _db.tbl_rel.FirstOrDefault(rel => rel.EmpId1 == loggedUserId && rel.EmpId2 == empId2);

            if (friendRelation != null)
            {
                // Remove the friend relation from the context and save changes
                _db.tbl_rel.Remove(friendRelation);
                _db.SaveChanges();
            }

            // Redirect back to the MyFriends view
            return RedirectToAction("MyFriends");
        }

        public IActionResult IndividualData()
        {
            var loggedUsername = HttpContext.Session.GetString("UserName");
            var loggedInUser = _db.tbl_emp.FirstOrDefault(emp => emp.UserName == loggedUsername);

            if (loggedInUser == null)
            {
                // Handle case when the logged-in user is not found
                return RedirectToAction("UserAction");
            }
            ViewBag.username = loggedUsername;
            var userList = new List<Employee> { loggedInUser };

            return View(userList);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(Employee employee)
        {

            if (ModelState.IsValid)
            {
                var loggedUsername = HttpContext.Session.GetString("UserName");
                // Retrieve the logged-in user from the database
                var loggedInUser = _db.tbl_emp.FirstOrDefault(emp => emp.UserName == loggedUsername);

                if (loggedInUser != null)
                {
                    // Update the employee's information
                    loggedInUser.FirstName = employee.FirstName;
                    loggedInUser.LastName = employee.LastName;
                    loggedInUser.Email = employee.Email;
                    loggedInUser.Password = employee.Password;
                    loggedInUser.UserName = employee.UserName;
                    loggedInUser.PhoneNumber = employee.PhoneNumber;

                    // Save the changes to the database
                    _db.SaveChanges();

                    // Redirect back to the IndividualData view
                    return RedirectToAction("UserAction");
                }
            }

            // If the model state is not valid or the logged-in user is not found, redirect to UserAction
            return RedirectToAction("IndicidualData");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
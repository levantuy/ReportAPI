using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace MVCReactPart3.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getEmployeeList(string sortColumnName = "FirstName", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<Employee> List = new List<Employee>();
            int totalPage = 0;
            int totalRecord = 0;

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var emp = dc.Employees.Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(a => a.FirstName.Contains(searchText) || a.LastName.Contains(searchText) || a.EmailID.Contains(searchText) || a.City.Contains(searchText) || a.Country.Contains(searchText));
                }
                totalRecord = emp.Count();
                if (pageSize > 0)
                {
                    totalPage = totalRecord / pageSize + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    List = emp.OrderBy(sortColumnName + " " + sortOrder).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                }
                else
                {
                    List = emp.ToList();
                }
            }

            return new JsonResult
            {
                //Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage},
                Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage, pageSize = pageSize, searchText = searchText },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
	}
}
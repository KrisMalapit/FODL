using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;


namespace FODLSystem.Controllers
{
    public class UsersController : Controller
    {
        private void ResetContextState() => _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
        private readonly FODLSystemContext _context;

        public UsersController(FODLSystemContext context)
        {
            _context = context;
        }

        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index(string domain)
        {
            this.SetCurrentBreadCrumbTitle("Users");
            ViewBag.Domain = domain;
            return View();
        }

        public IActionResult getData(string domain)
        {

            //NEW
            string status = "";
            string message = "";
            var lst = new List<UserViewModel>();
            string stat = "";
            int id = 0;
            string roles = "";
            string domainName = "";
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            PrincipalContext ctx2 = new PrincipalContext(ContextType.Domain);
            try
            {
                status = "success";

                if (domain == "SEMCALACA")
                {
                    domainName = "semcalaca";
                    ctx = new PrincipalContext(ContextType.Domain, domainName, "OU=SLPGC PLANT SITE,dc=semcalaca,dc=com", @"semcalaca\qmaster", "M@st3rQ###");
                    ctx2 = new PrincipalContext(ContextType.Domain, domainName, "OU=SCPC PLANT SITE,dc=semcalaca,dc=com", @"semcalaca\qmaster", "M@st3rQ###");
                }
                else if (domain == "FODLIRARAMINING")
                {
                    domainName = "FODLIRARAMINING";
                    domain = "FODLIRARAMINING";
                    ctx = new PrincipalContext(ContextType.Domain, domainName,
                                                    "OU=FODLIRARA MINESITE,DC=semiraramining,DC=net");
                }
                else
                {
                    domainName = "smcdacon";
                    ctx = new PrincipalContext(ContextType.Domain, domainName,
                                                    "OU=MAKATI HEAD OFFICE,dc=smcdacon,dc=com", @"smcdacon\qmaster", "M@st3rQ###");
                }


                var userPrinciple = new UserPrincipal(ctx);
                using (var search = new PrincipalSearcher(userPrinciple))
                {
                    foreach (UserPrincipal domainUser in search.FindAll().OrderBy(u => u.DisplayName))
                    {
                        var user = _context.Users.Where(u => u.Username == domainUser.SamAccountName.ToString()).Where(u => u.Domain == domainName).Where(u => u.Status == "1").FirstOrDefault();
                        if (user != null)
                        {
                            stat = "Enabled";
                            id = user.Id;
                        }
                        else
                        {
                            stat = "Disabled";
                            id = 0;
                        }
                        var adUser = new UserViewModel()
                        {
                            id = id,
                            Username = domainUser.SamAccountName,
                            Firstname = domainUser.GivenName,
                            Name = domainUser.DisplayName,
                            Lastname = domainUser.Surname,
                            mail = domainUser.EmailAddress,
                            sysid = domainUser.Guid.ToString(),
                            domain = domain,
                            status = stat,
                            Roles = roles

                        };
                        lst.Add(adUser);
                    }
                }

                if (domain == "SEMALACA")
                {
                    //FOR SEMALACA USING 2ND OU
                    var userPrinciple2 = new UserPrincipal(ctx2);
                    using (var search2 = new PrincipalSearcher(userPrinciple2))
                    {
                        foreach (UserPrincipal domainUser in search2.FindAll().OrderBy(u => u.DisplayName))
                        {

                            var user = _context.Users.Where(u => u.Username == domainUser.SamAccountName.ToString()).Where(u => u.Domain == domainName).Where(u => u.Status == "1").FirstOrDefault();


                            if (user != null)
                            {
                                stat = "Enabled";
                                id = user.Id;

                            }
                            else
                            {
                                stat = "Disabled";
                                id = 0;

                            }
                            var adUser = new UserViewModel()
                            {
                                id = id,
                                Username = domainUser.SamAccountName,
                                Firstname = domainUser.GivenName,
                                Name = domainUser.DisplayName,
                                Lastname = domainUser.Surname,
                                mail = domainUser.EmailAddress,
                                sysid = domainUser.Guid.ToString(),
                                domain = domain,
                                status = stat,
                                Roles = roles
                            };
                            lst.Add(adUser);

                        }
                    }
                }

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
                throw;
            }






            var model = new
            {

                status,
                message,
                data = lst
            };
            return Json(model);



        }



        [HttpPost]
        public IActionResult EnableDisableUser(string Domain, string UserName, string Email, string Status, string Name, string UserType)
        {

            string status = "";
            string message = "";

            try
            {
                if (Status == "Disabled")
                {
                    var result = _context.Users.Where(b => b.Username == UserName).Where(b => b.Domain == Domain).FirstOrDefault();
                    if (result != null)
                    {
                        result.Email = Email;
                        result.Status = "1";
                        _context.Entry(result).State = EntityState.Modified;
                        _context.SaveChanges();
                        status = "success";
                    }
                    else
                    {
                        User user = new User();
                        user.DepartmentId = 1; //Not set
                        user.Username = UserName;
                        user.Domain = Domain;
                        user.Name = Name;
                        user.Email = Email;
                        user.Status = "1";
                        user.RoleId = 2;
                        user.UserType = UserType;
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        status = "success";
                    }


                }
                else
                {

                    var result = _context.Users.Where(b => b.Username == UserName).Where(b => b.Domain == Domain).FirstOrDefault();
                    if (result != null)
                    {
                        result.Status = "0";
                        _context.Entry(result).State = EntityState.Modified;
                        _context.SaveChanges();

                    }
                    else
                    {
                        User user = new User();
                        user.DepartmentId = 1; //Not set
                        user.Username = UserName;
                        user.Domain = Domain;
                        user.Name = Name;
                        user.Status = "1";
                        user.RoleId = 2;
                        user.UserType = UserType;
                        _context.Users.Add(user);
                        _context.SaveChanges();

                    }


                    status = "success";

                }

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.InnerException.Message.ToString();

            }
            var model = new
            {
                status,
                message

            };



            return Json(model);
        }



        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult Edit(int? id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Users",
                Url = string.Format(Url.Action("Index", "Users")),
                Order = 1
            });
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.Users.Include(a => a.Departments).Where(a => a.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            string deptname = user.Departments.Name;
            ViewData["Department"] = new SelectList(_context.Departments.Where(a => a.Status == "Active").Where(a => a.CompanyId == user.Departments.CompanyId), "ID", "Name", user.DepartmentId);
            //ViewData["Company"] = new SelectList(_context.Companies.Where(a => a.Status == "Active"), "ID", "Name", user.Departments.CompanyId);
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);


            return View(user);
        }

        [HttpPost]
        public IActionResult ReloadDepartment(int? id)
        {
            var dept = new SelectList(_context.Departments.Where(a => a.Status == "Active").Where(a => a.CompanyId == id), "ID", "Name");

            return Json(dept);
        }

        [HttpPost]
        public IActionResult Edit(User u, string[] companytags)
        {

            //u.CompanyAccess = companytags.ToString();
            string companyaccess = string.Join(",", companytags);



            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(u.Id);
                user.CompanyAccess = companyaccess;
                user.RoleId = u.RoleId;
                user.DepartmentId = u.DepartmentId;
                user.UserType = u.UserType;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(u);
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            string status = "";
            string message = "";

            try
            {
                User detail = _context.Users.Find(id);
                detail.Status = "0";
                _context.Entry(detail).State = EntityState.Modified;
                _context.SaveChanges();
                status = "success";
            }
            catch (Exception ex)
            {
                message = ex.InnerException.InnerException.Message;
                status = "failed";
            }
            var model = new
            {
                status,
                message

            };
            return Json(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public JsonResult SelectUserList(string q)
        {

            var model = _context.Users.Where(b => b.Status == "1").Select(b => new
            {

                id = b.Id,
                text = b.Name,
            });

            if (!string.IsNullOrEmpty(q))
            {
                model = model.Where(b => b.text.Contains(q));
            }

            var modelUser = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelUser);
        }

    }
}

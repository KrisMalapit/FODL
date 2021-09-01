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
            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();
            this.SetCurrentBreadCrumbTitle("Users");
            ViewBag.Domain = domain;
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "ID", "Name");

            ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks.Where(a => stat.Contains(a.Status)), "Id", "Description");
            ViewData["DispenserId"] = new SelectList(_context.Dispensers.Where(a => stat.Contains(a.Status)), "Id", "Name");


            return View();
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "ID", "Name");
            ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks.Where(a => stat.Contains(a.Status)), "Id", "Description");
            ViewData["DispenserId"] = new SelectList(_context.Dispensers.Where(a => stat.Contains(a.Status)), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocalUserViewModel userView, string[] lubetags, string[] dispensertags)
        {
            string lubeaccess = string.Join(",", lubetags);
            string dispenseraccess = string.Join(",", dispensertags);
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User();
                    user.Password = GetSHA1HashData(userView.Password);
                    user.RoleId = userView.RoleId;
                    user.Username = userView.Username;
                    user.Status = "1";
                    user.Domain = "Local";
                    user.FirstName = userView.Firstname;
                    user.LastName = userView.Lastname;
                    user.Name = userView.Firstname + " " + userView.Lastname;
                    user.DepartmentId = userView.DepartmentId;
                    user.CompanyAccess = "1";
                    user.LubeAccess = lubeaccess;
                    user.DispenserAccess = dispenseraccess;
                    _context.Add(user);
                    await _context.SaveChangesAsync();


                    Log log = new Log
                    {
                        Descriptions = "New User - " + user.Id,
                        Action = "Add",
                        Status = "success",
                        UserId = User.Identity.Name
                    };
                    _context.Add(log);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ResetContextState();
                    Log log = new Log
                    {
                        Descriptions = "New User - " + e.InnerException.Message,
                        Action = "Add",
                        Status = "fail",
                        UserId = User.Identity.Name
                    };
                    _context.Add(log);
                    await _context.SaveChangesAsync();
                    ModelState.AddModelError("", e.InnerException.Message);
                }

            }






            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "Id", "Name", userView.RoleId);
            ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "ID", "Name", userView.DepartmentId);
            return View(userView);
        }
        //// POST: Users/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(User user)
        //{
            

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var users = _context.Users.Where(a => a.Id == user.Id).FirstOrDefault();
                    
        //            if (users != null)
        //            {
        //                _context.Entry(users).State = EntityState.Detached;
        //                user.Name = user.FirstName + " " + user.LastName;
        //                user.Password = users.Password;
        //            }


        //            _context.Update(user);
        //            await _context.SaveChangesAsync();

        //            Log log = new Log
        //            {
        //                Descriptions = "Edit User - " + user.Id,
        //                Action = "Edit",
        //                Status = "success",
        //                UserId = User.Identity.Name
        //            };
        //            _context.Add(log);
        //            await _context.SaveChangesAsync();


        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            ResetContextState();
        //            Log log = new Log
        //            {
        //                Descriptions = "Edit User - " + user.Id,
        //                Action = "Edit",
        //                Status = "fail",
        //                UserId = User.Identity.Name
        //            };
        //            _context.Add(log);
        //            await _context.SaveChangesAsync();

                  

        //        }
        //        return RedirectToAction(nameof(Index));
        //    }


        //    var itemStatus = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "Disabled", Value = "0"},
        //        new SelectListItem {Text = "Enabled", Value = "1"}
        //    };


        //    ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "Id", "Name", user.RoleId);
        //    ViewData["Status"] = new SelectList(itemStatus, "Value", "Text", user.Status);
        //    return View(user);
        //}


        private string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
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

                //if (domain == "SEMCALACA")
                //{
                //    domainName = "semcalaca";
                //    ctx = new PrincipalContext(ContextType.Domain, domainName, "OU=SLPGC PLANT SITE,dc=semcalaca,dc=com", @"semcalaca\qmaster", "M@st3rQ###");
                //    ctx2 = new PrincipalContext(ContextType.Domain, domainName, "OU=SCPC PLANT SITE,dc=semcalaca,dc=com", @"semcalaca\qmaster", "M@st3rQ###");
                //}
                //else 
                if (domain == "SEMIRARAMINING")
                {
                    domainName = "SEMIRARAMINING";
                    domain = "SEMIRARAMINING";
                    ctx = new PrincipalContext(ContextType.Domain, domainName,
                                                    "OU=SEMIRARA MINESITE,DC=semiraramining,DC=net");
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

                if (domain == "SEMCALACA")
                {
                    //FOR SEMCALACA USING 2ND OU
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
        public IActionResult getDataLocal()
        {

            string status = "";
            var v =

                _context.Users.Where(a => a.Status == "1").Where(a=>a.Domain == "Local").Select(a => new {


                    a.Username
                      ,
                    a.Name
                     
                        ,

                    a.Id
                    , a.DepartmentId
                    , Department = a.Departments.Name
                    , Role = a.Roles.Name
                    , a.RoleId
                    , a.FirstName
                    , a.LastName

                    ,
                    LubeAccess = a.LubeAccess
                    ,
                    DispenserAccess = a.DispenserAccess


                });
            status = "success";






            var model = new
            {
                status
                ,
                data = v.ToList()
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
                        user.DispenserAccess = "1";
                        user.LubeAccess = "1";
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
                        user.CompanyAccess = "1";
                        user.DispenserAccess = "1";
                        user.LubeAccess = "1";
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
            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();

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
            ViewData["Company"] = new SelectList(_context.Companies.Where(a => a.Status == "Active"), "ID", "Name", user.Departments.CompanyId);
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);


            ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks.Where(a => stat.Contains(a.Status)), "Id", "Description");
            ViewData["DispenserId"] = new SelectList(_context.Dispensers.Where(a => stat.Contains(a.Status)), "Id", "Name");
            return View(user);
        }

        [HttpPost]
        public IActionResult ReloadDepartment(int? id)
        {
            var dept = new SelectList(_context.Departments.Where(a => a.Status == "Active").Where(a => a.CompanyId == id), "ID", "Name");

            return Json(dept);
        }

        [HttpPost]
        public IActionResult Edit(User u, string[] companytags,string[] lubetags,string[] dispensertags)
        {

            //u.CompanyAccess = companytags.ToString();
            string companyaccess = string.Join(",", companytags);
            string lubeaccess = string.Join(",", lubetags);
            string dispenseraccess = string.Join(",", dispensertags);



            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(u.Id);
                user.CompanyAccess = companyaccess;
                user.RoleId = u.RoleId;
                user.DepartmentId = u.DepartmentId;
                user.UserType = u.UserType;
                user.LubeAccess = lubeaccess;
                user.DispenserAccess = dispenseraccess;
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
                detail.Status = "0" + DateTime.Now.Date.Ticks;
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
        [HttpPost]
        public ActionResult SaveLocal(LocalUserViewModel luser, string[] lubetags, string[] dispensertags)
        {
            string lubeaccess = string.Join(",", lubetags);
            string dispenseraccess = string.Join(",", dispensertags);


            string status = "";
            string message = "";
            try
            {

                
                var item = _context.Users.Find(luser.Id);
                item.Username = luser.Username;
                item.LastName = luser.Lastname;
                item.FirstName = luser.Firstname;
                item.DepartmentId = luser.DepartmentId;
                item.Name = luser.Firstname + " " + luser.Lastname;
                item.RoleId = luser.RoleId;
                item.CompanyAccess = "1";
                item.LubeAccess = lubeaccess;
                item.DispenserAccess = dispenseraccess;

                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
               


                status = "success";
            }
            catch (Exception ex)
            {
                status = "fail";
                message = ex.InnerException.Message;
            }

            var model = new
            {
                status,
                message
            };

            return Json(model);
        }

    }
}

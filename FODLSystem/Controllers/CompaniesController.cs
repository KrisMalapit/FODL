using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FODLSystem.Models;

namespace FODLSystem.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly SEMSystemContext _context;

        public CompaniesController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Companies
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Company");
            return View(await _context.Companies.ToListAsync());
        }
        public IActionResult getData()
        {
            string status = "";
            var v =

                _context.Companies.Where(a => a.Status != "Deleted").Select(a => new {


                    a.Code
                      ,
                    a.Name
                 
                    ,

                    a.ID




                });
            status = "success";






            var model = new
            {
                status
                ,
                data = v
            };
            return Json(model);
        }



        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Companies/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Company",
                Url = string.Format(Url.Action("Index", "Companies")),
                Order = 1
            });
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();

                Log log = new Log
                {
                    Descriptions = "Create Company - " + company.ID,
                    Action = "Create",
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);

                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Company",
                Url = string.Format(Url.Action("Index", "Companies")),
                Order = 1
            });

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company company)
        {
            if (id != company.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();

                    Log log = new Log
                    {
                        Descriptions = "Update Company - " + company.ID,
                        Action = "Edit",
                        Status = "success",
                        UserId = User.Identity.GetUserName()
                    };
                    _context.Add(log);

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
        [BreadCrumb(Title = "Delete", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Company",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });
            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Companies.FindAsync(id);
            model.Status = "Deleted";
            _context.Update(model);

            await _context.SaveChangesAsync();


            Log log = new Log
            {
                Descriptions = "Delete Company - " + id,
                Action = "Delete",
                Status = "success",
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.ID == id);
        }
    }
}

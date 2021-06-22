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
    public class BicyclesController : Controller
    {
        private readonly SEMSystemContext _context;

        public BicyclesController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Bicycle");
            return View();
        }
        public IActionResult getData()
        {
            string status = "";
            var v =

                _context.Bicycles.Where(a => a.Status == "Active").Select(a => new {


                    a.BrandName,
                    a.ContactNo,
                    a.IdentificationNo,
                    a.NameOwner,
                    CompanyName = a.Departments.Companies.Name,
                    DepartmentName = a.Departments.Name,
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



        // GET: Bicycles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycle = await _context.Bicycles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            return View(bicycle);
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Bicycles/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Bicycle",
                Url = string.Format(Url.Action("Index", "Bicycles")),
                Order = 1
            });


            var dept = _context.Departments.Where(a=>a.Status == "Active").Select(a => new
            {
                a.ID,
                Text = a.Name + " - " + a.Companies.Code
            });

            ViewData["DepartmentID"] = new SelectList(dept.OrderBy(a=>a.Text), "ID", "Text");
            return View();
        }

        // POST: Bicycles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bicycle bicycle)
        {
           
            if (ModelState.IsValid)
            {

                
                _context.Add(bicycle);
                await _context.SaveChangesAsync();

                Log log = new Log
                {
                    Descriptions = "Create Bicycle - " + bicycle.ID,
                    Action = "Create",
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);

                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            return View(bicycle);
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Bicycles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Bicycle",
                Url = string.Format(Url.Action("Index", "Bicycles")),
                Order = 1
            });

            if (id == null)
            {
                return NotFound();
            }

            var bicycle = await _context.Bicycles.FindAsync(id);


           


            if (bicycle == null)
            {
                return NotFound();
            }
            else
            {

                var dept = _context.Departments.Where(a => a.Status == "Active").Select(a => new
                {
                    a.ID,
                    Text = a.Name + " - " + a.Companies.Code
                });

                ViewData["DepartmentID"] = new SelectList(dept.OrderBy(a => a.Text), "ID", "Text", bicycle.DepartmentID);
            }
            return View(bicycle);
        }

        // POST: Bicycles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bicycle bicycle)
        {
            if (id != bicycle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bicycle);
                    await _context.SaveChangesAsync();

                    Log log = new Log
                    {
                        Descriptions = "Update Bicycle - " + bicycle.ID,
                        Action = "Edit",
                        Status = "success",
                        UserId = User.Identity.GetUserName()
                    };
                    _context.Add(log);

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BicycleExists(bicycle.ID))
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
            return View(bicycle);
        }
        [BreadCrumb(Title = "Delete", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Bicycles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Bicycle",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });
            var bicycle = await _context.Bicycles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            return View(bicycle);
        }

        // POST: Bicycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Bicycles.FindAsync(id);
            model.Status = "Deleted_" + DateTime.Now.Ticks.ToString() ;
            _context.Update(model);

            await _context.SaveChangesAsync();


            Log log = new Log
            {
                Descriptions = "Delete Bicycle - " + id,
                Action = "Delete",
                Status = "success",
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        private bool BicycleExists(int id)
        {
            return _context.Bicycles.Any(e => e.ID == id);
        }
    }
}

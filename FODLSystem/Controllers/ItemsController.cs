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
    public class ItemsController : Controller
    {
        private readonly SEMSystemContext _context;

        public ItemsController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Items
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Item");
            return View(await _context.Items.ToListAsync());
        }
        public IActionResult getData()
        {

            var yearDate = DateTime.Now.Date;


            var _itemForRefill = _context.Items
                .Where(a => a.EquipmentType == "Fire Extinguisher")
                .Where(a => a.Status == "Active")
                .Where(a=>a.ItemStatus != "For Refill")
                .Where(a=>a.DatePurchased.Value.Date.AddMonths(12) <= yearDate)
                .ToList();

            if (_itemForRefill.Count() > 0)
            {
                _itemForRefill.ForEach(a =>
                    {
                       a.ItemStatus = "For Refill";
                    }
                );
                _context.SaveChanges();
               
            }


            string status = "";
            var v =
                _context.Items
                .Where(a => a.Status != "Deleted")
                .Select(a => new {
                    a.Code,
                    a.Name,
                    a.DatePurchased,
                    a.ItemStatus,
                    a.Warranty,
                    a.SerialNo,
                    a.Id,
                    a.EquipmentType
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
        public IActionResult getDataActivity(int id)
        {
            string status = "";
            var v =

                _context.ItemLogs
                .Where(a=>a.ItemId == id)
                .Select(a => new {


                   a.CreatedDate
                  
                   ,a.Description
                   ,
                    a.CreatedBy

                    ,a.Id

                }).OrderByDescending(a => a.Id);
            status = "success";






            var model = new
            {
                status
                ,
                data = v
            };
            return Json(model);
        }
        public IActionResult getItems(string equipment)
        {
            string status = "";
            switch (equipment)
            {
                case "FE":
                    equipment = "Fire Extinguisher";
                    break;
                case "FH":
                    equipment = "Fire Hydrant";
                    break;
                case "IT":
                    equipment = "Inergen Tank";
                    break;
                case "EL":
                    equipment = "Emergency Light";
                    break;
            }
            var v =

                _context.Items
                .Where(a => a.Status != "Deleted")
                .Where(a=>a.EquipmentType == equipment)
                .Select(a => new {
                    a.Code,
                    a.Name,
                    a.DatePurchased,
                    a.ItemStatus,
                    a.Warranty,
                    a.SerialNo
                    ,a.EquipmentType,
                    a.Id,
                    Existing = _context.LocationItemDetails.Where(b=>b.ItemId == a.Id).Where(b=>b.Status == "Active").Count()
                })
                .Where(a=>a.Existing == 0);

            status = "success";






            var model = new
            {
                status
                ,
                data = v
            };
            return Json(model);
        }
        public IActionResult getDataItems(string type, int refid)
        {
            string status = "";
            
                var v =
                _context.LocationItemDetails
                .Where(a => a.HeaderId == refid)
                .Where(a => a.Equipment == type)
                .Where(a => a.Status == "Active")
                .Select(a => new {
                    a.Items.Code,
                    a.Items.Name,
                    a.Items.SerialNo,
                    a.Items.ItemStatus,
                    a.Id
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


        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Item == null)
            {
                return NotFound();
            }

            return View(Item);
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Items/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Item",
                Url = string.Format(Url.Action("Index", "Items")),
                Order = 1
            });
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {

            if (item.DatePurchased == null)
            {
                item.Warranty = 0;
            }
            if (ModelState.IsValid)
            {
                string series = "";
                string refno = "";
                string series_code = "ITEM";

                series =  new NoSeriesController(_context).GetNoSeries(series_code);
                refno = "IT" + series;
                item.Code = refno;
                _context.Add(item);
                await _context.SaveChangesAsync();

                //NoSeries ns = _context.NoSeries.SingleOrDefault(v => v.Code == series_code);
                //ns.LastNoUsed = series;
                //_context.SaveChanges();

                string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);


                ItemLog itemLog = new ItemLog
                {
                    ItemId = item.Id,
                    OldStatus = "",
                    NewStatus = "New",
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.GetFullName(),
                    Description = "Item added to item maintenance"
                };
                _context.Add(itemLog);   


                Log log = new Log
                {
                    Descriptions = "Create Item - " + item.Id,
                    Action = "Create",
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);
                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Item",
                Url = string.Format(Url.Action("Index", "Items")),
                Order = 1
            });

            if (id == null)
            {
                return NotFound();
            }

            var Item = await _context.Items.FindAsync(id);
            if (Item == null)
            {
                return NotFound();
            }
            ViewData["StatusOldValue"] = Item.ItemStatus;
            return View(Item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item, string StatusOldValue)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    item.Status = "Active";
                    _context.Update(item);
                    await _context.SaveChangesAsync();


                    if (item.ItemStatus != StatusOldValue)
                    {
                        ItemLog itemLog = new ItemLog
                        {
                            ItemId = item.Id,
                            OldStatus = StatusOldValue,
                            NewStatus = item.ItemStatus,
                            CreatedDate = DateTime.Now,
                            CreatedBy = User.Identity.GetFullName(),
                            Description = "Changes status from " + StatusOldValue + " to " + item.ItemStatus
                        };
                        _context.Add(itemLog);
                    }


                    Log log = new Log
                    {
                        Descriptions = "Update Item - " + item.Id,
                        Action = "Edit",
                        Status = "success",
                        UserId = User.Identity.GetUserName()
                    };
                    _context.Add(log);

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }
        [BreadCrumb(Title = "Delete", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Item",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });
            var Item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Item == null)
            {
                return NotFound();
            }

            return View(Item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Items.FindAsync(id);
            model.Status = "Deleted";
            _context.Update(model);

            await _context.SaveChangesAsync();


            Log log = new Log
            {
                Descriptions = "Delete Item - " + id,
                Action = "Delete",
                Status = "success",
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        public JsonResult DeleteItem(int id, string LocationName)
        {
            string status = "";
            string message = "";

            try
            {
                


                   
                    var item = _context.LocationItemDetails.Find(id);
                    item.Status = "Deleted_" + DateTime.Now.Ticks;
                    _context.Update(item);


                    Log log = new Log();
                    log.Descriptions = "Delete items in [LocationFireExtinguisherDetail] ItemId : " + id;
                    log.Action = "Delete";
                    log.Status = "Success";
                    log.UserId = User.Identity.GetUserName();
                    log.CreatedDate = DateTime.Now;
                    _context.Add(log);


                _context.Items.Find(item.ItemId).IsIn = 1;


                ItemLog itemLog = new ItemLog
                    {
                        ItemId = id,
                        OldStatus = "",
                        NewStatus = "",
                        CreatedDate = DateTime.Now,
                        CreatedBy = User.Identity.GetFullName(),
                        Description = "Removed item from location " + LocationName
                        
                    };
                    _context.Add(itemLog);
                _context.SaveChanges();

                status = "success";
            }
            catch (Exception e)
            {

                status = "fail";
                message = e.InnerException.Message;
            }

            var model = new
            {
                status,
                message
            };
            return Json(model);
        }
        public IActionResult SaveItemLocation(ItemListViewModel[] list,string LocationName) {

            string status = "";
            string message = "";
            try
            {
                foreach (var item in list)
                {
                    LocationItemDetail loc = new LocationItemDetail
                    {
                        CreatedDate = DateTime.Now,
                        Equipment = item.Equipment,
                        HeaderId = item.HeaderId,
                        ItemId = item.ItemId,
                        UpdatedBy = User.Identity.GetFullName()
                    };
                    _context.Add(loc);

                    ItemLog itemLog = new ItemLog
                    {
                        ItemId = item.ItemId,
                        OldStatus = "",
                        NewStatus = "",
                        CreatedDate = DateTime.Now,
                        CreatedBy = User.Identity.GetFullName(),
                        Description = "Added item to location " + LocationName
                    };

                    _context.Items.Find(item.ItemId).IsIn = 0;

                    _context.Add(itemLog);
                }

                Log log = new Log
                {
                    Descriptions = "Added items in [LocationItemDetail] HeaderId : " + list[0].HeaderId,
                    Action = "Add",
                    Status = "Success",
                    UserId = User.Identity.GetUserName(),
                    CreatedDate = DateTime.Now
                };
                _context.Add(log);

                _context.SaveChanges();


                

                status = "success";
            }
            catch (Exception e)
            {
                message = e.Message;
                status = "fail";
            }

            var model = new
            {
                status,
                message
            };

            return Json(model);
        }
        public class ItemListViewModel
        {

            public int ItemId { get; set; }
            public string Equipment { get; set; }
            public int HeaderId { get; set; }


        }
        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}

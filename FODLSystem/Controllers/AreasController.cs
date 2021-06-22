using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;

namespace FODLSystem.Controllers
{
    public class AreasController : Controller
    {
        private readonly SEMSystemContext _context;

        public AreasController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Areas
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Areas");
            var SEMSystemContext = _context.Areas.Include(d => d.Companies);

            return View();
        }
        public JsonResult SearchArea(string q)
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            var model = _context
                .Areas.Where(a => a.Status == "Active")
                .Where(a => compId.Contains(a.CompanyId))
                .Where( a=>a.Name.ToUpper().Contains(q.ToUpper()))
                .Select(b => new
            {
                id = b.ID,
                text = b.Name,

            });

            var modelItem = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelItem);
        }
        public IActionResult getData()
        {
            string status = "";
            var v =

                _context.Areas.Where(a => a.Status != "Deleted").Select(a => new {


                    a.Code
                      ,
                    a.Name
                    ,
                    CompanyName = a.Companies.Name,
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
        public IActionResult getLocation(int areaid)
        {
            string company = _context.Areas.Include(a=>a.Companies).Where(a=>a.ID == areaid).FirstOrDefault().Companies.Name;
            string status = "";
            var fe =

                _context.LocationFireExtinguishers.Where(a => a.Status == "Active").Where(a=>a.AreaId == areaid).Select(a => new {
                    a.Id,
                    a.Location
                });
            var el =

                _context.LocationEmergencyLights.Where(a => a.Status == "Active").Where(a => a.AreaId == areaid).Select(a => new {
                    a.Id,
                    a.Location
                });
            var fh =

                _context.LocationFireHydrants.Where(a => a.Status == "Active").Where(a => a.AreaId == areaid).Select(a => new {
                    a.Id,
                    a.Location
                });
            var it =

                _context.LocationInergenTanks.Where(a => a.Status == "Active").Where(a => a.AreaId == areaid).Select(a => new {
                    a.Id,
                    a.Location
                });
            status = "success";






            var model = new
            {
                status
                ,
                datafe = fe,
                datael = el,
                datafh = fh,
                datait = it,
                company
            };
            return Json(model);
        }
        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Area = await _context.Areas
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Area == null)
            {
                return NotFound();
            }

            return View(Area);
        }

        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Areas/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Area",
                Url = string.Format(Url.Action("Index", "Areas")),
                Order = 1
            });

            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name");
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Code,Name,Status,CompanyId")] Area Area)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Area);
                await _context.SaveChangesAsync();


                Log log = new Log
                {
                    Descriptions = "Create Area - " + Area.ID,
                    Action = "Create",
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);

                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", Area.CompanyId);
            return View(Area);
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Area",
                Url = string.Format(Url.Action("Index", "Areas")),
                Order = 1
            });

            var Area = await _context.Areas.FindAsync(id);
            if (Area == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", Area.CompanyId);
            return View(Area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Name,Status,CompanyId")] Area Area)
        {
            if (id != Area.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Area);
                    await _context.SaveChangesAsync();

                    Log log = new Log
                    {
                        Descriptions = "Update Area - " + Area.ID,
                        Action = "Edit",
                        Status = "success",
                        UserId = User.Identity.GetUserName()
                    };
                    _context.Add(log);

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(Area.ID))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", Area.CompanyId);
            return View(Area);
        }

        [BreadCrumb(Title = "Delete", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Area",
                Url = string.Format(Url.Action("Index", "Areas")),
                Order = 1
            });
            var Area = await _context.Areas
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Area == null)
            {
                return NotFound();
            }

            return View(Area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Areas.FindAsync(id);
            model.Status = "Deleted";
            _context.Update(model);
            await _context.SaveChangesAsync();


            Log log = new Log
            {
                Descriptions = "Delete Area - " + id,
                Action = "Delete",
                Status = "success",
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult SaveLocation(LocationViewModel locdetail)
        {
          

            string YearToDate = DateTime.Now.Year.ToString();

            string status = "";
            string message = "";
            string comp = User.Identity.GetCompanyAccess();

            try
            {
                int dcnt = locdetail.detail_id.Count();
                switch (locdetail.Equipment)
                {
                    case "divFE":
                        var locFE = _context.LocationFireExtinguishers.Where(a => a.AreaId == locdetail.AreaId).ToList();
                        if (locFE.Count() == 0)
                        {
                            for (int i = 0; i < dcnt; i++)
                            {
                                var loc = new LocationFireExtinguisher
                                {
                                    //Capacity = locdetail.capacity[i],
                                    AreaId = locdetail.AreaId,
                                    Code = locdetail.code[i],
                                    Location = locdetail.location[i],
                                    
                                    Status = "Active"
                                };
                                _context.Add(loc);
                            }

                            _context.SaveChanges();
                        }
                        else
                        {

                            locFE.ForEach(x => x.Status = "Deleted_" + DateTime.Now.Ticks.ToString());  //set all record to deleted

                            for (int i = 0; i < dcnt; i++)
                            {
                                try
                                {
                                    var _location = _context.LocationFireExtinguishers.Find(locdetail.detail_id[i]);
                                    if (_location == null)
                                    {
                                        var loc = new LocationFireExtinguisher
                                        {
                                            Id = locdetail.detail_id[i],
                                            //Capacity = locdetail.capacity[i],
                                            AreaId = locdetail.AreaId,
                                            Code = locdetail.code[i],
                                            Location = locdetail.location[i],
                                            //Type = locdetail.type[i],
                                            Status = "Active"
                                        };
                                        _context.Add(loc);
                                    }
                                    else
                                    {
                                        //_location.Capacity = locdetail.capacity[i];
                                        _location.AreaId = locdetail.AreaId;
                                        _location.Code = locdetail.code[i];
                                        _location.Location = locdetail.location[i];
                                        //_location.Type = locdetail.type[i];
                                        _location.Status = "Active";
                                        _context.Update(_location);
                                    }
                                    _context.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                    status = "failed";
                                }

                            }
                        }
                        break;
                    case "divEL":
                        var locEL = _context.LocationEmergencyLights.Where(a => a.AreaId == locdetail.AreaId).ToList();
                        if (locEL.Count() == 0)
                        {
                            try
                            {
                                for (int i = 0; i < dcnt; i++)
                                {
                                    var loc = new LocationEmergencyLight
                                    {
                                    
                                        AreaId = locdetail.AreaId,
                                        Code = locdetail.code[i],
                                        Location = locdetail.location[i],
                                    
                                        Status = "Active"
                                    };
                                    _context.Add(loc);
                                }

                                _context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                status = "failed";
                            }
                        }
                        else
                        {

                            locEL.ForEach(x => x.Status = "Deleted_" + DateTime.Now.Ticks.ToString());  //set all record to deleted

                            for (int i = 0; i < dcnt; i++)
                            {
                                try
                                {
                                    var _location = _context.LocationEmergencyLights.Find(locdetail.detail_id[i]);
                                    if (_location == null)
                                    {
                                        var loc = new LocationEmergencyLight
                                        {
                                            Id = locdetail.detail_id[i],
                                           
                                            AreaId = locdetail.AreaId,
                                            Code = locdetail.code[i],
                                            Location = locdetail.location[i],
                                           
                                            Status = "Active"
                                        };
                                        _context.Add(loc);
                                    }
                                    else
                                    {
                                       
                                        _location.AreaId = locdetail.AreaId;
                                        _location.Code = locdetail.code[i];
                                        _location.Location = locdetail.location[i];
                                        
                                        _location.Status = "Active";
                                        _context.Update(_location);
                                    }
                                    _context.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                    status = "failed";
                                }

                            }
                        }


                      


                        break;

                    case "divFH":
                        var locFH = _context.LocationFireHydrants.Where(a => a.AreaId == locdetail.AreaId).ToList();
                        if (locFH.Count() == 0)
                        {
                            try
                            {
                                for (int i = 0; i < dcnt; i++)
                                {
                                    var loc = new LocationFireHydrant
                                    {

                                        AreaId = locdetail.AreaId,
                                        Code = locdetail.code[i],
                                        Location = locdetail.location[i],

                                        Status = "Active"
                                    };
                                    _context.Add(loc);
                                }

                                _context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                status = "failed";
                            }
                        }
                        else
                        {

                            locFH.ForEach(x => x.Status = "Deleted_" + DateTime.Now.Ticks.ToString());  //set all record to deleted

                            for (int i = 0; i < dcnt; i++)
                            {
                                try
                                {
                                    var _location = _context.LocationFireHydrants.Find(locdetail.detail_id[i]);
                                    if (_location == null)
                                    {
                                        var loc = new LocationFireHydrant
                                        {
                                            Id = locdetail.detail_id[i],

                                            AreaId = locdetail.AreaId,
                                            Code = locdetail.code[i],
                                            Location = locdetail.location[i],

                                            Status = "Active"
                                        };
                                        _context.Add(loc);
                                    }
                                    else
                                    {

                                        _location.AreaId = locdetail.AreaId;
                                        _location.Code = locdetail.code[i];
                                        _location.Location = locdetail.location[i];

                                        _location.Status = "Active";
                                        _context.Update(_location);
                                    }
                                    _context.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                    status = "failed";
                                }

                            }
                        }





                        break;
                    case "divIT":
                        var locIT = _context.LocationInergenTanks.Where(a => a.AreaId == locdetail.AreaId).ToList();
                        if (locIT.Count() == 0)
                        {
                            try
                            {
                                for (int i = 0; i < dcnt; i++)
                                {
                                    var loc = new LocationInergenTank
                                    {

                                        AreaId = locdetail.AreaId,
                                        //Serial = locdetail.serial[i],
                                        //Capacity = locdetail.capacity[i],
                                        Location = locdetail.location[i],
                                        Area = locdetail.area[i],
                                        LocationCylinder = locdetail.locationcylinder[i],
                                        Status = "Active"
                                    };
                                    _context.Add(loc);
                                }

                                _context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                status = "failed";
                            }
                        }
                        else
                        {

                            locIT.ForEach(x => x.Status = "Deleted_" + DateTime.Now.Ticks.ToString());  //set all record to deleted

                            for (int i = 0; i < dcnt; i++)
                            {
                                try
                                {
                                    var _location = _context.LocationInergenTanks.Find(locdetail.detail_id[i]);
                                    if (_location == null)
                                    {
                                        var loc = new LocationInergenTank
                                        {
                                           
                                            //Serial = locdetail.serial[i],
                                            //Capacity = locdetail.capacity[i],
                                            AreaId = locdetail.AreaId,
                                            Area = locdetail.area[i],
                                            Location = locdetail.location[i],
                                            LocationCylinder = locdetail.locationcylinder[i],
                                            Status = "Active"
                                        };
                                        _context.Add(loc);
                                    }
                                    else
                                    {
                                        _location.AreaId = locdetail.AreaId;
                                        //_location.Serial = locdetail.serial[i];
                                        //_location.Capacity = locdetail.capacity[i];
                                        _location.Location = locdetail.location[i];
                                        _location.LocationCylinder = locdetail.locationcylinder[i];
                                        _location.Area = locdetail.area[i];
                                        _location.Status = "Active";
                                        _context.Update(_location);
                                    }
                                    _context.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                    status = "failed";
                                }

                            }
                        }

                        break;
                    default:
                        break;
                }
                
                   
                status = "success";
            }
            catch (Exception e)
            {
                message = e.Message;
                status = "failed";

            }

            Log log = new Log
            {
                Descriptions = "Area Modification : ID " + locdetail.AreaId,
                Action = "Create / Modify Equipment : " + locdetail.Equipment,
                Status = status,
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();

            var model = new { status, message };
            return Json(model);

        }
        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.ID == id);
        }



        public IActionResult getDataPerLocation(string LocationType, int LocationId, DateTime dateTime)
        {
            string status = "";


            
            if (LocationType == "LocationIDFE")
            {
               

                var v =
                _context.LocationFireExtinguishers
                .Where(a => a.Id == LocationId)
                .Where(a => a.Status == "Active") // A
                .GroupJoin(
                       _context.LocationItemDetails // B
                       .Where(a => a.Status == "Active")
                       .Where(a => a.Equipment == "FE"),
                       i => i.Id, //A key
                       p => p.HeaderId,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    )
                    .SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.Items.Id,
                              ItemName = B.Items.Name,
                              B.Items.Code,
                              B.Items.Type,
                              B.Items.Capacity,
                              CompanyName = A.i.Areas.Companies.Name,

                          }
                    );

                var detail = _context.FireExtinguisherDetails
                    .Where(a => a.FireExtinguisherHeaders.Status == "Active")
                    .Where(a => a.LocationFireExtinguisherId == LocationId)
                    .Where(a => a.FireExtinguisherHeaders.DocumentStatus != "Approved") //A
                    .GroupJoin(
                            _context.LocationFireExtinguishers // B
                            .Where(a => a.Status == "Active"),
                            i => i.LocationFireExtinguisherId, //A key
                            p => p.Id,//B key
                            (i, g) =>
                                new
                                {
                                    i, //holds A data
                                    g  //holds B data
                                }
                            )
                            .SelectMany(
                            temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                 new
                                 {

                                     id = A.i.ItemId,
                                     ItemName = A.i.Items.Name,
                                     A.i.Cylinder,
                                     A.i.Lever,
                                     A.i.Gauge,
                                     A.i.SafetySeal,
                                     A.i.Hose,
                                     A.i.Remarks,
                                     A.i.Items.Code,
                                     A.i.Items.Type,
                                     A.i.Items.Capacity,
                                     A.i.InspectedBy,
                                     A.i.ReviewedBy,
                                     A.i.NotedBy,
                                     CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.FireExtinguisherHeaderId
                                     ,detailid = A.i.Id
                                     ,A.i.ImageUrl
                                 }
                            );

                status = "success";

                var model = new
                {
                    status
                    ,

                    datadetail = detail

                    ,
                    data = v

                };
                return Json(model);
            }else if (LocationType == "LocationIDEL") {
                var v =
                _context.LocationEmergencyLights
                .Where(a => a.Id == LocationId)
                .Where(a => a.Status == "Active") // A
                .GroupJoin(
                       _context.LocationItemDetails // B
                       .Where(a => a.Status == "Active")
                       .Where(a => a.Equipment == "EL"),
                       i => i.Id, //A key
                       p => p.HeaderId,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    )
                    .SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.Items.Id,
                              ItemName = B.Items.Name,
                              B.Items.Code,
                           


                          }
                    );

                var detail = _context.EmergencyLightDetails
                    .Where(a => a.EmergencyLightHeaders.Status == "Active")
                    .Where(a => a.LocationEmergencyLightId == LocationId)
                    .Where(a => a.EmergencyLightHeaders.DocumentStatus != "Approved") //A
                    .GroupJoin(
                            _context.LocationEmergencyLights // B
                            .Where(a => a.Status == "Active"),
                            i => i.LocationEmergencyLightId, //A key
                            p => p.Id,//B key
                            (i, g) =>
                                new
                                {
                                    i, //holds A data
                                    g  //holds B data
                                }
                            )
                            .SelectMany(
                            temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                 new
                                 {

                                     id = A.i.ItemId,
                                     ItemName = A.i.Items.Name,
                                     A.i.Battery,
                                     A.i.Bulb,
                                     A.i.Usable,
                                     A.i.Remarks,
                                     A.i.Items.Code,
                                     A.i.InspectedBy,
                                     A.i.ReviewedBy,
                                     A.i.NotedBy,
                                     CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.EmergencyLightHeaderId,
                                     detailid = A.i.Id
                                     ,
                                     A.i.ImageUrl
                                 }
                            );
                status = "success";

                var model = new
                {
                    status
                    ,

                    datadetail = detail

                    ,
                    data = v

                };
                return Json(model);
            } else if (LocationType == "LocationIDFH") {
                var v =
                    _context.LocationFireHydrants
                    .Where(a => a.Id == LocationId)
                    .Where(a => a.Status == "Active") // A
                    .GroupJoin(
                           _context.LocationItemDetails // B
                           .Where(a => a.Status == "Active")
                           .Where(a => a.Equipment == "FH"),
                           i => i.Id, //A key
                           p => p.HeaderId,//B key
                           (i, g) =>
                              new
                              {
                                  i, //holds A data
                                  g  //holds B data
                              }
                        )
                        .SelectMany(
                           temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                           (A, B) =>
                              new
                              {
                                  B.Items.Id,
                                  ItemName = B.Items.Name,
                                  B.Items.Code,
                           


                              }
                        );


                
                    var detail = _context.FireHydrantDetails
                        .Where(a => a.FireHydrantHeaders.Status == "Active")
                        .Where(a => a.LocationFireHydrantId == LocationId)
                        .Where(a => a.FireHydrantHeaders.DocumentStatus != "Approved") //A
                        .GroupJoin(
                                _context.LocationFireHydrants // B
                                .Where(a => a.Status == "Active"),
                                i => i.LocationFireHydrantId, //A key
                                p => p.Id,//B key
                                (i, g) =>
                                    new
                                    {
                                        i, //holds A data
                                        g  //holds B data
                                    }
                                )
                                .SelectMany(
                                temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                                (A, B) =>
                                     new
                                     {
                                         Id = A.i.ItemId,
                                         ItemName = A.i.Items.Name,
                                         A.i.Items.Code,
                                         A.i.GlassCabinet,
                                         A.i.Hanger,
                                         A.i.Hose15,
                                         A.i.Nozzle15,
                                         A.i.Hose25,
                                         A.i.Nozzle25,
                                         A.i.SpecialTools,
                                         A.i.InspectedBy,
                                         A.i.ReviewedBy,
                                         A.i.NotedBy,
                                         A.i.Remarks,
                                         CompanyName = B.Areas.Companies.Name,
                                         HeaderId = A.i.FireHydrantHeaderId,
                                         detailid = A.i.Id
                                     ,
                                         A.i.ImageUrl
                                     }
                                );
                status = "success";
             
                var model = new
                {
                    status
                    ,

                    datadetail = detail

                    ,
                    data = v

                };
                return Json(model);
            } else if (LocationType == "LocationIDIT")
            {
                var v =
                _context.LocationInergenTanks
                .Where(a => a.Id == LocationId)
                .Where(a => a.Status == "Active") // A
                .GroupJoin(
                       _context.LocationItemDetails // B
                       .Where(a => a.Status == "Active")
                       .Where(a => a.Equipment == "IT"),
                       i => i.Id, //A key
                       p => p.HeaderId,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    )
                    .SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.Items.Id,
                              ItemName = B.Items.Name,
                              B.Items.Code,
                              Serial = B.Items.SerialNo,
                              B.Items.Capacity,
                              CompanyName = A.i.Areas.Companies.Name,

                          }
                    );
                //var xx = v.ToList();


                var detail = _context.InergenTankDetails
                    .Where(a => a.InergenTankHeaders.Status == "Active")
                   .Where(a => a.LocationInergenTankId == LocationId)
                        .Where(a => a.InergenTankHeaders.DocumentStatus != "Approved") //A
                    .GroupJoin(
                            _context.LocationInergenTanks // B
                            .Where(a => a.Status == "Active"),
                            i => i.LocationInergenTankId, //A key
                            p => p.Id,//B key
                            (i, g) =>
                                new
                                {
                                    i, //holds A data
                                    g  //holds B data
                                }
                            )
                            .SelectMany(
                            temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                 new
                                 {

                                     id = A.i.ItemId,
                                     ItemName = A.i.Items.Name,
                                     A.i.Items.Code,
                                     A.i.Cylinder,
                                 
                                     A.i.Gauge,
                                     A.i.Pressure,
                                     A.i.Hose,
                                     A.i.Remarks,

                                     Serial = A.i.Items.SerialNo,
                                     A.i.Items.Capacity,
                                     A.i.InspectedBy,
                                     A.i.ReviewedBy,
                                     A.i.NotedBy,
                                     CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.InergenTankHeaderId,
                                     detailid = A.i.Id
                                     ,
                                     A.i.ImageUrl
                                 }
                            );
               // var xy = detail.ToList();

                status = "success";

                var model = new
                {
                    status
                    ,

                    datadetail = detail

                    ,
                    data = v

                };
                return Json(model);
            }
            else
            {
                return null;
            }
           


  

                    
        }




        public IActionResult getDataLocation(int AreaId)
        {
            string status = "";
            var vFE =

                _context.LocationFireExtinguishers.Where(a => a.Status == "Active").Where(a => a.AreaId == AreaId).Select(a => new {


                   //a.Capacity,
                   a.Code,
                   a.Location,
                   //a.Type,
                   a.Id


                });
            var vEL =

               _context.LocationEmergencyLights.Where(a => a.Status == "Active").Where(a => a.AreaId == AreaId).Select(a => new {


                  
                   a.Code,
                   a.Location,
                
                   a.Id


               });
            var vFH =

               _context.LocationFireHydrants.Where(a => a.Status == "Active").Where(a => a.AreaId == AreaId).Select(a => new {



                   a.Code,
                   a.Location,

                   a.Id


               });
            var vIT =

              _context.LocationInergenTanks.Where(a => a.Status == "Active").Where(a => a.AreaId == AreaId).Select(a => new {



                  //a.Serial,
                  //a.Capacity,

                  a.Location,
                  a.Area,
                  a.LocationCylinder,

                  a.Id


              });
            status = "success";






            var model = new
            {
                status
                
               ,
                locationfe = vFE.ToList()
                ,
                locationel = vEL.ToList()
                ,
                locationfh = vFH.ToList()
                ,
                locationit = vIT.ToList()

            };
            return Json(model);
        }
    }
}


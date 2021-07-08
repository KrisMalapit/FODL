using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FODLSystem.Controllers
{
    public class FuelOilController : Controller
    {
        private readonly FODLSystemContext _context;

        public FuelOilController(FODLSystemContext context)
        {
            _context = context;
        }
       
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            this.SetCurrentBreadCrumbTitle("Fuel Oil Liquidation");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> saveSnapShot(string referenceNo,string imgData)
        {

            string filename = "";
            string status = "";
            string message = "";
            string imageurl = "";


            try
            {

                var fueloil = _context.FuelOils.Where(a => a.ReferenceNo == referenceNo).FirstOrDefault();
                fueloil.Signature = imgData;
                _context.Update(fueloil);
                _context.SaveChanges();




                imageurl = imgData;
                status = "success";
            }
            catch (Exception e)
            {

                status = "fail";
                message = e.Message;
                e.Message.WriteLog();

            }

            var model = new
            {

                message,
                status,
                imageurl
            };

            return Json(model);
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Companies/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fuel Oil Liquidation",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });
            ViewData["CreatedDate"] = DateTime.Now;
            ViewData["Signature"] = "";
            ViewData["EquipmentId"] = 0;
            ViewData["Id"] = 0;
            ViewData["LocationId"] = new SelectList(_context.Locations.Where(a => a.Status == "Active"), "Id", "List");
            return View();
        }
        public IActionResult Edit(int id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fuel Oil Liquidation",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });
            var model = _context.FuelOils.Include(a => a.Equipments).Where(a => a.Id == id).FirstOrDefault();
            ViewData["CreatedDate"] = model.CreatedDate;
            ViewData["EquipmentId"] = model.EquipmentId;
            ViewData["EquipmentName"] = model.Equipments.Name;
            ViewData["LocationId"] = new SelectList(_context.Locations.Where(a => a.Status == "Active"), "Id", "List", model.LocationId);
            ViewData["Id"] = model.Id;
            ViewData["Status"] = model.Status;
            ViewData["Signature"] = model.Signature;
            return View("Create", model);
        }
        public JsonResult SearchItem(string q)
        {
            var model = _context.Items
                .Where(a => a.Status == "Active")
                .Where(a => a.Description.ToUpper().Contains(q.ToUpper())).Select(b => new
                {
                    id = b.Id,
                    text = b.No + " | " + b.Description,

                });

            var modelItem = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelItem);
        }
        public JsonResult SearchComponent()
        {
            var model = _context.Components
                .Where(a => a.Status == "Active")
                .Select(b => new
                {
                    id = b.Id,
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
        [HttpPost]
        public IActionResult SaveForm(FuelOilViewModel fvm)
        {
            if (fvm.LubeTruckId == 0)
            {
                fvm.LubeTruckId = 1;
            }
            if (fvm.DispenserId == 0)
            {
                fvm.DispenserId = 1;
            }

            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            try
            {
                if (string.IsNullOrEmpty(fvm.ReferenceNo))
                {
                    string series_code = "FuelOil";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = "FO" + series;

                    var fo = new FuelOil
                    {
                        ReferenceNo = refno,
                        Shift = fvm.Shift,
                        EquipmentId = fvm.EquipmentId,
                        LocationId = fvm.LocationId,
                        SMR = fvm.SMR,
                        CreatedDate = DateTime.Now,
                        CreatedBy = User.Identity.GetFullName(),
                        TransactionDate = DateTime.Now.Date
                        ,DispenserId = fvm.DispenserId
                        ,LubeTruckId = fvm.LubeTruckId
                        ,Signature = fvm.Signature

                    };
                    _context.Add(fo);
                    _context.SaveChanges();
                    message = refno;

                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);

                    for (int i = 0; i < fvm.component.Length; i++)
                    {
                        var _detail = new FuelOilDetail
                        {
                            ItemId = Convert.ToInt32(fvm.no[i]),
                            ComponentId = Convert.ToInt32(fvm.component[i]),
                            VolumeQty = Convert.ToInt32(fvm.volume[i]),
                            FuelOilId = fo.Id,
                            TimeInput = DateTime.Now

                        };
                        _context.Add(_detail);
                    }

                    _context.SaveChanges();

                    status = "success";
                }
                else
                {
                    var fo = _context.FuelOils.Where(a => a.ReferenceNo == fvm.ReferenceNo).FirstOrDefault();

                    fo.Shift = fvm.Shift;
                    fo.EquipmentId = fvm.EquipmentId;
                    fo.LocationId = fvm.LocationId;
                    fo.SMR = fvm.SMR;
                    fo.CreatedDate = DateTime.Now;
                    fo.CreatedBy = User.Identity.GetFullName();
                    fo.DispenserId = fvm.DispenserId;
                    fo.LubeTruckId = fvm.LubeTruckId;
                    
                    _context.Update(fo);

                    _context.FuelOilDetails
                          .Where(a => a.FuelOilId == fo.Id)

                          .ToList().ForEach(a => a.Status = "Deleted");

                    _context.SaveChanges();

                    for (int i = 0; i < fvm.component.Length; i++)
                    {
                        var d = _context.FuelOilDetails
                            .Where(a => a.FuelOilId == fo.Id)
                            .Where(a => a.ItemId == Convert.ToInt32(fvm.no[i]))
                            .FirstOrDefault();

                        if (d == null)
                        {
                            var _detail = new FuelOilDetail
                            {
                                ItemId = Convert.ToInt32(fvm.no[i]),
                                ComponentId = Convert.ToInt32(fvm.component[i]),
                                VolumeQty = Convert.ToInt32(fvm.volume[i]),
                                FuelOilId = fo.Id,
                                TimeInput = DateTime.Now

                            };
                            _context.Add(_detail);
                        }
                        else
                        {
                            d.ItemId = Convert.ToInt32(fvm.no[i]);
                            d.ComponentId = Convert.ToInt32(fvm.component[i]);
                            d.VolumeQty = Convert.ToInt32(fvm.volume[i]);
                            d.TimeInput = DateTime.Now;
                            d.Status = "Active";
                            _context.Update(d);

                        }

                    }

                    _context.SaveChanges();
                    status = "success";
                    message = fo.ReferenceNo;

                }
            }
            catch (Exception ex)
            {

                status = "fail";
                message = ex.InnerException.Message;
            }

            var modelItem = new
            {
                status,
                message
            };
            return Json(modelItem);
        }
        public IActionResult Delete(int id)
        {
            string status = "";
            string message = "";
            try
            {
                var items = _context.FuelOils.Find(id);
                items.Status = "Delete_" + DateTime.Now.Date.Ticks;
                _context.Entry(items).State = EntityState.Modified;
                _context.SaveChanges();





                Log log = new Log
                {
                    Descriptions = "Delete FuelOil - " + id,
                    Action = "Delete"  ,
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);


                _context.SaveChangesAsync();


                status = "success";
            }
            catch (Exception e)
            {

                message = e.InnerException.ToString();

            }

            var model = new
            {

                status,
                message
            };
            return Json(model);







        }
        [HttpPost]
        public IActionResult PostForm(string referenceNo)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            try
            {
                
                    var fo = _context.FuelOils.Where(a => a.ReferenceNo == referenceNo).FirstOrDefault();

                    if (string.IsNullOrEmpty(fo.Signature))
                    {
                       

                        var model = new
                        {
                            status = "fail",
                            message = "Cannot post until form has been signed"
                         };
                        return Json(model);

                    };

                    fo.Status = "Posted";
                    _context.Update(fo);
                    _context.SaveChanges();

                    status = "success";
                    message = fo.ReferenceNo;

            }
            catch (Exception ex)
            {
                status = "fail";
                message = ex.Message;
            }

            var modelItem = new
            {
                status,
                message
            };
            return Json(modelItem);
        }
        [HttpPost]
        public ActionResult getData()
        {
            string strFilter = "";
            try
            {


                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


                for (int i = 0; i < 6; i++)
                {
                    string colval = Request.Form["columns[" + i + "][search][value]"];
                    if (colval != "")
                    {
                        colval = colval.ToUpper();
                        string colSearch = Request.Form["columns[" + i + "][name]"];



                        if (strFilter == "")
                        {

                            strFilter = colSearch + ".ToString().ToUpper().Contains(" + "\"" + colval + "\"" + ")";

                        }
                        else
                        {
                            strFilter = strFilter + " && " + colSearch + ".ToString().ToUpper().Contains(" + "\"" + colval + "\"" + ")";
                        }

                    }
                }


                if (strFilter == "")
                {
                    strFilter = "true";
                }



                int recCount =

                _context.FuelOils
                .Where(a => a.Status == "Active")

                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.FuelOils
              .Where(a => a.Status == "Active")
              .Where(strFilter)
              //.OrderBy(a => a.FileDate).ThenBy(a => a.Hour)
              .Skip(skip).Take(pageSize)
              .Select(a => new
              {
                  a.ReferenceNo,
                  a.CreatedDate,
                  a.Shift,
                  UnitNo = a.Equipments.Name,
                  Location = a.Locations.List,
                  a.SMR,
                  a.Id,
                  a.Status
              });




                bool desc = false;
                if (sortColumnDirection == "desc")
                {
                    desc = true;
                }
                v = v.OrderBy(sortColumn + (desc ? " descending" : ""));



                if (pageSize < 0)
                {
                    pageSize = recordsTotal;
                }


                var data = v;
                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        public IActionResult getDataDetails(int id)
        {
           
            var v = _context.FuelOilDetails.Where(a => a.FuelOilId == id).Where(a => a.Status == "Active").Select(a => new {
                a.ItemId,
                ItemName = a.Items.No + " | " + a.Items.Description,
                a.ComponentId,
                ComponentName = a.Components.Name,
                a.VolumeQty,
                a.Id
            });

         

            var model = new
            {
               
                data = v

            };
            return Json(model);



        }
    }
}
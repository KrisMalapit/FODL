using System;
using System.Collections.Generic;
using System.Globalization;
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
        public IActionResult signUrl(int id)
        {
            string imagedata = "";

            var fueloil = _context.FuelOilDetails.Where(a => a.Id == id).FirstOrDefault().Signature;
            if (string.IsNullOrEmpty(fueloil))
            {
                imagedata = "";
            }
            else
            {
                imagedata = fueloil;
            }
            var model = new
            {

                imagedata
            };

            return Json(model);

        }


        [HttpPost]
        public async Task<IActionResult> saveSnapShot(int id, string imgData)
        {

            string filename = "";
            string status = "";
            string message = "";
            string imageurl = "";


            try
            {

                var fueloil = _context.FuelOilDetails.Where(a => a.Id == id).FirstOrDefault();
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
            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fuel Oil Liquidation",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });

            ViewData["CreatedDate"] = DateTime.Now;
            ViewData["Signature"] = "";

            ViewData["Id"] = 0;
            ViewData["DispenserId"] = new SelectList(_context.Dispensers.Where(a => stat.Contains(a.Status)), "Id", "Name");
            ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks.Where(a => stat.Contains(a.Status)), "Id", "Description");

            ViewData["LocationId"] = new SelectList(_context.Locations.Where(a => a.Status == "Active"), "Id", "List");
            return View();
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult Edit(int id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fuel Oil Liquidation",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });
            var model = _context.FuelOils.Where(a => a.Id == id).FirstOrDefault();
            ViewData["CreatedDate"] = model.CreatedDate;
            ViewData["Id"] = model.Id;
            ViewData["Status"] = model.Status;
            ViewData["LocationId"] = new SelectList(_context.Locations.Where(a => a.Status == "Active"), "Id", "List");
            ViewData["DispenserId"] = new SelectList(_context.Dispensers.Where(a => a.Status != "Deleted"), "Id", "Name", model.DispenserId);
            ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks.Where(a => a.Status != "Deleted"), "Id", "Description", model.LubeTruckId);
            return View("Create", model);
        }
        public JsonResult SearchItem(string q)
        {
            var model = _context.Items
                .Where(a => a.Status == "Active")
                .Where(a => a.Description.ToUpper().Contains(q.ToUpper())
                || a.DescriptionLiquidation.ToUpper().Contains(q.ToUpper())
                || a.No.ToUpper().Contains(q.ToUpper())).Select(b => new
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
        public IActionResult SaveFormDetail(FuelOilViewModel fvm)
        {
            string status = "";
            string message = "";
            int refId = 0;
            try
            {
                if (fvm.Id == 0)
                {
                    FuelOilDetail fod = new FuelOilDetail
                    {
                        LocationId = fvm.LocationId,
                        EquipmentId = fvm.EquipmentId,
                        FuelOilId = fvm.FuelOilId
                        ,
                        CreatedDate = fvm.CreatedDate
                        ,
                        SMR = fvm.SMR
                    };
                    _context.Add(fod);
                    _context.SaveChanges();

                    refId = fod.Id;
                }
                else
                {
                    var fod = _context.FuelOilDetails.Find(fvm.Id);
                    fod.LocationId = fvm.LocationId;
                    fod.EquipmentId = fvm.EquipmentId;
                    fod.FuelOilId = fvm.FuelOilId;
                    fod.CreatedDate = fvm.CreatedDate;
                    fod.SMR = fvm.SMR;
                    _context.Update(fod);
                    _context.SaveChanges();

                    refId = fod.Id;
                }
                status = "success";
                message = refId.ToString();
            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;

            }


            var modelItem = new
            {
                status,
                message
            };
            return Json(modelItem);
        }


        [HttpPost]
        public IActionResult SaveForm(FuelOilViewModel fvm)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            string refid = "0";

            if (fvm.LubeTruckId == 0 & fvm.DispenserId == 0)
            {
                var model = new
                {
                    status = "fail",
                    message = "Please choose Lube Truck or Dispenser Entry"
                };
                return Json(model);
            }


            if (fvm.LubeTruckId == 0)
            {
                fvm.LubeTruckId = 1;
            }
            if (fvm.DispenserId == 0)
            {
                fvm.DispenserId = 1;
            }






            try
            {
                if (string.IsNullOrEmpty(fvm.ReferenceNo))
                {
                    string series_code = "FuelOil";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = "FLCR" + series;

                    var fo = new FuelOil
                    {
                        ReferenceNo = refno,
                        Shift = fvm.Shift,
                        CreatedDate = DateTime.Now,
                        CreatedBy = User.Identity.GetFullName(),
                        TransactionDate = DateTime.Now.Date
                        ,
                        DispenserId = fvm.DispenserId
                        ,
                        LubeTruckId = fvm.LubeTruckId


                    };

                    _context.Add(fo);
                    _context.SaveChanges();
                    message = refno;
                    refid = fo.Id.ToString();
                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);



                    status = "success";
                }
                else
                {
                    var fo = _context.FuelOils.Where(a => a.ReferenceNo == fvm.ReferenceNo).FirstOrDefault();

                    fo.Shift = fvm.Shift;
                    //fo.EquipmentId = fvm.EquipmentId;
                    //fo.LocationId = fvm.LocationId;
                    //fo.SMR = fvm.SMR;
                    fo.CreatedDate = DateTime.Now;
                    fo.CreatedBy = User.Identity.GetFullName();
                    fo.DispenserId = fvm.DispenserId;
                    fo.LubeTruckId = fvm.LubeTruckId;

                    _context.Update(fo);
                    _context.SaveChanges();
                    //_context.FuelOilDetails
                    //      .Where(a => a.FuelOilId == fo.Id)
                    //      .ToList().ForEach(a => a.Status = "Deleted");

                    //_context.SaveChanges();

                    refid = fo.Id.ToString();
                    //for (int i = 0; i < fvm.component.Length; i++)
                    //{
                    //    var d = _context.FuelOilDetails
                    //        .Where(a => a.FuelOilId == fo.Id)
                    //        //.Where(a => a.ItemId == Convert.ToInt32(fvm.no[i]))
                    //        .FirstOrDefault();

                    //    if (d == null)
                    //    {
                    //        var _detail = new FuelOilDetail
                    //        {
                    //            EquipmentId = Convert.ToInt32(fvm.no[i]),
                    //            LocationId = Convert.ToInt32(fvm.component[i]),
                    //            SMR = fvm.volume[i],
                    //            CreatedDate = DateTime.Now
                    //        };
                    //        _context.Add(_detail);
                    //    }
                    //    else
                    //    {
                    //        //d.ItemId = Convert.ToInt32(fvm.no[i]);
                    //        //d.ComponentId = Convert.ToInt32(fvm.component[i]);
                    //        //d.VolumeQty = Convert.ToInt32(fvm.volume[i]);
                    //        //d.TimeInput = DateTime.Now;
                    //        d.EquipmentId = Convert.ToInt32(fvm.no[i]);
                    //        d.LocationId = Convert.ToInt32(fvm.component[i]);
                    //        d.SMR = fvm.volume[i];
                    //        d.CreatedDate = DateTime.Now;


                    //        d.Status = "Active";
                    //        _context.Update(d);

                    //    }

                    //}


                    status = "success";
                    message = fo.ReferenceNo;

                }
            }
            catch (Exception ex)
            {

                status = "fail";
                message = ex.Message;
            }

            var modelItem = new
            {
                status,
                message,
                refid
            };
            return Json(modelItem);
        }
        [HttpPost]

        public ActionResult DeleteEquipment(int id)
        {
            string message = "";
            string status = "";
            try
            {
                FuelOilDetail item = _context.FuelOilDetails.Find(id);
                item.Status = "Deleted";
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                status = "success";
            }
            catch (Exception e)
            {

                status = "fail";
                message = e.InnerException.Message;
            }

            var res = new
            {
                message,
                status
            };
            return Json(res);
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
                    Action = "Delete",
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
            bool noSign = false;
            try
            {


                var fdetail = _context.FuelOilDetails.Where(a => a.FuelOils.ReferenceNo == referenceNo);
                foreach (var item in fdetail)
                {
                    if (string.IsNullOrEmpty(item.Signature))
                    {
                        var model = new
                        {
                            status = "fail",
                            message = "Cannot post until all form has been signed"
                        };
                        return Json(model);

                    };
                }

               


                var fo = _context.FuelOils.Where(a => a.ReferenceNo == referenceNo)
                    .FirstOrDefault();
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
              .Where(a => a.Status != "Deleted")
              .Where(strFilter)

              .Skip(skip).Take(pageSize)
              .Select(a => new
              {
                  a.ReferenceNo,
                  a.CreatedDate,
                  a.Shift,
                  LubeTruckName = a.LubeTrucks.Description,
                  DispenserName = a.Dispensers.Name,

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
        public IActionResult getDataDetails(int? id)
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


                for (int i = 0; i < 5; i++)
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

                _context.FuelOilDetails
                .Where(a => a.FuelOilId == id)
                .Where(a => a.Status == "Active")
                .Select(a => new
                {
                    EquipmentName = a.Equipments.No + " | " + a.Equipments.Name,
                    LocationName = a.Locations.List,
                    a.SMR,
                    a.CreatedDate,
                    a.Status,
                    SignStatus = a.Signature == "" ? "" : "Signed",
                    a.Id
                })
                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.FuelOilDetails
                  .Where(a => a.FuelOilId == id)
                  .Where(a => a.Status != "Deleted")
                  .Where(strFilter)
                  .Skip(skip).Take(pageSize)
                  .Select(a => new
                  {

                      EquipmentName = a.Equipments.No + " | " + a.Equipments.Name,
                      LocationName = a.Locations.List,
                      a.SMR,
                      a.CreatedDate,
                      a.Status,
                      a.Id,
                      a.LocationId,
                      a.EquipmentId,
                      SignStatus = string.IsNullOrEmpty(a.Signature) ? "" : "Signed",
                      DocumentStatus = a.FuelOils.Status

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

        //public IActionResult getDataDetails(int id)
        //{

        //    var v = _context.FuelOilDetails.Where(a => a.FuelOilId == id).Where(a => a.Status == "Active").Select(a => new {
        //        a.EquipmentId,
        //        EquipmentName = a.Equipments.No + " | " + a.Equipments.Name,
        //        a.LocationId,
        //        LocationName = a.Locations.List,
        //        a.SMR,
        //        a.Signature,
        //        a.CreatedDate,
        //        a.Status,
        //        a.Id
        //    });



        //    var model = new
        //    {

        //        data = v

        //    };
        //    return Json(model);



        //}

        public IActionResult getDataSubDetails(int id)
        {
            string strFilter = "";
            try
            {






                var v =

               _context.FuelOilSubDetails
               .Where(a => a.FuelOilDetailId == id)
              .Where(a => a.Status != "Deleted")

              .Select(a => new
              {
                  ItemId = a.Items.Id,
                  ItemName = a.Items.No + " | " + a.Items.Description,
                  ComponentId = a.Components.Id,
                  ComponentName = a.Components.Name,
                  a.VolumeQty,
                  a.Id

              });


                var model = new
                {
                    data = v.ToList()
                };




                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpPost]
        public IActionResult SaveFormSubDetail(FuelOilViewModel fvm)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            string refid = "0";





            try
            {
                var d = _context.FuelOilSubDetails.Where(a => a.FuelOilDetailId == fvm.Id).Count();
                if (d == 0)
                {
                    for (int i = 0; i < fvm.component.Length; i++)
                    {
                        var sub = new FuelOilSubDetail();
                        sub.ItemId = Convert.ToInt32(fvm.no[i]);
                        sub.ComponentId = Convert.ToInt32(fvm.component[i]);
                        sub.VolumeQty = Convert.ToInt32(fvm.volume[i]);
                        sub.TimeInput = DateTime.Now;
                        sub.FuelOilDetailId = fvm.Id;
                        _context.Add(sub);

                    }
                    _context.SaveChanges();
                    status = "success";


                }
                else
                {

                    _context.FuelOilSubDetails
                          .Where(a => a.FuelOilDetailId == fvm.Id)
                          .ToList().ForEach(a => a.Status = "Deleted");

                    //_context.SaveChanges();


                    for (int i = 0; i < fvm.component.Length; i++)
                    {
                        var sub = new FuelOilSubDetail();
                        sub.ItemId = Convert.ToInt32(fvm.no[i]);
                        sub.ComponentId = Convert.ToInt32(fvm.component[i]);
                        sub.VolumeQty = Convert.ToInt32(fvm.volume[i]);
                        sub.TimeInput = DateTime.Now;
                        sub.FuelOilDetailId = fvm.Id;
                        _context.Add(sub);

                    }
                    _context.SaveChanges();
                    status = "success";




                }
            }
            catch (Exception ex)
            {

                status = "fail";
                message = ex.Message;
            }

            var modelItem = new
            {
                status,
                message,
                refid
            };
            return Json(modelItem);
        }
    }
}
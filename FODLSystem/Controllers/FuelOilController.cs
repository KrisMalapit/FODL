using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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
        [BreadCrumb(Title = "Summary", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult Summary()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fuel Oil Liquidation",
                Url = string.Format(Url.Action("Index", "FuelOil")),
                Order = 1
            });
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
        public async Task<IActionResult> DigitalSignature(int id,int DriverId)
        {

            string filename = "";
            string status = "";
            string message = "";
            string imageurl = "";


            try
            {

                var fueloil = _context.FuelOilDetails
                    .Where(a => a.FuelOilId == id)
                    .FirstOrDefault();

                fueloil.Signature = "SIGNED";
                fueloil.DriverId = DriverId;
                _context.Update(fueloil);
                _context.SaveChanges();

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
            try
            {
                string lubeAccess = User.Identity.GetLubeAccess();
                string dispenserAccess = User.Identity.GetDispenserAccess();


                int[] lubeId = lubeAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                int[] dispenserId = dispenserAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

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
                ViewData["Status"] = "Active";
                ViewData["Id"] = 0;


                var disp = _context.Dispensers
                    .Where(a => dispenserId.Contains(a.Id))
                    .Where(a => stat.Contains(a.Status));
                List<CustomList> lstdisp = new List<CustomList>();
                foreach (var item in disp)
                {
                    CustomList tag = new CustomList()
                    {
                        Id = item.Id,
                        Text = item.Name
                    };
                    lstdisp.Add(tag);
                }
                CustomList def = new CustomList()
                {
                    Id = 0,
                    Text = "SELECT DISPENSER"
                };
                lstdisp.Add(def);
                ViewData["DispenserId"] = new SelectList(lstdisp.AsQueryable().OrderBy(c => c.Id), "Id", "Text");

                var lube = _context.LubeTrucks
                    .Where(a => lubeId.Contains(a.Id))
                    .Where(a => stat.Contains(a.Status));
                List<CustomList> lst = new List<CustomList>();
                foreach (var item in lube)
                {
                    CustomList tag = new CustomList()
                    {
                        Id = item.Id,
                        Text = item.No + " | " + item.Description
                    };
                    lst.Add(tag);
                }
                CustomList def2 = new CustomList()
                {
                    Id = 0,
                    Text = "SELECT LUBE TRUCK"
                };
                lst.Add(def2);
                ViewData["LubeTruckId"] = new SelectList(lst.AsQueryable().OrderBy(c=>c.Id), "Id", "Text");



                ViewData["LocationId"] = new SelectList(_context.Locations.Where(a => a.Status == "Active"), "Id", "List");


                ViewData["EquipmentId"] = new SelectList(_context.Equipments.Where(a => a.Status == "Active"), "Id", "No");
                ViewData["DriverId"] = new SelectList(_context.Drivers.Where(a => a.Status == "Enabled"), "ID", "Name");


                //ViewData["Id"] = 0 ;
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Index");
               
            }
            
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult Edit(int id)
        {
            string lubeAccess = User.Identity.GetLubeAccess();
            string dispenserAccess = User.Identity.GetDispenserAccess();


            int[] lubeId = lubeAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            int[] dispenserId = dispenserAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

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
            ViewData["EquipmentId"] = new SelectList(_context.Equipments.Where(a => a.Status == "Active"), "Id", "No");
            ViewData["DriverId"] = new SelectList(_context.Drivers.Where(a => a.Status == "Enabled"), "ID", "Name");
            var disp = _context.Dispensers
                 .Where(a => dispenserId.Contains(a.Id))
                 .Where(a => a.Status != "Deleted");
            List<CustomList> lstdisp = new List<CustomList>();
            foreach (var item in disp)
            {
                CustomList tag = new CustomList()
                {
                    Id = item.Id,
                    Text = item.Name
                };
                lstdisp.Add(tag);
            }
            CustomList def = new CustomList()
            {
                Id = 0,
                Text = "SELECT DISPENSER"
            };
            lstdisp.Add(def);
            ViewData["DispenserId"] = new SelectList(lstdisp.AsQueryable().OrderBy(c => c.Id), "Id", "Text", model.DispenserId);

            var lube = _context.LubeTrucks
                 .Where(a => lubeId.Contains(a.Id))
                 .Where(a => a.Status != "Deleted");
            List<CustomList> lst = new List<CustomList>();
            foreach (var item in lube)
            {
                CustomList tag = new CustomList()
                {
                    Id = item.Id,
                    Text = item.No + " | " + item.Description
                };
                lst.Add(tag);
            }
            CustomList def2 = new CustomList()
            {
                Id = 0,
                Text = "SELECT LUBE TRUCK"
            };
            lst.Add(def2);
            ViewData["LubeTruckId"] = new SelectList(lst.AsQueryable().OrderBy(c => c.Id), "Id", "Text", model.LubeTruckId);





            //ViewData["DispenserId"] = new SelectList(_context.Dispensers
            //     .Where(a => dispenserId.Contains(a.Id))
            //    .Where(a => a.Status != "Deleted"), "Id", "Name", model.DispenserId);
            //ViewData["LubeTruckId"] = new SelectList(_context.LubeTrucks
            //     .Where(a => lubeId.Contains(a.Id))
            //    .Where(a => a.Status != "Deleted"), "Id", "Description", model.LubeTruckId);







            return View("Create", model);
        }
        public JsonResult SearchItem(string q)
        {

            
            var model = _context.Items
                .Where(a => a.Status == "Active")
                .Where(a => a.DescriptionLiquidation2.ToUpper().Contains(q.ToUpper())
                ).Select(b => new
                {
                    id = b.Id,
                    text = b.No + " | " + b.DescriptionLiquidation2.ToUpper()

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


            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();

            var model = _context.Components
                .Where(a => stat.Contains(a.Status))
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
                        ,DriverId = fvm.DriverId
                        
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
                    fod.DriverId = fvm.DriverId;
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
        private bool testNetworkConnection() {
            try
            {
                Ping myPing = new Ping();
                String host = "192.168.70.231";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
               
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult SaveForm(FuelOilViewModel fvm)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            string refid = "0";
            string isNew = "true";
            int messagenumber = 0;
            DateTime dt = new DateTime();





            if (fvm.LubeTruckId <= 1 & fvm.DispenserId <= 1)
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
                
                var hasConnection = testNetworkConnection();


                if (string.IsNullOrEmpty(fvm.ReferenceNo))
                {

           //         modelBuilder.Entity<FuelOil>()
           //.HasIndex(p => new { p.TransactionDate, p.Shift, p.Status, p.LubeTruckId, p.DispenserId })
           //.IsUnique();


                    var fccount = _context.FuelOils
                        .Where(a => a.Status != "Deleted")
                        .Where(a => a.TransactionDate == DateTime.Now.Date)
                        .Where(a => a.Shift == fvm.Shift)
                        .Where(a => a.LubeTruckId == fvm.LubeTruckId)
                        .Where(a => a.DispenserId == fvm.DispenserId)
                        .Count();

                    if (fccount>0)
                    {
                        var model1 = new
                        {
                            status = "fail",
                            message = "Duplicate entry found for Lube Truck or Dispenser Entry"
                        };
                        return Json(model1);
                    }


                    string series_code = "FuelOil";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    if (hasConnection)
                    {
                        refno = "FLCR" + series;
                    }  
                    else
                    {
                        long ticks = DateTime.Now.Ticks;
                        refno = "FLCR-" + string.Format("{0:X}", ticks).ToLower();
                    }

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
                        , SourceReferenceNo = refno
                        ,
                        OriginalDate = DateTime.Now
                    };

                    _context.Add(fo);
                    _context.SaveChanges();
                    message = refno;
                    refid = fo.Id.ToString();

                    if (hasConnection)
                    {
                        string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);
                    }
                    



                    status = "success";

                    dt = DateTime.Now;
                }
                else
                {
                   

                    isNew = "false";
                    var fo = _context.FuelOils.Where(a => a.ReferenceNo == fvm.ReferenceNo).FirstOrDefault();

                    fo.Shift = fvm.Shift;
                    
                    fo.CreatedDate = fvm.CreatedDate;
                    fo.CreatedBy = User.Identity.GetFullName();
                    fo.DispenserId = fvm.DispenserId;
                    fo.LubeTruckId = fvm.LubeTruckId;

                    if (fo.Status != "Posted")
                    {
                        fo.OriginalDate = fvm.CreatedDate;
                    }
                    

                    _context.Update(fo);
                    _context.SaveChanges();
                   

                    refid = fo.Id.ToString();

                    dt = fvm.CreatedDate;

                    status = "success";
                    message = fo.ReferenceNo;

                }
            }
            catch (Exception ex)
            {

                status = "fail";
                message = ex.InnerException.Message;
                messagenumber = ex.InnerException.HResult;
            }

            var modelItem = new
            {
                status,
                message,
                refid,
                isNew,
                messagenumber,
                dt
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
                item.Status = "Deleted_" + DateTime.Now.ToString("yyyyMMddHHmmss");
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

                if (items.Status != "Active")
                {
                    var model1 = new
                    {

                        status = "fail",
                        message = "Only item with Active status can be deleted"
                    };
                    return Json(model1);
                }

                items.Status = "Deleted";
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


                var fdetail = _context.FuelOilDetails.Where(a => a.FuelOils.ReferenceNo == referenceNo).Where(a=>a.Status == "Active");
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
        public ActionResult getData(int columnCount)
        {
            string strFilter = "";
            string status = "Active,Posted,Transferred";
            string[] stat = status.Split(',').Select(n => n).ToArray();

           
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


                for (int i = 0; i < columnCount; i++)
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

                var _fuel = _context.FuelOils
                 //.Where(a => a.CreatedBy == User.Identity.GetFullName())
                 .Where(a => stat.Contains(a.Status))
                 .Where(strFilter);

                int recCount = _fuel.Count();

                if (User.Identity.GetRoleName() == "User")
                {
                    recCount = _fuel
                                   .Where(a => a.CreatedBy == User.Identity.GetFullName())
                                   .Count();
                }
               

                recordsTotal = recCount;
                int recFilter = recCount;



                var v = _context.FuelOils
               //.Where(a => a.CreatedBy == User.Identity.GetFullName())
              .Where(a => stat.Contains(a.Status))
              .Where(strFilter)
              .OrderByDescending(a => a.Id)
              //.Skip(skip).Take(pageSize)
              .Select(a => new
              {
                  a.CreatedBy,
                  a.ReferenceNo,
                  a.CreatedDate,
                  a.Shift,
                  LubeTruckName = a.LubeTrucks.No + " | " + a.LubeTrucks.Description,
                  DispenserName = a.Dispensers.Name,

                  a.Id,
                  a.Status,
                  a.SourceReferenceNo
              });
                if (User.Identity.GetRoleName() == "User")
                {
                    v = v.Where(a => a.CreatedBy == User.Identity.GetFullName()).Skip(skip).Take(pageSize);
                }
                else
                {
                    v = v.Skip(skip).Take(pageSize);
                }




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

                if (!desc)
                {
                    v.OrderByDescending(a => a.Id);
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
                    EquipmentName = a.Equipments.No ,
                    LocationName = a.Locations.List,
                    a.SMR,
                    a.CreatedDate,
                    a.Status,
                    SignStatus = a.Signature == "" ? "" : "Signed",
                    a.Id,
                    
                })
                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.FuelOilDetails
                  .Where(a => a.FuelOilId == id)
                  //.Where(a => a.Status != "Deleted")
                  .Where(a => a.Status == "Active")
                  .Where(strFilter)
                  .Skip(skip).Take(pageSize)
                  .Select(a => new
                  {

                      EquipmentName = a.Equipments.No,
                      LocationName = a.Locations.List,
                      a.SMR,
                      a.CreatedDate,
                      a.Status,
                      a.Id,
                      a.LocationId,
                      a.EquipmentId,
                      SignStatus = string.IsNullOrEmpty(a.Signature) ? "" : "Signed",
                      DocumentStatus = a.FuelOils.Status,
                      a.DriverId

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
        [HttpPost]
        public ActionResult getDataReferenceNo()
        {
            string status = "";
            try
            {

                var v = _context.FuelOils
                    .Where(a => a.Status == "Posted")
                    .Select( 
                        a => new {
                            a.Id,
                            a.ReferenceNo
                        } 
                    );




                status = "success";

                var model = new
                {
                    status
                 ,
                    data = v
                };
                return Json(model);
            }
            catch (Exception ex)
            {
                var model = new
                {
                    status = "fail"
                 ,
                    message = ex.Message
                };
                return Json(model);
            }
        }
        [HttpPost]
        public ActionResult getDataSummary(string refid)
        {
            string status = "";
            int[] fuelid;
           
            try
            {
                if (string.IsNullOrEmpty(refid))
                {

                    fuelid = _context.FuelOils
                        .Where(a => a.Status == "Posted")
                        .Select(a => a.Id).ToArray();
                }
                else
                {
                    fuelid = refid.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                }
               
                    
                

               


                var v =

               _context.FuelOilSubDetails
                  .Where(a=> fuelid.Contains(a.FuelOilDetails.FuelOilId))
                  .Where(a => a.Status == "Active")
                  .Where(a => a.FuelOilDetails.Status == "Active")
                  .Select(a => new
                  {
                      a.FuelOilDetails.FuelOils.ReferenceNo,
                      EntryType = "Negative Adjmt.",
                      ItemNo = a.Items.No,
                      PostingDate = a.FuelOilDetails.FuelOils.TransactionDate,
                      DocumentDate = a.FuelOilDetails.FuelOils.CreatedDate,
                      Qty = a.VolumeQty,
                      EquipmentCode = a.FuelOilDetails.Equipments.No,
                      a.FuelOilDetails.Locations.OfficeCode,
                      FuelCode = a.Items.TypeFuel == "OIL-LUBE" ? a.FuelOilDetails.Equipments.FuelCodeOil : a.FuelOilDetails.Equipments.FuelCodeDiesel,
                      LocationCode = "SMPC-SITE",
                      //a.FuelOilDetails.Equipments.DepartmentCode,
                      DepartmentCode = a.Items.TypeFuel == "OIL-LUBE" ? "345" : a.FuelOilDetails.Equipments.DepartmentCode,
                      a.Id,
                      a.Status,
                      a.FuelOilDetailId
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
            catch (Exception ex)
            {
                var model = new
                {
                    status = "fail"
                 ,
                    message = ex.Message
                };
                return Json(model);
            }
        }

        public IActionResult getDataSubDetails(int id)
        {
            string strFilter = "";
            string status = "Active,Transferred";
            string[] stat = status.Split(',').Select(n => n).ToArray();
            try
            {

                var v =

               _context.FuelOilSubDetails
               .Where(a => a.FuelOilDetailId == id)
              //.Where(a => a.Status != "Deleted")
              .Where(a => status.Contains(a.Status))
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
                    for (int i = 0; i < fvm.no.Length; i++)
                    {
                        if (Convert.ToInt32(fvm.no[i]) != 1 && Convert.ToInt32(fvm.no[i]) != 106 && Convert.ToInt32(fvm.no[i]) != 280)
                        {
                            if (Convert.ToInt32(fvm.component[i]) == 1)
                            {
                                string itemcode = _context.Items.Where(a => a.Id == Convert.ToInt32(fvm.no[i])).FirstOrDefault().No;
                                var model = new
                                {
                                    status = "fail",
                                    message = "Item " + itemcode + " cannot used N/A component",
                                    refid = 0
                                };
                                return Json(model);
                            }
                        }


                        if (Convert.ToInt32(fvm.no[i]) == 1 || Convert.ToInt32(fvm.no[i]) == 106 || Convert.ToInt32(fvm.no[i]) == 280)
                        {
                            fvm.component[i] = "1";
                        }



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


                    for (int i = 0; i < fvm.no.Length; i++)
                    {



                        if (Convert.ToInt32(fvm.no[i]) != 1 && Convert.ToInt32(fvm.no[i]) != 106 && Convert.ToInt32(fvm.no[i]) != 280)
                        {
                            try
                            {
                                if (Convert.ToInt32(fvm.component[i]) == 1)
                                {
                                    string itemcode = _context.Items.Where(a => a.Id == Convert.ToInt32(fvm.no[i])).FirstOrDefault().No;
                                    var model = new
                                    {
                                        status = "fail",
                                        message = "Item " + itemcode + " cannot used N/A component"

                                    };
                                    return Json(model);
                                }
                            }
                            catch (Exception)
                            {

                                string itemcode = _context.Items.Where(a => a.Id == Convert.ToInt32(fvm.no[i])).FirstOrDefault().No;
                                var model = new
                                {
                                    status = "fail",
                                    message = "Item " + itemcode + " cannot used blank component"

                                };
                                return Json(model);
                            }
                            
                        }

                        if (Convert.ToInt32(fvm.no[i]) == 1 || Convert.ToInt32(fvm.no[i]) == 106 || Convert.ToInt32(fvm.no[i]) == 280)
                        {
                            fvm.component[i] = "1";
                        }

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
        

        public class Author
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public JsonResult Checkifposted() {

            string message = "";
            string status = "";
            int cnt = 0;
            cnt = _context.FuelOils.Where(a => a.TransactionDate == DateTime.Now.Date)
                    .Where(a => a.Status == "Active").Count();

            int cntPosted = _context.FuelOils
                    .Where(a => a.Status == "Posted").Count();

            if (cnt > 0)
            {
                message = "Not all input has been posted. Continue with downloading of posted data?";
                status = "success";
            }
            else if (cntPosted == 0) 
            {
                message = "No data to be download. Please try refreshing the page.";
                status = "fail";
            }
            else
            {
                cnt = -1;
                message = "This will download all data and will be move to archived. Continue?";
                status = "success";
            }

            var model = new
            {
                status,
                message

            };

            return Json(model);

        }



        public IActionResult DownloadExcel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "fodl_" + DateTime.Now.ToString("MMddyyyy") + ".xlsx";
            try
            {


                var fodlheader = _context.FuelOils
                    .Where(a => a.Status == "Posted");
                int[] foid = fodlheader.Select(a => a.Id).ToArray();
                var fodldetail = _context.FuelOilDetails.Where(a => foid.Contains(a.FuelOilId))
                    .Where(a => a.Status == "Active");
                int[] fodetailid = fodldetail.Select(a => a.Id).ToArray();
                var fodlsub = _context.FuelOilSubDetails.Where(a => fodetailid.Contains(a.FuelOilDetailId))
                    .Where(a => a.Status == "Active");

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Header");
                    worksheet.Cell(1, 1).Value = "ReferenceNo";
                    worksheet.Cell(1, 2).Value = "Shift";
                    worksheet.Cell(1, 3).Value = "CreatedDate";
                    worksheet.Cell(1, 4).Value = "CreatedBy";
                    worksheet.Cell(1, 5).Value = "Status";
                    worksheet.Cell(1, 6).Value = "DispenserId";
                    worksheet.Cell(1, 7).Value = "LubeTruckId";
                    worksheet.Cell(1, 8).Value = "TransactionDate";
                    worksheet.Cell(1, 9).Value = "Id";
                    worksheet.Cell(1, 10).Value = "OriginalDate";
                    worksheet.Cell(1, 11).Value = "OriginalDate";
                    int index = 1;

                    foreach (var item in fodlheader)
                    {
                        worksheet.Cell(index + 1, 1).Value = item.ReferenceNo;
                        worksheet.Cell(index + 1, 2).Value = item.Shift;
                        worksheet.Cell(index + 1, 3).Value = item.CreatedDate;
                        worksheet.Cell(index + 1, 4).Value = item.CreatedBy;
                        worksheet.Cell(index + 1, 5).Value = item.Status;
                        worksheet.Cell(index + 1, 6).Value = item.DispenserId;
                        worksheet.Cell(index + 1, 7).Value = item.LubeTruckId;
                        worksheet.Cell(index + 1, 8).Value = item.TransactionDate;
                        worksheet.Cell(index + 1, 9).Value = item.Id;
                        worksheet.Cell(index + 1, 10).Value = item.OriginalDate;
                        index++;
                    }


                    IXLWorksheet worksheet2 =
                     workbook.Worksheets.Add("Detail");
                    worksheet2.Cell(1, 1).Value = "CreatedDate";
                    worksheet2.Cell(1, 2).Value = "EquipmentId";
                    worksheet2.Cell(1, 3).Value = "LocationId";
                    worksheet2.Cell(1, 4).Value = "FuelOilId";
                    worksheet2.Cell(1, 5).Value = "Status";
                    worksheet2.Cell(1, 6).Value = "SMR";
                    worksheet2.Cell(1, 7).Value = "Signature";
                    worksheet2.Cell(1, 8).Value = "Id";
                    worksheet2.Cell(1, 9).Value = "CreatedBy";
                    worksheet2.Cell(1, 10).Value = "DriverId";
                    index = 1;

                    foreach (var item in fodldetail)
                    {
                        worksheet2.Cell(index + 1, 1).Value = item.CreatedDate;
                        worksheet2.Cell(index + 1, 2).Value = item.EquipmentId;
                        worksheet2.Cell(index + 1, 3).Value = item.LocationId;
                        worksheet2.Cell(index + 1, 4).Value = item.FuelOilId;
                        worksheet2.Cell(index + 1, 5).Value = item.Status;
                        worksheet2.Cell(index + 1, 6).Value = "'" + item.SMR;
                        worksheet2.Cell(index + 1, 7).Value = item.Signature;
                        worksheet2.Cell(index + 1, 8).Value = item.Id;
                        worksheet2.Cell(index + 1, 9).Value = item.FuelOils.CreatedBy;
                        worksheet2.Cell(index + 1, 10).Value = item.DriverId;
                        index++;
                    }


                    IXLWorksheet worksheet3 =
                     workbook.Worksheets.Add("SubDetail");
                    worksheet3.Cell(1, 1).Value = "TimeInput";
                    worksheet3.Cell(1, 2).Value = "ItemId";
                    worksheet3.Cell(1, 3).Value = "ComponentId";
                    worksheet3.Cell(1, 4).Value = "VolumeQty";
                    worksheet3.Cell(1, 5).Value = "FuelOilDetailId";
                    worksheet3.Cell(1, 6).Value = "Status";
                    worksheet3.Cell(1, 7).Value = "Id";
                    worksheet3.Cell(1, 8).Value = "CreatedBy";


                    index = 1;

                    foreach (var item in fodlsub)
                    {
                        worksheet3.Cell(index + 1, 1).Value = item.TimeInput;
                        worksheet3.Cell(index + 1, 2).Value = item.ItemId;
                        worksheet3.Cell(index + 1, 3).Value = item.ComponentId;
                        worksheet3.Cell(index + 1, 4).Value = item.VolumeQty;
                        worksheet3.Cell(index + 1, 5).Value = item.FuelOilDetailId;
                        worksheet3.Cell(index + 1, 6).Value = item.Status;
                        worksheet3.Cell(index + 1, 7).Value = item.Id;
                        worksheet3.Cell(index + 1, 8).Value = item.FuelOilDetails.FuelOils.CreatedBy;

                        index++;
                    }



                    string dateticks = DateTime.Now.Ticks.ToString();


                    _context.FuelOils.Where(a => a.Status == "Posted").ToList().ForEach(a => a.Status = "Archived_" + dateticks);
                    _context.SaveChanges();
                    _context.FuelOilDetails.Where(a => foid.Contains(a.FuelOilId)).Where(a => a.Status == "Archived").ToList().ForEach(a => a.Status = "Deleted");
                    _context.SaveChanges();
                    _context.FuelOilSubDetails.Where(a => fodetailid.Contains(a.FuelOilDetailId)).Where(a => a.Status == "Archived").ToList().ForEach(a => a.Status = "Deleted");
                    _context.SaveChanges();
                   

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult UploadExcel()
        {
            string filePath = "";
            int hid = 0;
            string message = "";
            string status = "";
            IFormFile file = Request.Form.Files[0];
            try
            {

                string strFilename = Path.GetFileNameWithoutExtension(file.FileName);

                int cntRec = _context.FileUploads.Where(a => a.FileName == strFilename).Count();
                if (cntRec > 0)
                {
                    var err = new
                    {
                        status = "failed",
                        message = "File already uploaded"
                    };

                   
                    return Json(err);
                }





                //int yr = Convert.ToInt32(strFilename.Substring(9, 4));
                //int mm = Convert.ToInt32(strFilename.Substring(5, 2));
                //int dd = Convert.ToInt32(strFilename.Substring(7, 2));
                //var dtFile = new DateTime(yr, mm, dd);
                StringBuilder sb = new StringBuilder();
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    ISheet sheet2;
                    ISheet sheet3;

                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\fileuploads\", file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                           
                            sheet = hssfwb.GetSheet("Header"); //get first sheet from workbook 
                            sheet2 = hssfwb.GetSheet("Detail");
                            sheet3 = hssfwb.GetSheet("SubDetail");
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                           
                            sheet = hssfwb.GetSheet("Header"); //get first sheet from workbook 
                            sheet2 = hssfwb.GetSheet("Detail");
                            sheet3 = hssfwb.GetSheet("SubDetail");
                        }

                      
                        string strTest = UploadExcelTest(sheet, sheet2, sheet3);

                        if (strTest != "Ok")
                        {
                            var models = new
                            {
                                status = "failed",
                                message = strTest
                            };

                            filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\fileuploads\" + file.FileName);
                            System.IO.File.Delete(filePath);
                            return Json(models);
                        }

                        var transferExcel = UploadExcelFinal(sheet, sheet2, sheet3,strFilename);
                        
                    }
                }
                status = "success";
                message = "Uploaded successfully!";
            }
            catch (Exception e)
            {



                status = "failed";
                message = e.Message.ToString();
            }

            var model = new
            {
                status,
                message
            };

            filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\fileuploads\" + file.FileName);
            System.IO.File.Delete(filePath);

            return Json(model);
        }
        public string UploadExcelTest(ISheet sheet, ISheet sheet2, ISheet sheet3)
        {

            try
            {
                try
                {
                   
                    
                   

                    int rowCount = sheet.LastRowNum;
                    int cnt = 0;
                    int line = 1;
                    //header

                    List<FuelOil> svm = new List<FuelOil>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        cnt = 0;
                        line = 1;
                        IRow headerRow = sheet.GetRow(i); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        string[] clc = new string[cellCount];

                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 8)
                            {
                                break;
                            }

                            if (cnt == 2)
                            {
                                var xx = headerRow.GetCell(j).DateCellValue.ToString();
                          
                                clc[cnt] = xx;

                            }
                            else
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 8)
                            {

                                FuelOil sv = new FuelOil
                                {
                                    ReferenceNo = clc[0],
                                    Shift = clc[1],
                                    CreatedDate = Convert.ToDateTime(clc[2]),
                                    CreatedBy = clc[3],
                                    DispenserId = Convert.ToInt32(clc[5]),
                                    LubeTruckId = Convert.ToInt32(clc[6]),
                                    TransactionDate = Convert.ToDateTime(clc[7]),
                                    Status = clc[4],
                                  
                                    OriginalDate = Convert.ToDateTime(clc[9])

                                };
                                svm.Add(sv);

                                line += 1;

                            }

                        }
                    }

                    //detail
                    rowCount = sheet2.LastRowNum;
                    List<FuelOilDetail> svmDetail = new List<FuelOilDetail>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        cnt = 0;
                        line = 1;
                        IRow detailRow = sheet2.GetRow(i); //Get Detail Row
                        int cellCount = detailRow.LastCellNum;
                        string[] clc = new string[cellCount];
                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 9)
                            {
                                break;
                            }

                            if (cnt == 0)
                            {
                                var xx = detailRow.GetCell(j).DateCellValue.ToString();
                                clc[cnt] = xx;
                            }
                            else
                            {
                                clc[cnt] = detailRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 10)
                            {

                                FuelOilDetail sv = new FuelOilDetail
                                {
                                    CreatedDate = Convert.ToDateTime(clc[0]),
                                    EquipmentId = Convert.ToInt32(clc[1]),
                                    LocationId = Convert.ToInt32(clc[2]),
                                    FuelOilId = Convert.ToInt32(clc[3]),
                                    Status = clc[4],
                                    SMR = clc[5],
                                    Signature = clc[6],
                                    //OldId = Convert.ToInt32(clc[8]),
                                    //DriverId = Convert.ToInt32(clc[9]),
                                };
                                svmDetail.Add(sv);
                                line += 1;

                            }

                        }
                    }


                    //sub-detail
                    rowCount = sheet3.LastRowNum;
                    List<FuelOilSubDetail> subsvmDetail = new List<FuelOilSubDetail>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        cnt = 0;
                        line = 1;
                        IRow subdetailRow = sheet3.GetRow(i); //Get Detail Row
                        int cellCount = subdetailRow.LastCellNum;
                        string[] clc = new string[cellCount];
                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 6)
                            {
                                break;
                            }

                            if (cnt == 0)
                            {
                                var xx = subdetailRow.GetCell(j).DateCellValue.ToString();
                                clc[cnt] = xx;
                            }
                            else
                            {
                                clc[cnt] = subdetailRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 6)
                            {

                                FuelOilSubDetail sv = new FuelOilSubDetail
                                {
                                    TimeInput = Convert.ToDateTime(clc[0]),
                                    ItemId = Convert.ToInt32(clc[1]),
                                    ComponentId = Convert.ToInt32(clc[2]),
                                    VolumeQty = Convert.ToInt32(clc[3]),
                                    FuelOilDetailId = Convert.ToInt32(clc[4]),
                                    Status = clc[5],

                                };
                                subsvmDetail.Add(sv);
                                line += 1;

                            }

                        }
                    }
                    return "Ok";
                }
                catch (Exception e)
                {

                    return e.ToString();
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public string UploadExcelFinal(ISheet sheet, ISheet sheet2, ISheet sheet3,string strFilename)
        {
            
            
            try
            {
                try
                {
                    int rowCount = sheet.LastRowNum;
                    int cnt = 0;
                    int line = 1;

                    //header
                    List<FuelOil> svm = new List<FuelOil>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        string createdBy = "";
                        cnt = 0;
                        line = 1;
                        IRow headerRow = sheet.GetRow(i); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        string[] clc = new string[cellCount];

                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 9)
                            {
                                break;
                            }

                            if (cnt == 2)
                            {
                                var xx = headerRow.GetCell(j).DateCellValue.ToString();

                                clc[cnt] = xx;

                            }
                            else
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 9)
                            {
                                string series_code = "FuelOil";
                                string series = new NoSeriesController(_context).GetNoSeries(series_code);
                                string refno = "FLCR" + series;


                                    FuelOil sv = new FuelOil
                                    {
                                    ReferenceNo = refno,
                                    Shift = clc[1],
                                    CreatedDate = Convert.ToDateTime(clc[2]),
                                    CreatedBy = clc[3],
                                    DispenserId = Convert.ToInt32(clc[5]),
                                    LubeTruckId = Convert.ToInt32(clc[6]),
                                    TransactionDate = Convert.ToDateTime(clc[7]),
                                    Status = clc[4],
                                    TransferDate = DateTime.Now,
                                    TransferredBy = User.Identity.GetFullName(),
                                    OldId = Convert.ToInt32(clc[8]),
                                   
                                    SourceReferenceNo = clc[0],
                                        OriginalDate = Convert.ToDateTime(clc[9])

                                    };
                                //svm.Add(sv);
                                _context.FuelOils.Add(sv);
                                _context.SaveChanges();
                                string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);
                                line += 1;

                            }

                        }
                    }
                   

                    //detail
                    rowCount = sheet2.LastRowNum;
                    List<FuelOilDetail> svmDetail = new List<FuelOilDetail>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        cnt = 0;
                        line = 1;
                        IRow detailRow = sheet2.GetRow(i); //Get Detail Row
                        int cellCount = detailRow.LastCellNum;
                        string[] clc = new string[cellCount];
                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 10)
                            {
                                break;
                            }

                            if (cnt == 0)
                            {
                                var xx = detailRow.GetCell(j).DateCellValue.ToString();
                                clc[cnt] = xx;
                            }
                            else
                            {
                                clc[cnt] = detailRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 10)
                            {
                                int hId = _context.FuelOils.Where(a => a.OldId == Convert.ToInt32(clc[3]))
                                    .Where(a => a.CreatedBy == clc[8])
                                    .FirstOrDefault().Id;
                                //kcms
                                FuelOilDetail sv = new FuelOilDetail
                                {
                                    CreatedDate = Convert.ToDateTime(clc[0]),
                                    EquipmentId = Convert.ToInt32(clc[1]),
                                    LocationId = Convert.ToInt32(clc[2]),
                                    Status = clc[4],
                                    SMR = clc[5],
                                    Signature = clc[6],
                                    OldId = Convert.ToInt32(clc[7]), //FuelOilId
                                    FuelOilId = hId,
                                    DriverId = Convert.ToInt32(clc[9])
                                };
                                //svmDetail.Add(sv);
                                _context.FuelOilDetails.Add(sv);
                                line += 1;

                            }

                        }
                    }
                    _context.SaveChanges();

                    //sub-detail
                    rowCount = sheet3.LastRowNum;
                    List<FuelOilSubDetail> subsvmDetail = new List<FuelOilSubDetail>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        cnt = 0;
                        line = 1;
                        IRow subdetailRow = sheet3.GetRow(i); //Get Detail Row
                        int cellCount = subdetailRow.LastCellNum;
                        string[] clc = new string[cellCount];
                        for (int j = 0; j < (cellCount); j++)
                        {
                            if (line > 8)
                            {
                                break;
                            }

                            if (cnt == 0)
                            {
                                var xx = subdetailRow.GetCell(j).DateCellValue.ToString();
                                clc[cnt] = xx;
                            }
                            else
                            {
                                clc[cnt] = subdetailRow.GetCell(j).ToString();
                            }

                            cnt += 1;
                            if (cnt == 8)
                            {
                                int hId = _context.FuelOilDetails
                                    .Where(a => a.OldId == Convert.ToInt32(clc[4]))
                                    .Where(a => a.FuelOils.CreatedBy == clc[7])
                                    .FirstOrDefault().Id;

                                FuelOilSubDetail sv = new FuelOilSubDetail
                                {
                                    TimeInput = Convert.ToDateTime(clc[0]),
                                    ItemId = Convert.ToInt32(clc[1]),
                                    ComponentId = Convert.ToInt32(clc[2]),
                                    VolumeQty = Convert.ToInt32(clc[3]),
                                    FuelOilDetailId = hId,
                                    Status = clc[5],
                                    OldId = Convert.ToInt32(clc[6])

                                };
                                //subsvmDetail.Add(sv);
                                _context.FuelOilSubDetails.Add(sv);
                                line += 1;
                            }

                        }
                    }

                    Log log = new Log
                    {
                        Action = "Upload",
                        CreatedDate = DateTime.Now,
                        Descriptions = "Upload Excel File " + strFilename,
                        Status = "success",
                        UserId = User.Identity.GetUserId().ToString()
                    };

                    _context.Add(log);


                    FileUpload fu = new FileUpload
                    {
                        FileName = strFilename,
                        UploadDate = DateTime.Now,
                        UploadBy = User.Identity.GetFullName()
                    };
                    _context.Add(fu);




                    _context.SaveChanges();

                    return "Ok";
                }
                catch (Exception e)
                {

                    return e.ToString();
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }
        public IActionResult DownloadCSV()
        {

            string fileName = "fodl_"+DateTime.Now.ToString("MMddyyyy") +".csv";
            try
            {
                var fodlheader = _context.FuelOils
                    .Where(a => a.Status == "Posted");
                int[] foid = fodlheader.Select(a => a.Id).ToArray();
                var fodldetail = _context.FuelOilDetails.Where(a=> foid.Contains(a.FuelOilId))
                    .Where(a => a.Status == "Active");
                int[] fodetailid = fodldetail.Select(a => a.Id).ToArray();
                var fodlsub = _context.FuelOilSubDetails.Where(a => fodetailid.Contains(a.FuelOilDetailId))
                    .Where(a => a.Status == "Active");

                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("ReferenceNo,Shift,CreatedDate,CreatedBy,Status,DispenserId,LubeTruckId,TransactionDate");
                    foreach (var author in fodlheader)
                    {
                        stringBuilder.AppendLine($"{author.ReferenceNo},{ author.Shift},{ author.CreatedDate}," +
                            $"{author.CreatedBy }, {author.Status}, {author.DispenserId},{author.LubeTruckId },{author.TransactionDate }");
                    }
                    return File(Encoding.UTF8.GetBytes
                    (stringBuilder.ToString()), "text/csv", fileName);
                }
                catch
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}
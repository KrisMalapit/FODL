using System;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using FODLSystem.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using FODLSystem.Models.View_Model;
using Microsoft.EntityFrameworkCore;

namespace FODLSystem.Controllers
{
    public class InergenTanksController : Controller
    {
        private readonly SEMSystemContext _context;
        public InergenTanksController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Inergen Tanks");
            return View();
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: InergenTank/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Inergen Tank",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });
            ViewData["ID"] = 0;
            ViewData["Title"] = "Create";
            ViewData["AreaId"] = new SelectList(_context.Areas, "ID", "Name");
            ViewData["CreatedAt"] = DateTime.Now.Date.ToString("MM-dd-yyyy");
            return View();
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: InergenTank/Create
        public IActionResult Edit(int id)
        {
            var model = _context.InergenTankHeaders.Where(a => a.Id == id).FirstOrDefault();

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Inergen Tank",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });


            ViewData["ID"] = id;
            ViewData["DocumentStatus"] = model.DocumentStatus;
            //ViewData["AreaId"] = new SelectList(_context.Areas, "ID", "Name", model.AreaId);
            ViewData["Area"] = _context.Areas.Find(model.AreaId).Name;

            // ViewData["LocationId"] = new SelectList(_context.LocationInergenTanks, "Id", "Name", model.LocationInergenTankId);
            //ViewData["Location"] = _context.LocationInergenTanks.Find(model.LocationInergenTankId).Area;

            ViewData["Title"] = "Edit";
            ViewData["CreatedAt"] = model.CreatedAt.ToString("MM-dd-yyyy");
            ViewData["Company"] = _context.Areas.Include(a => a.Companies).Where(a => a.ID == model.AreaId).FirstOrDefault().Companies.Name;
            return View("Create", model);
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


                for (int i = 0; i < 2; i++)
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



                //int recCount =

                //     _context.InergenTankHeaders
                //     .Select(a => new
                //     {
                //         a.CreatedAt,
                //         CompanyName = a.Locations.Areas.Companies.Name,
                //         AreaName = a.Locations.Areas.Name
                //          ,
                //         Location = a.Locations.Area
                //         , a.Status,
                //         a.DocumentStatus,
                //         a.ReferenceNo
                //     })
                //    .Where(a => a.Status == "Active")
                //    .Where(strFilter)
                //    .Count();

                int recCount = _context.InergenTankHeaders //A
                   .GroupJoin(
                      _context.Areas // B
                    ,
                      i => i.AreaId, //A key
                      p => p.ID,//B key
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
                             A.i.CreatedAt,
                             CompanyName = B.Companies.Name,
                             AreaName = B.Name
                                      //,
                                      //a.Locations.Location
                                      ,
                             A.i.Status,
                             A.i.DocumentStatus,
                             A.i.ReferenceNo,
                             A.i.CreatedBy
                         }
                   ).Where(a => a.Status == "Active")
                    .Where(strFilter)
                    .Count();


                recordsTotal = recCount;
                int recFilter = recCount;


                var v =

             _context.InergenTankHeaders //A
                   .GroupJoin(
                      _context.Areas // B
                    ,
                      i => i.AreaId, //A key
                      p => p.ID,//B key
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
                             A.i.CreatedAt,
                             CompanyName = B.Companies.Name,
                             AreaName = B.Name
                                      //,
                                      //a.Locations.Location
                                      ,
                             A.i.Status,
                             A.i.DocumentStatus,
                             A.i.ReferenceNo,
                             A.i.Id,
                             A.i.CreatedBy
                         }
                   )
              .Where(strFilter)
              .Where(a => a.Status == "Active")
              .Skip(skip).Take(pageSize)
              ;

                //string strSql = v.ToSql();
                bool desc = false;
                if (sortColumnDirection == "desc")
                {
                    desc = true;
                }
                v = v.OrderBy(sortColumn + (desc ? " descending" : ""));

                var data = v;
                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public IActionResult getDataPerArea(int LocationId, DateTime dateTime)
        {
            string status = "";
            int headerid = 0;
            //var v =

            //    _context.LocationInergenTanks
            //    .Where(a => a.Status == "Active")
            //    .Where(a => a.AreaId == AreaId)
            //    .Select(a => new {
            //        Location = a.Areas.Name,
            //        a.Areas.Code,
            //        a.Id,
            //        CompanyName = a.Areas.Companies.Name
            //    });



            //var detail = _context.InergenTankDetails
            //    .Where(a => a.InergenTankHeaders.Status == "Active")
            //        .Where(a => a.InergenTankHeaders.AreaId == AreaId)
            //        .Where(a => a.InergenTankHeaders.CreatedAt == dateTime) //A
            //    //.Where(a => a.InergenTankHeaderId == id) //A
            //        .GroupJoin(
            //           _context.LocationInergenTanks // B
            //           .Where(a => a.Status == "Active"),
            //           i => i.LocationInergenTankId, //A key
            //           p => p.Id,//B key
            //           (i, g) =>
            //              new
            //              {
            //                  i, //holds A data
            //                  g  //holds B data
            //              }
            //        )
            //        .SelectMany(
            //           temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
            //           (A, B) =>
            //              new
            //              {

            //                  Id = A.i.LocationInergenTankId,
            //                  A.i.Cylinder,
            //                  A.i.Gauge,
            //                  A.i.Hose,
            //                  A.i.Pressure,
            //                  A.i.Remarks,
            //                  //B.Location,
            //                  //B.Code,
            //                  A.i.InspectedBy,
            //                  A.i.ReviewedBy,
            //                  A.i.NotedBy,

            //                  Location = B.Areas.Name,

            //                  Code = B.Areas.Code,


            //                  CompanyName = B.Areas.Companies.Name,
            //                  HeaderId = A.i.InergenTankHeaderId
            //              }
            //        );

            var v =

               _context.LocationInergenTanks
                    .Where(a => a.Id == LocationId)
                    .Where(a => a.Status == "Status") //A
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
                              //A.i.Location,

                              B.Items.Capacity,
                              Serial = B.Items.SerialNo,
                              B.Items.Code,
                              ItemName = B.Items.Name,
                              CompanyName = A.i.Areas.Companies.Name,
                          }
                    );

            var detail = _context.InergenTankDetails
                    .Where(a => a.InergenTankHeaders.Status == "Active")
                    .Where(a => a.LocationInergenTankId == LocationId)
                    .Where(a => a.InergenTankHeaders.CreatedAt == dateTime) //A
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

                                     A.i.ItemId,
                                     ItemName = A.i.Items.Name,
                                     A.i.Cylinder,
                                     A.i.Gauge,
                                     A.i.Pressure,
                                     A.i.Hose,

                                     A.i.Remarks,

                                     Serial = A.i.Items.SerialNo,
                                     A.i.Items.Capacity,
                                     A.i.Items.Code,
                                     A.i.InspectedBy,
                                     A.i.ReviewedBy,
                                     A.i.NotedBy,
                                     CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.InergenTankHeaderId,
                                     A.i.ImageUrl,
                                     A.i.Id
                                 }
                            );

            status = "success";

            var model = new
            {
                status,

                datadetail = detail
                ,
                data = v

            };
            return Json(model);
        }
        [HttpPost]
        public IActionResult SaveData(FireExtinguisherViewModel[] item)
        {
            int headerId = 0;
            string status = "";
            string message = "";

            string series = "";
            string refno = "";
            //string series_code = "INERGENTANK";
            //series = new NoSeriesController(_context).GetNoSeries(series_code);
            //refno = "IT" + series;


            try
            {
                //var _header = _context.InergenTankHeaders.Where(a => a.Status == "Active")
                //      //.Where(a => a.AreaId == item[0].AreaId)
                //    .Where(a => a.LocationInergenTankId == item[0].LocationInergenTankId)
                //    .Where(a => a.CreatedAt == DateTime.Now.Date);
                var _header = _context.InergenTankHeaders
                    .Where(a=>a.AreaId == item[0].AreaId)
                    .Where(a => a.Status == "Active")
                    .Where(a => a.DocumentStatus != "Approved");
                if (_header.Count() == 0)
                {
                    var comp = _context.LocationInergenTanks.Include(a => a.Areas.Companies).Where(a => a.Id == item[0].LocationInergenTankId)
                        .FirstOrDefault().Areas.Companies.Code;

                    if (comp == "SCPC")
                    {
                        comp = "SC";
                    }
                    else
                    {
                        comp = "SL";
                    }
                    string series_code = comp + "INERGENTANK";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = comp + "IT" + series;


                    InergenTankHeader header = new InergenTankHeader
                    {
                        ReferenceNo = refno,
                        AreaId = item[0].AreaId,
                        CreatedAt = DateTime.Now.Date,
                        CreatedBy = User.Identity.GetFullName()
                    };
                    _context.Add(header);
                    _context.SaveChanges();
                    headerId = header.Id;

                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);



                    foreach (var detail in item)
                    {
                        var _detail = new InergenTankDetail
                        {
                           
                            ItemId = detail.ItemId,

                            Cylinder = detail.Cylinder == "true" ? 1 : 0,
                            Gauge = detail.Gauge == "true" ? 1 : 0,
                            Hose = detail.Hose == "true" ? 1 : 0,
                            Pressure = detail.Pressure == "true" ? 1 : 0,

                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now.Date,
                            InergenTankHeaderId = headerId,
                            Remarks = detail.Remarks,
                            InspectedBy = detail.InspectedBy,
                            ReviewedBy = detail.ReviewedBy,
                            NotedBy = detail.NotedBy,
                            LocationInergenTankId = detail.LocationInergenTankId,
                        };

                        _context.Add(_detail);
                    }
                }
                else
                {
                    //headerId = item[0].ID;
                    headerId = _header.FirstOrDefault().Id;
                    foreach (var detail in item)
                    {
                        var d = _context.InergenTankDetails
                            .Where(a => a.InergenTankHeaderId == headerId)
                            .Where(a => a.LocationInergenTankId == detail.LocationInergenTankId)
                            .Where(a => a.ItemId == detail.ItemId)
                            .FirstOrDefault();

                        if (d == null)
                        {

                            var _detail = new InergenTankDetail
                            {
                               
                                ItemId = detail.ItemId,
                                Cylinder = detail.Cylinder == "true" ? 1 : 0,
                                Gauge = detail.Gauge == "true" ? 1 : 0,
                                Hose = detail.Hose == "true" ? 1 : 0,
                                Pressure = detail.Pressure == "true" ? 1 : 0,

                                CreatedAt = DateTime.Now.Date,
                                UpdatedAt = DateTime.Now,
                                InergenTankHeaderId = headerId,
                                Remarks = detail.Remarks,
                                InspectedBy = detail.InspectedBy,
                                ReviewedBy = detail.ReviewedBy,
                                NotedBy = detail.NotedBy,
                                LocationInergenTankId = detail.LocationInergenTankId,
                            };

                            _context.Add(_detail);


                        }
                        else
                        {
                            d.Cylinder = detail.Cylinder == "true" ? 1 : 0;
                            d.Gauge = detail.Gauge == "true" ? 1 : 0;
                            d.Hose = detail.Hose == "true" ? 1 : 0;
                            d.Pressure = detail.Pressure == "true" ? 1 : 0;

                            d.UpdatedAt = DateTime.Now;
                            d.UpdatedBy = User.Identity.GetUserName();
                            d.InergenTankHeaderId = headerId;
                            d.Remarks = detail.Remarks;
                            d.InspectedBy = detail.InspectedBy;
                            d.ReviewedBy = detail.ReviewedBy;
                            d.NotedBy = detail.NotedBy;
                            d.LocationInergenTankId = detail.LocationInergenTankId;
                            _context.Update(d);
                        }
                        _context.SaveChanges();
                    }
                }





                _context.SaveChanges();
                status = "success";

            }
            catch (Exception ex)
            {

                status = "failed";
                message = ex.InnerException.Message;
            }





            var model = new
            {
                status
                ,
                message,
                ReferenceId = headerId
            };
            return Json(model);
        }
        [HttpPost]
        public ActionResult EditData(FireExtinguisherViewModel[] item)
        {
            string status = "success";
            string message = "";

            try
            {
                int headerId = item[0].ID;
                foreach (var detail in item)
                {
                    var d = _context.InergenTankDetails
                        .Where(a => a.InergenTankHeaderId == headerId)
                       
                        .Where(a => a.ItemId == detail.ItemId)
                        .FirstOrDefault();

                    if (d == null)
                    {

                        var _detail = new InergenTankDetail
                        {
                          
                            ItemId = detail.ItemId,
                            Cylinder = detail.Cylinder == "true" ? 1 : 0,
                            Gauge = detail.Gauge == "true" ? 1 : 0,
                            Hose = detail.Hose == "true" ? 1 : 0,
                            Pressure = detail.Pressure == "true" ? 1 : 0,

                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now,
                            InergenTankHeaderId = headerId,
                            Remarks = detail.Remarks,
                            //InspectedBy = detail.InspectedBy,
                            //ReviewedBy = detail.ReviewedBy,
                            //NotedBy = detail.NotedBy
                        };

                        _context.Add(_detail);


                    }
                    else
                    {
                        d.Cylinder = detail.Cylinder == "true" ? 1 : 0;
                        d.Gauge = detail.Gauge == "true" ? 1 : 0;
                        d.Hose = detail.Hose == "true" ? 1 : 0;
                        d.Pressure = detail.Pressure == "true" ? 1 : 0;

                        d.UpdatedAt = DateTime.Now;
                        d.UpdatedBy = User.Identity.GetUserName();
                        d.InergenTankHeaderId = headerId;
                        d.Remarks = detail.Remarks;
                        //d.InspectedBy = detail.InspectedBy;
                        //d.ReviewedBy = detail.ReviewedBy;
                        //d.NotedBy = detail.NotedBy;
                        _context.Update(d);
                    }
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message;
            }


            var model = new
            {
                status
               ,
                message

            };
            return Json(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string status = "";
            string message = "";
            try
            {


                var mddaf = _context.InergenTankHeaders.Find(id);
                mddaf.Status = "Deleted_" + DateTime.Now.Ticks.ToString();
                _context.Update(mddaf);


                Log log = new Log();
                log.Descriptions = "Delete InergenTankHeader Id : " + id;
                log.Action = "Delete";
                log.Status = "Success";
                log.UserId = User.Identity.GetUserName();
                log.CreatedDate = DateTime.Now;
                _context.Add(log);


                _context.SaveChanges();

                status = "success";


            }
            catch (Exception e)
            {
                status = "failed";
                message = e.Message;

            }


            var model = new { status, message };
            return Json(model);
        }
        [HttpPost]
        public ActionResult getDataDetails(int id)
        {
            string status = "";
            //var v = _context.InergenTankDetails.Where(a => a.InergenTankHeaderId == id) //A
            //        .GroupJoin(
            //           _context.LocationInergenTanks // B
            //           .Where(a => a.Status == "Active"),
            //           i => i.LocationInergenTankId, //A key
            //           p => p.Id,//B key
            //           (i, g) =>
            //              new
            //              {
            //                  i, //holds A data
            //                  g  //holds B data
            //              }
            //        )
            //        .SelectMany(
            //           temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
            //           (A, B) =>
            //              new
            //              {

            //                  Id = A.i.LocationInergenTankId,
            //                  A.i.Cylinder,
            //                  A.i.Gauge,
            //                  A.i.Hose,
            //                  A.i.Pressure,
            //                  A.i.Remarks,
            //                  //B.Location,
            //                  //B.Code,

            //                  A.i.InspectedBy,
            //                  A.i.ReviewedBy,
            //                  A.i.NotedBy,
            //                  Location = B.Areas.Name,

            //                  Code = B.Areas.Code,


            //                  CompanyName = B.Areas.Companies.Name
            //              }
            //        );
            var v = _context.InergenTankDetails
                  .Where(a => a.InergenTankHeaders.Status == "Active")
                  .Where(a => a.InergenTankHeaderId == id) //A
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
                          temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                          (A, B) =>
                               new
                               {

                                   A.i.ItemId,
                                   ItemName = A.i.Items.Name,
                                   A.i.Cylinder,
                                   A.i.Items.Code,
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
                                   A.i.InergenTankHeaders.DocumentStatus,
                                   A.i.ImageUrl,
                                   A.i.Id,
                                   B.Location
                               }
                          );
            status = "success";
            var xxx = v.ToList();
            var model = new
            {
                status
                ,
                data = v

            };
            return Json(model);
        }
    }
}
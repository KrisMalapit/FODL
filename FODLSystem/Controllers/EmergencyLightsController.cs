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
    public class EmergencyLightsController : Controller
    {
        private readonly SEMSystemContext _context;
        public EmergencyLightsController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Emergency Lights");
            return View();
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: EmergencyLight/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Emergency Light",
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
        // GET: EmergencyLight/Create
        public IActionResult Edit(int id)
        {
            var model = _context.EmergencyLightHeaders.Where(a => a.Id == id).FirstOrDefault();

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Emergency Light",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });


            ViewData["ID"] = id;
            ViewData["DocumentStatus"] = model.DocumentStatus;
            ViewData["Area"] = _context.Areas.Find(model.AreaId).Name;
           
            //ViewData["Location"] = _context.LocationEmergencyLights.Find(model.LocationEmergencyLightId).Location;

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

                //     _context.EmergencyLightHeaders
                //     .Select(a => new
                //     {
                //         a.CreatedAt,
                //         CompanyName = a.Locations.Areas.Companies.Name,
                //         AreaName = a.Locations.Areas.Name
                //           ,
                //         a.Locations.Location
                //         ,
                //         a.Status,
                //         a.DocumentStatus,
                //         a.ReferenceNo
                //     })
                //    .Where(a => a.Status == "Active")
                //    .Where(strFilter)
                //    .Count();

                int recCount = _context.EmergencyLightHeaders //A
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


                //  var v =

                //_context.EmergencyLightHeaders
                // .Select(a => new
                // {
                //     a.CreatedAt,
                //     CompanyName = a.Locations.Areas.Companies.Name,
                //     AreaName = a.Locations.Areas.Name
                //       ,
                //     a.Locations.Location
                //    ,
                //     a.Id
                //     ,
                //     a.Status,
                //     a.DocumentStatus,
                //     a.ReferenceNo

                // })
                //.Where(strFilter)
                //.Where(a => a.Status == "Active")
                //.Skip(skip).Take(pageSize);

                var v = _context.EmergencyLightHeaders //A
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
                             A.i.ReferenceNo
                             ,A.i.Id,
                             A.i.CreatedBy
                         }
                   ).Where(a => a.Status == "Active")
                    .Where(strFilter)
                   .Skip(skip).Take(pageSize);


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

            //    _context.LocationEmergencyLights
            //    .Where(a => a.Status == "Active")
            //    .Where(a => a.AreaId == AreaId)
            //    .Select(a => new {
            //        a.Location,

            //        a.Code,
            //        a.Id,
            //        CompanyName = a.Areas.Companies.Name
            //    });



            //var detail = _context.EmergencyLightDetails
            //     .Where(a => a.EmergencyLightHeaders.Status == "Active")
            //        .Where(a => a.EmergencyLightHeaders.AreaId == AreaId)
            //        .Where(a => a.EmergencyLightHeaders.CreatedAt == dateTime) //A
            //    //.Where(a => a.EmergencyLightHeaderId == id) //A
            //                   .GroupJoin(
            //                      _context.LocationEmergencyLights // B
            //                      .Where(a => a.Status == "Active"),
            //                      i => i.LocationEmergencyLightId, //A key
            //                      p => p.Id,//B key
            //                      (i, g) =>
            //                         new
            //                         {
            //                             i, //holds A data
            //                  g  //holds B data
            //              }
            //                   )
            //                   .SelectMany(
            //                      temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
            //                      (A, B) =>
            //                         new
            //                         {

            //                             Id = A.i.LocationEmergencyLightId,
            //                             A.i.Battery,
            //                             A.i.Bulb,
            //                             A.i.Usable,

            //                             A.i.Remarks,
            //                             B.Location,
            //                             B.Code,
            //                             A.i.InspectedBy,
            //                             A.i.ReviewedBy,
            //                             A.i.NotedBy,

            //                             CompanyName = B.Areas.Companies.Name,
            //                             HeaderId = A.i.EmergencyLightHeaderId
            //                         }
            //                   );

            var v =

               _context.LocationEmergencyLights
                    .Where(a => a.Id == LocationId)
                    .Where(a => a.Status == "Status") //A
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
                       temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              //A.i.Location,
                              B.Items.Code,
                              ItemName = B.Items.Name,
                              CompanyName = A.i.Areas.Companies.Name,
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
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                 new
                                 {

                                     A.i.ItemId,
                                     ItemName = A.i.Items.Name,
                                     A.i.Battery,
                                     A.i.Bulb,
                                     A.i.Usable,
                                     A.i.Remarks,
                                     A.i.Items.Code,

                                     A.i.InspectedBy,
                                     A.i.ReviewedBy,
                                     A.i.NotedBy,
                                     //CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.EmergencyLightHeaderId,
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
        public IActionResult SaveData(EmergencyLightViewModel[] item)
        {
            int headerId = 0;
            string status = "";
            string message = "";

            string series = "";
            string refno = "";
            //string series_code = "EMERGENCYLIGHT";
            //series = new NoSeriesController(_context).GetNoSeries(series_code);
            //refno = "EL" + series;


            try
            {
                //var _header = _context.EmergencyLightHeaders
                //    .Where(a => a.Status == "Active")
                //    .Where(a => a.LocationEmergencyLightId == item[0].LocationEmergencyLightId)
                //    .Where(a => a.CreatedAt == DateTime.Now.Date);

                var _header = _context.EmergencyLightHeaders
                     .Where(a => a.AreaId == item[0].AreaId)
                    .Where(a => a.Status == "Active")
                    .Where(a => a.DocumentStatus != "Approved");

                if (_header.Count() == 0)
                {

                    var comp = _context.LocationEmergencyLights.Include(a => a.Areas.Companies).Where(a => a.Id == item[0].LocationEmergencyLightId).FirstOrDefault()
                      .Areas.Companies.Code;
                    if (comp == "SCPC")
                    {
                        comp = "SC";
                    }
                    else
                    {
                        comp = "SL";

                    }
                    string series_code = comp + "EMERGENCYLIGHT";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = comp + "EL" + series;


                    EmergencyLightHeader header = new EmergencyLightHeader
                    {
                        //header.AreaId = item[0].AreaId;
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
                        var _detail = new EmergencyLightDetail
                        {
                            ItemId = detail.ItemId,
                            Battery = detail.Battery == "true" ? 1 : 0,
                            Bulb = detail.Bulb == "true" ? 1 : 0,
                            Usable = detail.Usable == "true" ? 1 : 0,
                          
                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now.Date,
                            EmergencyLightHeaderId = headerId,
                            Remarks = detail.Remarks,

                            InspectedBy = detail.InspectedBy,
                            ReviewedBy = detail.ReviewedBy,
                            NotedBy = detail.NotedBy,
                            LocationEmergencyLightId = detail.LocationEmergencyLightId


                        };

                        _context.Add(_detail);
                    }
                }
                else
                {
                    headerId = _header.FirstOrDefault().Id;

                    foreach (var detail in item)
                    {
                        var d = _context.EmergencyLightDetails
                            .Where(a => a.EmergencyLightHeaderId == headerId)
                            .Where(a => a.LocationEmergencyLightId == detail.LocationEmergencyLightId)
                            .Where(a => a.ItemId == detail.ItemId)
                            .FirstOrDefault();

                        if (d == null)
                        {

                            var _detail = new EmergencyLightDetail
                            {
                                
                                ItemId = detail.ItemId,
                                Battery = detail.Battery == "true" ? 1 : 0,
                                Bulb = detail.Bulb == "true" ? 1 : 0,
                                Usable = detail.Usable == "true" ? 1 : 0,
                               
                                CreatedAt = DateTime.Now.Date,
                                UpdatedAt = DateTime.Now,
                                EmergencyLightHeaderId = headerId,
                                Remarks = detail.Remarks,

                                InspectedBy = detail.InspectedBy,
                                ReviewedBy = detail.ReviewedBy,
                                NotedBy = detail.NotedBy,
                                LocationEmergencyLightId = detail.LocationEmergencyLightId,

                            };

                            _context.Add(_detail);


                        }
                        else
                        {
                            d.Battery = detail.Battery == "true" ? 1 : 0;
                            d.Bulb = detail.Bulb == "true" ? 1 : 0;
                            d.Usable = detail.Usable == "true" ? 1 : 0;
                            
                            d.UpdatedAt = DateTime.Now;
                            d.UpdatedBy = User.Identity.GetUserName();
                            d.EmergencyLightHeaderId = headerId;
                            d.Remarks = detail.Remarks;
                            d.InspectedBy = detail.InspectedBy;
                            d.ReviewedBy = detail.ReviewedBy;
                            d.NotedBy = detail.NotedBy;
                            d.LocationEmergencyLightId = detail.LocationEmergencyLightId;
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
        public ActionResult EditData(EmergencyLightViewModel[] item)
        {
            string status = "success";
            string message = "";

            try
            {
                int headerId = item[0].ID;

                foreach (var detail in item)
                {
                    var d = _context.EmergencyLightDetails
                        .Where(a => a.EmergencyLightHeaderId == headerId)
                        .Where(a => a.ItemId == detail.ItemId)
                        .FirstOrDefault();

                    if (d == null)
                    {

                        var _detail = new EmergencyLightDetail
                        {
                          
                            ItemId = detail.ItemId,
                            Battery = detail.Battery == "true" ? 1 : 0,
                            Bulb = detail.Bulb == "true" ? 1 : 0,
                            Usable = detail.Usable == "true" ? 1 : 0,

                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now,
                            EmergencyLightHeaderId = headerId,
                            Remarks = detail.Remarks,

                            //InspectedBy = detail.InspectedBy,
                            //ReviewedBy = detail.ReviewedBy,
                            //NotedBy = detail.NotedBy

                        };

                        _context.Add(_detail);


                    }
                    else
                    {
                        d.Battery = detail.Battery == "true" ? 1 : 0;
                        d.Bulb = detail.Bulb == "true" ? 1 : 0;
                        d.Usable = detail.Usable == "true" ? 1 : 0;

                        d.UpdatedAt = DateTime.Now;
                        d.UpdatedBy = User.Identity.GetUserName();
                        d.EmergencyLightHeaderId = headerId;
                        d.Remarks = detail.Remarks;
                        //d.InspectedBy = detail.InspectedBy;
                        //d.ReviewedBy = detail.ReviewedBy;
                        //d.NotedBy = detail.NotedBy;
                        _context.Update(d);
                    }
                    _context.SaveChanges();
                }
                status = "success";
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


                var mddaf = _context.EmergencyLightHeaders.Find(id);
                mddaf.Status = "Deleted_" + DateTime.Now.Ticks.ToString();
                _context.Update(mddaf);


                Log log = new Log();
                log.Descriptions = "Delete EmergencyLightHeader Id : " + id;
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
            //var v = _context.EmergencyLightDetails.Where(a => a.EmergencyLightHeaderId == id) //A
            //        .GroupJoin(
            //           _context.LocationEmergencyLights // B
            //           .Where(a => a.Status == "Active"),
            //           i => i.LocationEmergencyLightId, //A key
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

            //                  Id = A.i.LocationEmergencyLightId,
            //                  A.i.Battery,
            //                  A.i.Bulb,
            //                  A.i.Usable,

            //                  A.i.Remarks,
            //                  B.Location,
            //                  B.Code,
            //                  A.i.InspectedBy,
            //                  A.i.ReviewedBy,
            //                  A.i.NotedBy,

            //                  CompanyName = B.Areas.Companies.Name
            //              }
            //        );

            var v = _context.EmergencyLightDetails
                  .Where(a => a.EmergencyLightHeaders.Status == "Active")
                  .Where(a => a.EmergencyLightHeaderId == id) //A
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
                          temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                          (A, B) =>
                               new
                               {

                                   A.i.ItemId,
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
                                   A.i.EmergencyLightHeaders.DocumentStatus,
                                   A.i.ImageUrl,
                                   A.i.Id,
                                   A.i.Locations.Location
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
    }
}
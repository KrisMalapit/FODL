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
    public class FireExtinguishersController : Controller
    {
        private readonly SEMSystemContext _context;
        public FireExtinguishersController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Fire Extinguishers");
            return View();
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: FireExtinguisher/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fire Extinguisher",
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
        // GET: FireExtinguisher/Edit
        public IActionResult Edit(int id)
        {
            var model = _context.FireExtinguisherHeaders.Where(a=>a.Id == id).FirstOrDefault();
            
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fire Extinguisher",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });

            ViewData["ID"] = id;
            ViewData["DocumentStatus"] = model.DocumentStatus;
            //ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", model.Locations.AreaId);
            ViewData["Area"] = _context.Areas.Find(model.AreaId).Name;
            //ViewData["LocationId"] = new SelectList(_context.LocationFireExtinguishers, "Id", "Name",model.LocationFireExtinguisherId);
            //ViewData["Location"] = _context.LocationFireExtinguishers.Find(model.LocationFireExtinguisherId).Location;

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

                //     _context.FireExtinguisherHeaders
                //     .Select(a => new
                //     {
                //         a.CreatedAt,
                //         CompanyName = a.Areas.Companies.Name,
                //         AreaName = a.Areas.Name
                //         //,
                //         //a.Locations.Location
                //         ,
                //         a.Status,
                //         a.DocumentStatus,
                //         a.ReferenceNo
                //     })
                //    .Where(a => a.Status == "Active")
                //    .Where(strFilter)
                //    .Count();



                int recCount =  _context.FireExtinguisherHeaders //A
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
                             A.i.Id
                             ,
                            A.i.CreatedBy
                         }
                   ).Where(a => a.Status == "Active")
                    .Where(strFilter)
                    .Count();






                recordsTotal = recCount;
                int recFilter = recCount;






                var v =

              _context.FireExtinguisherHeaders
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
                   ).Where(a => a.Status == "Active")
              .Where(strFilter)
            
              .Skip(skip).Take(pageSize)
              ;

                //string strSql = v.ToSql();
                bool desc = false;
                if (sortColumnDirection == "desc")
                {
                    desc = true;
                }
                v = v.OrderBy(sortColumn + (desc ? " descending" : ""));




                var xxx = v.ToList();
                var data = v;

                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal,  data };
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


            var v =

               _context.LocationFireExtinguishers
                    .Where(a => a.Id == LocationId)
                    .Where(a => a.Status == "Status") //A
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
                              //A.i.Location,
                              B.Items.Type,
                              B.Items.Capacity,
                              B.Items.Code,
                              ItemName = B.Items.Name,
                              CompanyName = A.i.Areas.Companies.Name,   
                              A.i.Id
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

                                      A.i.ItemId,
                                      ItemName = A.i.Items.Name,
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
                                      ,A.i.ImageUrl
                                      ,
                                     A.i.Id
                                 }

                            );
            status = "success";

            var model = new
            {
                status
                ,
                 
                datadetail = detail

                , data = v
            
            };
            return Json(model);
        }
        [HttpPost]
        public IActionResult SaveData(FireExtinguisherViewModel[] item)
        {
            string series = "";
            string refno = "";
           
            
            int headerId = 0;
            string status = "";
            string message = "";
            try
            {
                var _header = _context.FireExtinguisherHeaders
                     .Where(a => a.AreaId == item[0].AreaId)
                    .Where(a => a.Status == "Active")
                    .Where(a => a.DocumentStatus != "Approved");

                if (_header.Count() == 0)
                {

                    var comp = _context.LocationFireExtinguishers.Include(a=>a.Areas.Companies).Where(a => a.Id == item[0]
                        .LocationFireExtinguisherId)
                        .FirstOrDefault()
                        .Areas.Companies.Code;

                    if (comp == "SCPC")
                    {
                        comp = "SC";
                    }
                    else
                    {
                        comp = "SL";

                    }
                    string series_code = comp + "FIREEXTINGUISHER";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = comp + "FE" + series;


                    FireExtinguisherHeader header = new FireExtinguisherHeader
                    {
                        ReferenceNo =  refno,
                        AreaId = item[0].AreaId,
                        CreatedAt = DateTime.Now.Date,
                        CreatedBy = User.Identity.GetFullName()
                    };

                    _context.Add(header);
                    _context.SaveChanges();
                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);

                    headerId = header.Id;

                    foreach (var detail in item)
                    {
                        var _detail = new FireExtinguisherDetail
                        {
                            ItemId = detail.ItemId,
                            Cylinder = detail.Cylinder == "true" ? 1 : 0,
                            Lever = detail.Lever == "true" ? 1 : 0,
                            Gauge = detail.Gauge == "true" ? 1 : 0,
                            SafetySeal = detail.SafetySeal == "true" ? 1 : 0,
                            Hose = detail.Hose == "true" ? 1 : 0,
                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now.Date,
                            FireExtinguisherHeaderId = headerId,
                            Remarks = detail.Remarks,
                            InspectedBy = detail.InspectedBy,
                            ReviewedBy = detail.ReviewedBy,
                            NotedBy = detail.NotedBy,
                            LocationFireExtinguisherId = detail.LocationFireExtinguisherId

                        };

                        _context.Add(_detail);
                    }
                }
                else
                {
                    headerId = _header.FirstOrDefault().Id;

                    foreach (var detail in item)
                    {
                        var d = _context.FireExtinguisherDetails
                            .Where(a => a.FireExtinguisherHeaderId == headerId)
                            .Where(a=>a.LocationFireExtinguisherId == item[0].LocationFireExtinguisherId)
                            .Where(a => a.ItemId == detail.ItemId)
                            .FirstOrDefault();

                        if (d == null)
                        {

                            var _detail = new FireExtinguisherDetail
                            {
                                ItemId = detail.ItemId,
                                Cylinder = detail.Cylinder == "true" ? 1 : 0,
                                Lever = detail.Lever == "true" ? 1 : 0,
                                Gauge = detail.Gauge == "true" ? 1 : 0,
                                SafetySeal = detail.SafetySeal == "true" ? 1 : 0,
                                Hose = detail.Hose == "true" ? 1 : 0,
                                CreatedAt = DateTime.Now.Date,
                                UpdatedAt = DateTime.Now,
                                FireExtinguisherHeaderId = headerId,
                                Remarks = detail.Remarks,
                                InspectedBy = detail.InspectedBy,
                                ReviewedBy = detail.ReviewedBy,
                                NotedBy = detail.NotedBy,
                                LocationFireExtinguisherId = detail.LocationFireExtinguisherId
                            };

                            _context.Add(_detail);


                        }
                        else
                        {
                            d.Cylinder = detail.Cylinder == "true" ? 1 : 0;
                            d.Lever = detail.Lever == "true" ? 1 : 0;
                            d.Gauge = detail.Gauge == "true" ? 1 : 0;
                            d.SafetySeal = detail.SafetySeal == "true" ? 1 : 0;
                            d.Hose = detail.Hose == "true" ? 1 : 0;
                            d.UpdatedAt = DateTime.Now;
                            d.UpdatedBy = User.Identity.GetUserName();
                            d.FireExtinguisherHeaderId = headerId;
                            d.Remarks = detail.Remarks;
                            d.InspectedBy = detail.InspectedBy;
                            d.ReviewedBy = detail.ReviewedBy;
                            d.NotedBy = detail.NotedBy;
                            d.LocationFireExtinguisherId = detail.LocationFireExtinguisherId;
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
                ,message
                ,ReferenceId= headerId
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
                    var d = _context.FireExtinguisherDetails
                        .Where(a => a.FireExtinguisherHeaderId == headerId)
                        .Where(a => a.ItemId == detail.ItemId)
                        .FirstOrDefault();

                    if (d == null)
                    {

                        var _detail = new FireExtinguisherDetail
                        {
                            ItemId = detail.ItemId,
                            Cylinder = detail.Cylinder == "true" ? 1 : 0,
                            Lever = detail.Lever == "true" ? 1 : 0,
                            Gauge = detail.Gauge == "true" ? 1 : 0,
                            SafetySeal = detail.SafetySeal == "true" ? 1 : 0,
                            Hose = detail.Hose == "true" ? 1 : 0,
                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now,
                            FireExtinguisherHeaderId = headerId,
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
                        d.Lever = detail.Lever == "true" ? 1 : 0;
                        d.Gauge = detail.Gauge == "true" ? 1 : 0;
                        d.SafetySeal = detail.SafetySeal == "true" ? 1 : 0;
                        d.Hose = detail.Hose == "true" ? 1 : 0;
                        d.UpdatedAt = DateTime.Now;
                        d.UpdatedBy = User.Identity.GetUserName();
                        d.FireExtinguisherHeaderId = headerId;
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


                var mddaf = _context.FireExtinguisherHeaders.Find(id);
                mddaf.Status = "Deleted_" + DateTime.Now.Ticks.ToString();
                _context.Update(mddaf);


                Log log = new Log();
                log.Descriptions = "Delete FireExtinguisherHeader Id : " + id;
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
            var v = _context.FireExtinguisherDetails
                   .Where(a => a.FireExtinguisherHeaders.Status == "Active")
                   .Where(a => a.FireExtinguisherHeaderId == id) //A
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
                           temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                           (A, B) =>
                                new
                                {

                                    A.i.ItemId,
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
                                    HeaderId = A.i.FireExtinguisherHeaderId,
                                    A.i.FireExtinguisherHeaders.DocumentStatus,
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
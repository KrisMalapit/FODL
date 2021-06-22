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
    public class FireHydrantsController : Controller
    {
        private readonly SEMSystemContext _context;
        public FireHydrantsController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Fire Hydrants");
            return View();
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: FireHydrant/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fire Hydrant",
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
        // GET: FireHydrant/Create
        public IActionResult Edit(int id)
        {
            var model = _context.FireHydrantHeaders.Where(a => a.Id == id).FirstOrDefault();

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Fire Hydrant",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });


            ViewData["ID"] = id;
            ViewData["DocumentStatus"] = model.DocumentStatus;
            // ViewData["AreaId"] = new SelectList(_context.LocationFireHydrants, "ID", "Name", model.LocationFireHydrantId);
            ViewData["Area"] = _context.Areas.Find(model.AreaId).Name;

            //ViewData["LocationId"] = new SelectList(_context.LocationFireExtinguishers, "Id", "Name",model.LocationFireExtinguisherId);
            //ViewData["Location"] = _context.LocationFireHydrants.Find(model.LocationFireHydrantId).Location;


            ViewData["Title"] = "Edit";
            ViewData["CreatedAt"] = model.CreatedAt.ToString("MM-dd-yyyy");
            ViewData["Company"] = _context.Areas.Include(a=>a.Companies).Where(a=>a.ID == model.AreaId).FirstOrDefault().Companies.Name;


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

                //     _context.FireHydrantHeaders
                //     .Select(a => new
                //     {
                //         a.CreatedAt,
                //         CompanyName = a.Locations.Areas.Companies.Name,
                //         AreaName = a.Locations.Areas.Name
                //         ,
                //         a.Locations.Location
                //         ,
                //         a.Status,
                //         a.DocumentStatus,
                //         a.ReferenceNo
                //     })
                //    .Where(a => a.Status == "Active")
                //    .Where(strFilter)
                //    .Count();

                int recCount = _context.FireHydrantHeaders //A
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
                    .Count();


                recordsTotal = recCount;
                int recFilter = recCount;


                //  var v =

                //_context.FireHydrantHeaders
                // .Select(a => new
                // {



                //     a.CreatedAt,
                //     CompanyName = a.Locations.Areas.Companies.Name,
                //     AreaName = a.Locations.Areas.Name
                //     ,
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
                //.Skip(skip).Take(pageSize)
                //;
                var v = _context.FireHydrantHeaders //A
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
                     ).Where(strFilter)
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

            //    _context.LocationFireHydrants
            //    .Where(a => a.Status == "Active")
            //    .Where(a => a.AreaId == AreaId)
            //    .Select(a => new {
            //        a.Location,

            //        a.Code,
            //        a.Id,
            //        CompanyName = a.Areas.Companies.Name
            //    });



            //var detail = _context.FireHydrantDetails
            //        .Where(a => a.FireHydrantHeaders.Status == "Active")
            //        .Where(a => a.FireHydrantHeaders.AreaId == AreaId)
            //        .Where(a => a.FireHydrantHeaders.CreatedAt == dateTime) //A
            //                                                                   //.Where(a => a.FireHydrantHeaderId == id) //A
            //                   .GroupJoin(
            //                      _context.LocationFireHydrants // B
            //                      .Where(a => a.Status == "Active"),
            //                      i => i.LocationFireHydrantId, //A key
            //                      p => p.Id,//B key
            //                      (i, g) =>
            //                         new
            //                         {
            //                             i, //holds A data
            //                             g  //holds B data
            //                         }
            //                   )
            //                   .SelectMany(
            //                      temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
            //                      (A, B) =>
            //                         new
            //                         {

            //                             Id = A.i.LocationFireHydrantId,

            //                             A.i.GlassCabinet,
            //                             A.i.Hanger,
            //                             A.i.Hose15,
            //                             A.i.Nozzle15,
            //                             A.i.Hose25,
            //                             A.i.Nozzle25,
            //                             A.i.SpecialTools,

            //                             A.i.Remarks,
            //                             B.Location,
            //                             B.Code,
            //                             A.i.InspectedBy,
            //                             A.i.ReviewedBy,
            //                             A.i.NotedBy,

            //                             CompanyName = B.Areas.Companies.Name,
            //                             HeaderId = A.i.FireHydrantHeaderId
            //                         }
            //                   );
            var v =

               _context.LocationFireHydrants
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

            var detail = _context.FireHydrantDetails
                    .Where(a => a.FireHydrantHeaders.Status == "Active")
                    .Where(a => a.LocationFireHydrantId == LocationId)
                    .Where(a => a.FireHydrantHeaders.CreatedAt == dateTime) //A
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
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                 new
                                 {

                                     A.i.ItemId,
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
                                     CompanyName = B.Areas.Companies.Name,
                                     HeaderId = A.i.FireHydrantHeaderId,
                                     A.i.FireHydrantHeaders.DocumentStatus,
                                     A.i.ImageUrl,
                                     A.i.Id
                                 }
                            );



            status = "success";
            //var x = detail.ToList();
            var model = new
            {
                status,

                datadetail = detail.ToList()
                ,
                data = v

            };
            return Json(model);
        }
        [HttpPost]
        public IActionResult SaveData(FireHydrantViewModel[] item)
        {
            string series = "";
            string refno = "";
            //string series_code = "FIREHYDRANT";
            //series = new NoSeriesController(_context).GetNoSeries(series_code);
            //refno = "FH" + series;

            int headerId = 0;
            string status = "";
            string message = "";
            try
            {
                //var _header = _context.FireHydrantHeaders.Where(a => a.Status == "Active")
                //// .Where(a => a.AreaId == item[0].AreaId)
                //.Where(a => a.LocationFireHydrantId == item[0].LocationFireHydrantId)
                //.Where(a => a.CreatedAt == DateTime.Now.Date);

                var _header = _context.FireHydrantHeaders
                     .Where(a => a.AreaId == item[0].AreaId)
                    .Where(a => a.Status == "Active")
                    .Where(a => a.DocumentStatus != "Approved");

                if (_header.Count() == 0)
                {
                    var comp = _context.LocationFireHydrants.Include(a => a.Areas.Companies).Where(a => a.Id == item[0].LocationFireHydrantId).FirstOrDefault()
                     .Areas.Companies.Code;
                    if (comp == "SCPC")
                    {
                        comp = "SC";
                    }
                    else
                    {
                        comp = "SL";

                    }
                    string series_code = comp + "FIREHYDRANT";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = comp + "FH" + series;


                    FireHydrantHeader header = new FireHydrantHeader
                    {
                        ReferenceNo = refno,
                        //LocationFireHydrantId = item[0].LocationFireHydrantId,
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
                        var _detail = new FireHydrantDetail
                        {
                           
                            ItemId = detail.ItemId,
                            GlassCabinet = detail.GlassCabinet == "true" ? 1 : 0,
                            Hanger = detail.Hanger == "true" ? 1 : 0,
                            Hose15 = detail.Hose15 == "true" ? 1 : 0,
                            Nozzle15 = detail.Nozzle15 == "true" ? 1 : 0,
                            Hose25 = detail.Hose25 == "true" ? 1 : 0,
                            Nozzle25 = detail.Nozzle25 == "true" ? 1 : 0,
                            SpecialTools = detail.SpecialTools == "true" ? 1 : 0,
                            InspectedBy = detail.InspectedBy,
                            ReviewedBy = detail.ReviewedBy,
                            NotedBy = detail.NotedBy,
                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now.Date,
                            FireHydrantHeaderId = headerId,
                            Remarks = detail.Remarks,
                            LocationFireHydrantId = detail.LocationFireHydrantId,


                        };

                        _context.Add(_detail);
                    }
                }
                else
                {
                    headerId = _header.FirstOrDefault().Id;

                    foreach (var detail in item)
                    {
                        var d = _context.FireHydrantDetails
                            .Where(a => a.FireHydrantHeaderId == headerId)
                            .Where(a => a.LocationFireHydrantId == item[0].LocationFireHydrantId)
                            .Where(a => a.ItemId == detail.ItemId)
                            .FirstOrDefault();

                        if (d == null)
                        {

                            var _detail = new FireHydrantDetail
                            {
                               
                                ItemId = detail.ItemId,


                                GlassCabinet = detail.GlassCabinet == "true" ? 1 : 0,
                                Hanger = detail.Hanger == "true" ? 1 : 0,
                                Hose15 = detail.Hose15 == "true" ? 1 : 0,
                                Nozzle15 = detail.Nozzle15 == "true" ? 1 : 0,
                                Hose25 = detail.Hose25 == "true" ? 1 : 0,
                                Nozzle25 = detail.Nozzle25 == "true" ? 1 : 0,
                                SpecialTools = detail.SpecialTools == "true" ? 1 : 0,


                                InspectedBy = detail.InspectedBy,
                                ReviewedBy = detail.ReviewedBy,
                                NotedBy = detail.NotedBy,

                                LocationFireHydrantId = detail.LocationFireHydrantId,


                                CreatedAt = DateTime.Now.Date,
                                UpdatedAt = DateTime.Now,
                                FireHydrantHeaderId = headerId,
                                Remarks = detail.Remarks
                            };

                            _context.Add(_detail);


                        }
                        else
                        {
                            d.GlassCabinet = detail.GlassCabinet == "true" ? 1 : 0;
                            d.Hanger = detail.Hanger == "true" ? 1 : 0;
                            d.Hose15 = detail.Hose15 == "true" ? 1 : 0;
                            d.Nozzle15 = detail.Nozzle15 == "true" ? 1 : 0;
                            d.Hose25 = detail.Hose25 == "true" ? 1 : 0;
                            d.Nozzle25 = detail.Nozzle25 == "true" ? 1 : 0;
                            d.SpecialTools = detail.SpecialTools == "true" ? 1 : 0;


                            d.InspectedBy = detail.InspectedBy;
                            d.Remarks = detail.Remarks;
                            d.NotedBy = detail.NotedBy;

                            d.LocationFireHydrantId = detail.LocationFireHydrantId;




                            d.UpdatedAt = DateTime.Now;
                            d.UpdatedBy = User.Identity.GetUserName();
                            d.FireHydrantHeaderId = headerId;
                            d.ReviewedBy = detail.ReviewedBy;

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
                message
                 ,
                ReferenceId = headerId
            };
            return Json(model);
        }
        [HttpPost]
        public ActionResult EditData(FireHydrantViewModel[] item)
        {
            string status = "success";
            string message = "";

            try
            {
                int headerId = item[0].ID;

                foreach (var detail in item)
                {
                    var d = _context.FireHydrantDetails
                        .Where(a => a.FireHydrantHeaderId == headerId)
                        .Where(a => a.ItemId == detail.ItemId)
                        .FirstOrDefault();

                    if (d == null)
                    {

                        var _detail = new FireHydrantDetail
                        {
                            
                            ItemId = detail.ItemId,
                            GlassCabinet = detail.GlassCabinet == "true" ? 1 : 0,
                            Hanger = detail.Hanger == "true" ? 1 : 0,
                            Hose15 = detail.Hose15 == "true" ? 1 : 0,
                            Nozzle15 = detail.Nozzle15 == "true" ? 1 : 0,
                            Hose25 = detail.Hose25 == "true" ? 1 : 0,
                            Nozzle25 = detail.Nozzle25 == "true" ? 1 : 0,
                            SpecialTools = detail.SpecialTools == "true" ? 1 : 0,
                            InspectedBy = detail.InspectedBy,
                            ReviewedBy = detail.ReviewedBy,
                            NotedBy = detail.NotedBy,
                            CreatedAt = DateTime.Now.Date,
                            UpdatedAt = DateTime.Now,
                            FireHydrantHeaderId = headerId,
                            Remarks = detail.Remarks
                        };

                        _context.Add(_detail);


                    }
                    else
                    {
                        d.GlassCabinet = detail.GlassCabinet == "true" ? 1 : 0;
                        d.Hanger = detail.Hanger == "true" ? 1 : 0;
                        d.Hose15 = detail.Hose15 == "true" ? 1 : 0;
                        d.Nozzle15 = detail.Nozzle15 == "true" ? 1 : 0;
                        d.Hose25 = detail.Hose25 == "true" ? 1 : 0;
                        d.Nozzle25 = detail.Nozzle25 == "true" ? 1 : 0;
                        d.SpecialTools = detail.SpecialTools == "true" ? 1 : 0;


                        d.InspectedBy = detail.InspectedBy;
                        d.Remarks = detail.Remarks;
                        d.NotedBy = detail.NotedBy;
                        d.UpdatedAt = DateTime.Now;
                        d.UpdatedBy = User.Identity.GetUserName();
                        d.FireHydrantHeaderId = headerId;
                        d.ReviewedBy = detail.ReviewedBy;

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


                var mddaf = _context.FireHydrantHeaders.Find(id);
                mddaf.Status = "Deleted_" + DateTime.Now.Ticks.ToString();
                _context.Update(mddaf);


                Log log = new Log();
                log.Descriptions = "Delete FireHydrantHeader Id : " + id;
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
            //var v = _context.FireHydrantDetails.Where(a => a.FireHydrantHeaderId == id) //A
            //        .GroupJoin(
            //           _context.LocationFireHydrants // B
            //           .Where(a => a.Status == "Active"),
            //           i => i.LocationFireHydrantId, //A key
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

            //                  Id = A.i.LocationFireHydrantId,
            //                  A.i.GlassCabinet,
            //                  A.i.Hanger,
            //                  A.i.Hose15,
            //                  A.i.Nozzle15,
            //                  A.i.Hose25,
            //                  A.i.Nozzle25,
            //                  A.i.SpecialTools,



            //                  A.i.Remarks,
            //                  B.Location,
            //                  B.Code,
            //                  A.i.InspectedBy,
            //                  A.i.ReviewedBy,
            //                  A.i.NotedBy,

            //                  CompanyName = B.Areas.Companies.Name
            //              }
            //        );



            var v = _context.FireHydrantDetails
                   .Where(a => a.FireHydrantHeaders.Status == "Active")
                   .Where(a => a.FireHydrantHeaderId == id) //A
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
                           temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                           (A, B) =>
                                new
                                {

                                    A.i.ItemId,
                                    ItemName = A.i.Items.Name,
                                    A.i.GlassCabinet,
                                    A.i.Hanger,
                                    A.i.Hose15,
                                    A.i.Nozzle15,
                                    A.i.Hose25,
                                    A.i.Nozzle25,
                                    A.i.SpecialTools,
                                    A.i.Remarks,
                                    A.i.Items.Code,

                                    A.i.InspectedBy,
                                    A.i.ReviewedBy,
                                    A.i.NotedBy,
                                    CompanyName = B.Areas.Companies.Name,
                                    HeaderId = A.i.FireHydrantHeaderId,
                                    A.i.FireHydrantHeaders.DocumentStatus,
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
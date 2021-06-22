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
    public class BicycleEntryController : Controller
    {
        private readonly SEMSystemContext _context;
        public BicycleEntryController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Bicycles
        public async Task<IActionResult> Index()
        {
            this.SetCurrentBreadCrumbTitle("Bicycle Entry");
            return View();
        }
        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: BicycleEntry/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Bicycle Entry",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });
            ViewData["ID"] = 0;
            ViewData["Title"] = "Create";
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "ID", "IdentificationNo");
            ViewData["CreatedAt"] = DateTime.Now.Date.ToString("MM-dd-yyyy");
            return View();
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: BicycleEntry/Create
        public IActionResult Edit(int id)
        {
            var model = _context.BicycleEntryHeaders.Find(id);

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Bicycle Entry",
                Url = string.Format(Url.Action("Index")),
                Order = 1
            });


            ViewData["ID"] = id;
            //ViewData["BicycleId"] = new SelectList(_context.Areas, "ID", "IdentificationNo", model.BicycleId);
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "ID", "IdentificationNo", model.BicycleId);
            ViewData["Title"] = "Edit";
            ViewData["CreatedAt"] = model.CreatedAt.ToString("MM-dd-yyyy");

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



                int recCount =

                     _context.BicycleEntryHeaders
                     .Select(a => new
                     {
                         a.CreatedAt,
                         a.Bicycles.NameOwner,
                         a.Bicycles.IdentificationNo,
                         a.ReferenceNo,
                         a.Status
                         ,DepartmentName = a.Bicycles.Departments.Name,
                         CompanyName = a.Bicycles.Departments.Companies.Name
                     })
                    .Where(a => a.Status == "Active")
                    .Where(strFilter)
                    .Count();

                recordsTotal = recCount;
                int recFilter = recCount;


                var v =

              _context.BicycleEntryHeaders
               .Select(a => new
               {



                   a.CreatedAt,
                   a.Bicycles.NameOwner,
                   a.Bicycles.IdentificationNo,
                  
                   a.Id
                   ,
                   a.Status,
                   a.ReferenceNo,
                   DepartmentName = a.Bicycles.Departments.Name,
                   CompanyName = a.Bicycles.Departments.Companies.Name,
                   a.DocumentStatus,
                   a.CreatedBy

               })
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
        public IActionResult getDataPerIdentificationNo(int BicycleId)
        {
            string status = "";
            bool hasValue = true;

            var v =

                _context.Bicycles
                .Where(a => a.Status == "Active")
                .Where(a => a.ID == BicycleId)
                .Select(a => new
                {
                    a.NameOwner,
                    a.BrandName,
                    a.ContactNo,
                    Department = a.Departments.Name,
                    Company = a.Departments.Companies.Name,

                });

            //var v =  _context.BicycleEntryHeaders
            //      .Where(a => a.Status == "Active")
            //      .Select(a => new { a.Bicycles.NameOwner, a.Bicycles.BrandName, a.Bicycles.ContactNo, a.DocumentStatus });

            var detail = _context.BicycleEntryDetails
                .Where(a => a.BicycleHeaders.BicycleId == BicycleId)
                .Where(a => a.BicycleHeaders.Status ==  "Active")
                .Where(a => a.BicycleHeaders.DocumentStatus != "Approved")
                .FirstOrDefault();

            if (detail ==  null)
            {
                hasValue = false;
            }



            status = "success";




            var model = new
            {
                status
                ,
                data = v
                ,datadetail = detail
                ,
                hasValue

            };
            return Json(model);
        }
        public IActionResult getDataPerIdentificationNoEdit(int HeaderId)
        {
            string status = "";
            bool hasValue = true;

            var v = _context.BicycleEntryHeaders
                  .Where(a => a.Id == HeaderId)
                  .Select(a => new {
                      a.Bicycles.NameOwner
                      , a.Bicycles.BrandName
                  , a.Bicycles.ContactNo
                  , a.DocumentStatus
                  , Company = a.Bicycles.Departments.Companies.Name
                  , Department = a.Bicycles.Departments.Name


                  });

            var detail = _context.BicycleEntryDetails
                .Where(a => a.BicycleEntryHeaderId == HeaderId)
                
                .FirstOrDefault();

            if (detail == null)
            {
                hasValue = false;
            }



            status = "success";




            var model = new
            {
                status
                ,
                data = v
                ,
                datadetail = detail
                ,
                hasValue

            };
            return Json(model);
        }
        [HttpPost]
        public IActionResult CreateModifyBicycle(BicycleEntryDetail item,int BicycleId)
        {

          
            int headerId = 0;
            string status = "";
            string message = "";



            string series = "";
            string refno = "";
           

            try
            {
                var _header = _context.BicycleEntryHeaders
                    .Where(a => a.Status == "Active")
                    .Where(a => a.BicycleId == BicycleId)
                    .Where(a => a.DocumentStatus != "Approved").FirstOrDefault();

                if (_header == null)
                {
                    var comp = _context.Bicycles.Include(a => a.Departments.Companies)
                        .Where(a => a.ID == BicycleId).FirstOrDefault().Departments.Companies.Code;
                        

                    if (comp == "SCPC")
                    {
                        comp = "SC";
                    }
                    else
                    {
                        comp = "SL";
                    }
                    string series_code = comp + "BICYCLE";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = comp + "BIC" + series;

                    BicycleEntryHeader header = new BicycleEntryHeader
                    {
                        ReferenceNo = refno,
                        BicycleId = BicycleId,
                        CreatedAt = DateTime.Now.Date,
                        CreatedBy = User.Identity.GetFullName()
                    };
                    _context.Add(header);
                    _context.SaveChanges();

                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);


                    item.BicycleEntryHeaderId = header.Id;
                    item.UpdatedBy = User.Identity.GetUserName();
                    item.UpdatedAt = DateTime.Now.Date;
                    _context.Add(item);


                    headerId = header.Id;
                }
                else
                {
                    var d = _context.BicycleEntryDetails.Where(a=>a.BicycleEntryHeaderId == _header.Id).FirstOrDefault();
                    d.BrakeRemarks = item.BrakeRemarks;
                    d.BrakeSafe = item.BrakeSafe;
                    d.BrakeUnSafe = item.BrakeUnSafe;
                    d.ChainRemarks = item.ChainRemarks;
                    d.ChainSafe = item.ChainSafe;
                    d.ChainUnSafe = item.ChainUnSafe;
                    d.CrankChainRemarks = item.CrankChainRemarks;
                    d.CrankChainSafe = item.CrankChainSafe;
                    d.CrankChainUnSafe = item.CrankChainUnSafe;
                    d.FrameRemarks = item.FrameRemarks;
                    d.FrameSafe = item.FrameSafe;
                    d.FrameUnSafe = item.FrameUnSafe;
                    d.FrontForkRemarks = item.FrontForkRemarks;
                    d.FrontForkSafe = item.FrontForkSafe;
                    d.FrontForkUnSafe = item.FrontForkUnSafe;
                    d.FrontRearRemarks = item.FrontRearRemarks;
                    d.FrontRearSafe = item.FrontRearSafe;
                    d.FrontRearUnSafe = item.FrontRearUnSafe;

                    d.HandlebarRemarks = item.HandlebarRemarks;
                    d.HandlebarSafe = item.HandlebarSafe;
                    d.HandlebarUnSafe = item.HandlebarUnSafe;

                    d.SeatRemarks = item.SeatRemarks;
                    d.SeatSafe = item.SeatSafe;
                    d.SeatUnSafe = item.SeatUnSafe;

                    d.InspectedBy = item.InspectedBy;
                    d.NotedBy = item.NotedBy;

                    d.UpdatedBy = User.Identity.GetUserName();
                    d.UpdatedAt = DateTime.Now.Date;

                    _context.Entry(d).State = EntityState.Modified;
                    _context.SaveChanges();

                    headerId = _header.Id;

                }

              


                _context.SaveChanges();
                status = "success";

            }
            catch (Exception ex)
            {
                headerId = 0;
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
        public ActionResult Delete(int id)
        {
            string status = "";
            string message = "";
            try
            {
                var mddaf = _context.BicycleEntryHeaders.Find(id);
                mddaf.Status = "Deleted_" + DateTime.Now.Ticks.ToString();
                _context.Update(mddaf);

                Log log = new Log();
                log.Descriptions = "Delete BicycleEntryHeader Id : " + id;
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
            var v = _context.BicycleEntryDetails
                .Where(a => a.BicycleEntryHeaderId == id) 
                   .Select(a => new
                   {
                       a.Id,
                       //a.Remarks,
                       //a.Safe,
                       //a.UnSafe
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
    }
}
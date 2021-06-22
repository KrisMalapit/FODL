using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FODLSystem.Models;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using FODLSystem.Models.View_Model;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FODLSystem.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0, IgnoreAjaxRequests = true)]
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;
        private readonly SEMSystemContext _context;

        public HomeController(SEMSystemContext context
            //, ILogger<HomeController> logger
            )
        {
            //_logger = logger;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> saveSnapShot(string equipmenttype,int detailid)
        {
            
            string filename = "";
            string status = "";
            string message = "";
            try
            {
                IFormFile file = Request.Form.Files[0];
                string newFileName = DateTime.Now.Ticks +"_"+ equipmenttype+Convert.ToString(detailid) + "_" + file.FileName;
                string filePath = "";
               
                if (file.Length > 0)
                {
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\", newFileName);
                    
                   
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        filename = stream.Name;
                    }
                }


                switch (equipmenttype)
                {
                    case "fe":
                        var fe = _context.FireExtinguisherDetails.Find(detailid);
                        //if (!string.IsNullOrEmpty(fe.ImageUrl))
                        //{
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\" + fe.ImageUrl);
                        //    System.IO.File.Delete(filePath);
                        //}
                        fe.ImageUrl = newFileName;
                        _context.Entry(fe).State = EntityState.Modified;
                        _context.SaveChanges();
                        break;
                    case "el":
                        var el = _context.EmergencyLightDetails.Find(detailid);
                        //if (!string.IsNullOrEmpty(el.ImageUrl))
                        //{
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\" + el.ImageUrl);
                        //    System.IO.File.Delete(filePath);
                        //}
                        el.ImageUrl = newFileName;
                        _context.Entry(el).State = EntityState.Modified;
                        _context.SaveChanges();
                        break;
                    case "it":
                        var it = _context.InergenTankDetails.Find(detailid);
                        //if (!string.IsNullOrEmpty(it.ImageUrl))
                        //{
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\" + it.ImageUrl);
                        //    System.IO.File.Delete(filePath);
                        //}
                        it.ImageUrl = newFileName;
                        _context.Entry(it).State = EntityState.Modified;
                        _context.SaveChanges();
                        break;
                    case "fh":
                        var fh = _context.FireHydrantDetails.Find(detailid);
                        //if (!string.IsNullOrEmpty(fh.ImageUrl))
                        //{
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\" + fh.ImageUrl);
                        //    System.IO.File.Delete(filePath);
                        //}
                        fh.ImageUrl = newFileName;
                        _context.Entry(fh).State = EntityState.Modified;
                        _context.SaveChanges();
                        break;
                    case "bc":
                        var bc = _context.BicycleEntryDetails.Find(detailid);
                        //if (!string.IsNullOrEmpty(fh.ImageUrl))
                        //{
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\snapshots\" + fh.ImageUrl);
                        //    System.IO.File.Delete(filePath);
                        //}
                        bc.ImageUrl = newFileName;
                        _context.Entry(bc).State = EntityState.Modified;
                        _context.SaveChanges();
                        break;
                    default:
                        break;
                }
                status = "success";
            }
            catch (Exception e)
            {

                status = "fail";
                message = e.Message;
                e.Message.WriteLog();
             
            }
            

            return Json(null);
        }

        

        public LocationDashboardCount GetLocationCount()
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            var fe = _context.FireExtinguisherHeaders
                .Where(a => a.Status == "Active")
                .GroupJoin(
                       _context.Areas // B
                       .Where(a => a.Status == "Active"),
                       i => i.AreaId, //A key
                       p => p.ID,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    ).SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.CompanyId,
                              A.i.DocumentStatus
                          }
                    ).Where(a => compId.Contains(a.CompanyId));



            var el = _context.EmergencyLightHeaders
                .Where(a => a.Status == "Active").GroupJoin(
                       _context.Areas // B
                       .Where(a => a.Status == "Active"),
                       i => i.AreaId, //A key
                       p => p.ID,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    ).SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.CompanyId,
                              A.i.DocumentStatus
                          }
                    ).Where(a => compId.Contains(a.CompanyId));

            var it = _context.InergenTankHeaders
                .Where(a => a.Status == "Active").GroupJoin(
                       _context.Areas // B
                       .Where(a => a.Status == "Active"),
                       i => i.AreaId, //A key
                       p => p.ID,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    ).SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.CompanyId,
                              A.i.DocumentStatus
                          }
                    ).Where(a => compId.Contains(a.CompanyId));

            var fh = _context.FireHydrantHeaders
                .Where(a => a.Status == "Active").GroupJoin(
                       _context.Areas // B
                       .Where(a => a.Status == "Active"),
                       i => i.AreaId, //A key
                       p => p.ID,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    ).SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              B.CompanyId,
                              A.i.DocumentStatus
                          }
                    ).Where(a => compId.Contains(a.CompanyId));

          
            var ldc = new LocationDashboardCount
            {
                ReviewFE = fe.Where(a => a.DocumentStatus == "For Review").Count(),
                ApproverFE = fe.Where(a => a.DocumentStatus == "For Approval").Count(),
                ReviewEL = el.Where(a => a.DocumentStatus == "For Review").Count(),
                ApproverEL = el.Where(a => a.DocumentStatus == "For Approval").Count(),
                ReviewIT = it.Where(a => a.DocumentStatus == "For Review").Count(),
                ApproverIT = it.Where(a => a.DocumentStatus == "For Approval").Count(),
                ReviewFH = fh.Where(a => a.DocumentStatus == "For Review").Count(),
                ApproverFH = fh.Where(a => a.DocumentStatus == "For Approval").Count(),
                ApproverBC = _context.BicycleEntryHeaders.Where(a=>a.Status == "Active").Where(a => a.DocumentStatus == "For Approval").Count()
            };

            return ldc;

        }
        [HttpPost]
        public JsonResult Approve(int[] id, string docstatus,string equipmenttype)
        {
            string status = "";
            string message = "";
            string refno = "";


            if (docstatus == "For Review")
            {
                docstatus = "For Approval";
            }
            else
            {
                docstatus = "Approved";
            }

            switch (equipmenttype)
            {
                case "fe":

                    foreach (var item in id)
                    {
                        var wdif = _context.FireExtinguisherHeaders.Find(item);
                        wdif.DocumentStatus = docstatus;

                        if (docstatus == "For Approval")
                        {
                            wdif.ReviewedDate = DateTime.Now.Date;
                            _context.FireExtinguisherDetails.Where(a => a.FireExtinguisherHeaderId == item).ToList()
                           .ForEach(a =>
                               a.ReviewedBy = User.Identity.GetFullName()
                           );
                        }
                        else
                        {
                            wdif.ApprovedDate = DateTime.Now.Date;
                            _context.FireExtinguisherDetails.Where(a => a.FireExtinguisherHeaderId == item).ToList()
                           .ForEach(a =>
                               a.NotedBy = User.Identity.GetFullName()
                           );
                        }

                        _context.Entry(wdif).State = EntityState.Modified;




                        Log log = new Log
                        {
                            Descriptions = "Modified FEHeader, Id = " + item,
                            Action = "Modify",
                            Status = "Success",
                            UserId = User.Identity.GetUserName(),
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(log);

                        _context.SaveChanges();

                        string stat = new NotifyController(_context).SendNotification(docstatus,equipmenttype, item); // send email

                    }


                    status = "success";

                    var model = new
                    {
                        status,
                        message,
                       
                    };



                    return Json(model);

                case "el":

                    foreach (var item in id)
                    {
                        var wdif = _context.EmergencyLightHeaders.Find(item);
                        wdif.DocumentStatus = docstatus;

                        if (docstatus == "For Approval")
                        {
                            wdif.ReviewedDate = DateTime.Now.Date;
                            _context.EmergencyLightDetails.Where(a => a.EmergencyLightHeaderId == item).ToList()
                           .ForEach(a =>
                               a.ReviewedBy = User.Identity.GetFullName()
                           );
                        }
                        else
                        {
                            wdif.ApprovedDate = DateTime.Now.Date;
                            _context.EmergencyLightDetails.Where(a => a.EmergencyLightHeaderId == item).ToList()
                           .ForEach(a =>
                               a.NotedBy = User.Identity.GetFullName()
                           );
                        }

                        _context.Entry(wdif).State = EntityState.Modified;




                        Log log = new Log
                        {
                            Descriptions = "Modified ELHeader, Id = " + item,
                            Action = "Modify",
                            Status = "Success",
                            UserId = User.Identity.GetUserName(),
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(log);

                        _context.SaveChanges();

                        string stat = new NotifyController(_context).SendNotification(docstatus, equipmenttype, item); // send email

                    }


                    status = "success";

                    var modelel = new
                    {
                        status,
                        message,

                    };



                    return Json(modelel);


                case "it":

                    foreach (var item in id)
                    {
                        var wdif = _context.InergenTankHeaders.Find(item);
                        wdif.DocumentStatus = docstatus;

                        if (docstatus == "For Approval")
                        {
                            wdif.ReviewedDate = DateTime.Now.Date;
                            _context.InergenTankDetails.Where(a => a.InergenTankHeaderId == item).ToList()
                           .ForEach(a =>
                               a.ReviewedBy = User.Identity.GetFullName()
                           );
                        }
                        else
                        {
                            wdif.ApprovedDate = DateTime.Now.Date;
                            _context.InergenTankDetails.Where(a => a.InergenTankHeaderId == item).ToList()
                           .ForEach(a =>
                               a.NotedBy = User.Identity.GetFullName()
                           );
                        }

                        _context.Entry(wdif).State = EntityState.Modified;

                        Log log = new Log
                        {
                            Descriptions = "Modified ITHeader, Id = " + item,
                            Action = "Modify",
                            Status = "Success",
                            UserId = User.Identity.GetUserName(),
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(log);

                        _context.SaveChanges();

                        string stat = new NotifyController(_context).SendNotification(docstatus, equipmenttype, item); // send email

                    }


                    status = "success";

                    var modelit = new
                    {
                        status,
                        message,

                    };



                    return Json(modelit);

                case "bc":

                    foreach (var item in id)
                    {
                        var wdif = _context.BicycleEntryHeaders.Find(item);
                        wdif.DocumentStatus = docstatus;

                        
                        wdif.ApprovedDate = DateTime.Now.Date;
                        _context.BicycleEntryDetails.Where(a => a.BicycleEntryHeaderId == item).ToList()
                        .ForEach(a =>
                            a.NotedBy = User.Identity.GetFullName()
                        );
                 
                        _context.Entry(wdif).State = EntityState.Modified;

                        Log log = new Log
                        {
                            Descriptions = "Modified BCHeader, Id = " + item,
                            Action = "Modify",
                            Status = "Success",
                            UserId = User.Identity.GetUserName(),
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(log);

                        _context.SaveChanges();

                        string stat = new NotifyController(_context).SendNotification(docstatus, equipmenttype, item); // send email

                    }


                    status = "success";

                    var modelbc = new
                    {
                        status,
                        message,

                    };



                    return Json(modelbc);


                case "fh":

                    foreach (var item in id)
                    {
                        var wdif = _context.FireHydrantHeaders.Find(item);
                        wdif.DocumentStatus = docstatus;

                        if (docstatus == "For Approval")
                        {
                            wdif.ReviewedDate = DateTime.Now.Date;
                            _context.FireHydrantDetails.Where(a => a.FireHydrantHeaderId == item).ToList()
                           .ForEach(a =>
                               a.ReviewedBy = User.Identity.GetFullName()
                           );
                        }
                        else
                        {
                            wdif.ApprovedDate = DateTime.Now.Date;
                            _context.FireHydrantDetails.Where(a => a.FireHydrantHeaderId == item).ToList()
                           .ForEach(a =>
                               a.NotedBy = User.Identity.GetFullName()
                           );
                        }

                        _context.Entry(wdif).State = EntityState.Modified;




                        Log log = new Log
                        {
                            Descriptions = "Modified FHHeader, Id = " + item,
                            Action = "Modify",
                            Status = "Success",
                            UserId = User.Identity.GetUserName(),
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(log);

                        _context.SaveChanges();

                        string stat = new NotifyController(_context).SendNotification(docstatus, equipmenttype, item); // send email

                    }


                    status = "success";

                    var modelfh = new
                    {
                        status,
                        message,

                    };



                    return Json(modelfh);




                default:
                    return null;
            }




        }
        public IActionResult GetLocationData(string equipmenttype, string docstatus)
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            string status = "";
            string message = "";
           
            switch (equipmenttype)
            {
                case "fe":
                    var fe = _context.FireExtinguisherHeaders
                        .Where(a => a.DocumentStatus == docstatus)
                        .Where(a => a.Status == "Active")
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
                                 A.i.Id,
                                 A.i.Status,
                                 A.i.DocumentStatus,
                                 A.i.ReferenceNo,
                                 B.CompanyId
                             }
                       )
                       .Where(a => compId.Contains(a.CompanyId));

                    status = "success";

                    var model = new
                    {
                        status,
                        message,
                        data = fe
                    };
                  
                    return Json(model);

                case "el":

                    //var el = _context.EmergencyLightHeaders
                    //    .Where(a => a.Status == "Active")
                    //   .Where(a => a.DocumentStatus == docstatus)
                    //   .Select(a => new
                    //   {
                    //       a.CreatedAt,
                    //       CompanyName = a.Locations.Areas.Companies.Name,
                    //       AreaName = a.Locations.Areas.Name
                    //       ,
                    //       a.Locations.Location
                    //       ,
                    //       a.Id
                    //       ,
                    //       a.Status
                    //       ,
                    //       a.DocumentStatus,
                    //       a.ReferenceNo

                    //   });

                    var el = _context.EmergencyLightHeaders
                        .Where(a => a.DocumentStatus == docstatus)
                        .Where(a => a.Status == "Active")
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
                                 A.i.Id,
                                 A.i.Status,
                                 A.i.DocumentStatus,
                                 A.i.ReferenceNo,
                                 B.CompanyId
                             }
                       ).Where(a => compId.Contains(a.CompanyId));

                    status = "success";

                    var modelel = new
                    {
                        status,
                        message,
                        data = el
                    };


                    return Json(modelel);

                  
                case "it":
                    //var it = _context.InergenTankHeaders
                    //    .Where(a => a.Status == "Active")
                    //    .Where(a => a.DocumentStatus == docstatus)
                    //    .Select(a => new
                    //    {
                    //        a.CreatedAt
                    //       ,
                    //        CompanyName = a.Locations.Areas.Companies.Name
                    //       ,
                    //        AreaName = a.Locations.Areas.Name
                    //       ,
                    //        Location = a.Locations.Area
                    //       ,
                    //        a.Id
                    //       ,
                    //        a.Status
                    //       ,
                    //        a.DocumentStatus,
                    //        a.ReferenceNo

                    //    });

                    var it = _context.InergenTankHeaders
                        .Where(a => a.DocumentStatus == docstatus)
                        .Where(a => a.Status == "Active")
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
                                 A.i.Id,
                                 A.i.Status,
                                 A.i.DocumentStatus,
                                 A.i.ReferenceNo,
                                 B.CompanyId
                             }
                             ).Where(a => compId.Contains(a.CompanyId));
                    status = "success";

                    var modelit = new
                    {
                        status,
                        message,
                        data = it
                    };


                    return Json(modelit);

                case "bc":

                    var bc = _context.BicycleEntryHeaders
                       .Where(a => a.DocumentStatus == docstatus)
                       .Where(a => a.Status == "Active")
                       .GroupJoin(
                     _context.Bicycles // B
                       ,
                         i => i.BicycleId, //A key
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
                                CompanyName = B.Departments.Companies.Name,
                                AreaName = "",
                                A.i.Id,
                                A.i.Status,
                                A.i.DocumentStatus,
                                A.i.ReferenceNo,
                                B.Departments.CompanyId
                            }
                            ).Where(a => compId.Contains(a.CompanyId));
                    status = "success";


                    var rr = bc.ToList();
                    var modelbc = new
                    {
                        status,
                        message,
                        data = bc
                    };


                    return Json(modelbc);
                case "fh":
                    //var fh = _context.FireHydrantHeaders
                    //    .Where(a => a.Status == "Active")
                    //   .Where(a => a.DocumentStatus == docstatus)
                    //    .Select(a => new
                    //    {
                    //        a.CreatedAt,
                    //        CompanyName = a.Locations.Areas.Companies.Name,
                    //        AreaName = a.Locations.Areas.Name
                    //       ,
                    //        a.Locations.Location
                    //       ,
                    //        a.Id
                    //       ,
                    //        a.Status
                    //       ,
                    //        a.DocumentStatus,
                    //        a.ReferenceNo

                    //    });
                    var fh = _context.FireHydrantHeaders
                        .Where(a => a.DocumentStatus == docstatus)
                        .Where(a => a.Status == "Active")
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
                                 A.i.Id,
                                 A.i.Status,
                                 A.i.DocumentStatus,
                                 A.i.ReferenceNo,
                                 B.CompanyId
                             }
                       ).Where(a => compId.Contains(a.CompanyId));
                    status = "success";

                    var modelfh = new
                    {
                        status,
                        message,
                        data = fh
                    };


                    return Json(modelfh);


                  

                default:
                    return null; 
            }
           
           
           
            

            

           

        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();


            ViewData["ID"] = 0;
            //ViewData["AreaId"] = new SelectList(_context.Areas, "ID", "Name");
            ViewData["BicycleId"] = new SelectList(_context.Bicycles.Include(a=>a.Departments).Where(a=>compId.Contains(a.Departments.CompanyId)), "ID", "IdentificationNo");
            ViewData["CreatedAt"] = DateTime.Now.Date.ToString("MM-dd-yyyy");

            ViewData["ReferenceIdFE"] = 0;
            ViewData["ReferenceIdEL"] = 0;
            ViewData["ReferenceIdIT"] = 0;
            ViewData["ReferenceIdFH"] = 0;
            ViewData["ReferenceIdB"] = 0;

            var locationdashboard = GetLocationCount();

            ViewData["ReviewFE"] = locationdashboard.ReviewFE;
            ViewData["ReviewEL"] = locationdashboard.ReviewEL; 
            ViewData["ReviewIT"] = locationdashboard.ReviewIT; 
            ViewData["ReviewFH"] = locationdashboard.ReviewFH; 
            ViewData["ApproverFE"] = locationdashboard.ApproverFE;
            ViewData["ApproverEL"] = locationdashboard.ApproverEL;
            ViewData["ApproverIT"] = locationdashboard.ApproverIT;
            ViewData["ApproverFH"] = locationdashboard.ApproverFH;
            ViewData["ApproverBC"] = locationdashboard.ApproverBC;
            return View();
        }
        

        [BreadCrumb(Title = "Posts List", Order = 3, IgnoreAjaxRequests = true)]
        public ActionResult Posts()
        {
            //this.SetCurrentBreadCrumbTitle("dynamic title 1");

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Wikis",
                Url = string.Format("{0}?id=1", Url.Action("Index", "Home", values: new { area = "" })),
                Order = 1
            });
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Lab",
                Url = string.Format("{0}?id=2", Url.Action("Index", "Home", values: new { area = "" })),
                Order = 2
            });

            return View();
        }


        public IActionResult SampleLayout()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        public JsonResult SubmitDocument(int id,string module)
        {
            string message = "";
            string status = "";
            try
            {
                switch (module)
                {
                    case "FE":
                        var modelFE = _context.FireExtinguisherHeaders.Find(id);

                        int FELocationCnt = _context.LocationFireExtinguishers.Where(a => a.AreaId == modelFE.AreaId).Where(a => a.Status == "Active").Count();
                        int FEDetails = _context.FireExtinguisherDetails.Where(a => a.FireExtinguisherHeaders.AreaId == modelFE.AreaId).GroupBy(a => a.LocationFireExtinguisherId).Count();
                        if (FELocationCnt == FEDetails)
                        {
                            modelFE.DocumentStatus = "For Review";
                            _context.Update(modelFE);
                            status = "success";
                        }
                        else
                        {
                            message = "Submit not allowed. Not all locations has been checked";
                            status = "fail";
                        }
                       
                        break;
                    case "EL":
                        var modelEL = _context.EmergencyLightHeaders.Find(id);

                        int ELLocationCnt = _context.LocationEmergencyLights.Where(a => a.AreaId == modelEL.AreaId).Where(a => a.Status == "Active").Count();
                        int ELDetails = _context.EmergencyLightDetails.Where(a => a.EmergencyLightHeaders.AreaId == modelEL.AreaId).GroupBy(a => a.LocationEmergencyLightId).Count();
                        if (ELLocationCnt == ELDetails)
                        {
                            modelEL.DocumentStatus = "For Review";
                            _context.Update(modelEL);
                            status = "success";
                        }
                        else
                        {
                            message = "Submit not allowed. Not all locations has been checked";
                            status = "fail";
                        }
                     
                        break;
                    case "IT":
                       
                        var modelIT = _context.InergenTankHeaders.Find(id);

                        int ITLocationCnt = _context.LocationInergenTanks.Where(a => a.AreaId == modelIT.AreaId).Where(a => a.Status == "Active").Count();
                        int ITDetails = _context.InergenTankDetails.Where(a => a.InergenTankHeaders.AreaId == modelIT.AreaId).GroupBy(a => a.LocationInergenTankId).Count();
                        if (ITLocationCnt == ITDetails)
                        {
                            modelIT.DocumentStatus = "For Review";
                            _context.Update(modelIT);
                            status = "success";
                        }
                        else
                        {
                            message = "Submit not allowed. Not all locations has been checked";
                            status = "fail";
                        }

                      
                        break;
                    case "FH":
                        var modelFH = _context.FireHydrantHeaders.Find(id);

                        int FHLocationCnt = _context.LocationFireHydrants.Where(a => a.AreaId == modelFH.AreaId).Where(a => a.Status == "Active").Count();
                        int FHDetails = _context.FireHydrantDetails.Where(a => a.FireHydrantHeaders.AreaId == modelFH.AreaId).GroupBy(a => a.LocationFireHydrantId).Count();
                        if (FHLocationCnt == FHDetails)
                        {
                          
                            modelFH.DocumentStatus = "For Review";
                            _context.Update(modelFH);
                            status = "success";
                        }
                        else
                        {
                            message = "Submit not allowed. Not all locations has been checked";
                            status = "fail";
                        }

                       
                        break;

                    case "BC":
                        var modelBC = _context.BicycleEntryHeaders.Find(id);


                            modelBC.DocumentStatus = "For Approval";
                            _context.Update(modelBC);
                            status = "success";
                       


                        break;
                    default:
                        break;
                }
                string equipmenttype = module.ToLower();

                if (equipmenttype != "bc")
                {
                    string stat = new NotifyController(_context).SendNotification("For Review", equipmenttype, id); // send email
                }
                else
                {
                    
                    string stat = new NotifyController(_context).SendNotification("For Approval", equipmenttype, id); // send email
                }
                
                                                                                                              


                Log log = new Log
                {
                    Descriptions = "SubmitDocument ID " + id + " MODULE " + module,
                    Action = "Add",
                    Status = status,
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);
                _context.SaveChanges();

               
            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
            }

            var model = new
            {
                status, message
            };

            return Json(model);

        }
        
        
    }
  
}

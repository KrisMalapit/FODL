using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;

namespace FODLSystem.Controllers
{
    
    public class ReportsController : Controller
    {
        private readonly SEMSystemContext _context;

        public ReportsController(SEMSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public ActionResult Index()
        {
            this.SetCurrentBreadCrumbTitle("Reports");
            return View();
        }
        [HttpPost]
        public ActionResult getDataItemHistory(string strStart, string end, int AreaId)
        {
            string strFilter = "";
            try
            {
                DateTime startDate = DateTime.ParseExact(strStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);


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
                     _context.LocationItemDetails
                    .Where(a => a.Status == "Active")
                    .Where(a => a.CreatedDate.Date >= startDate.Date && a.CreatedDate.Date <= endDate.Date)
                    .Select(a => new
                    {
                        Location = a.Equipment == "FE" ? _context.LocationFireExtinguishers.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               a.Equipment == "EL" ? _context.LocationEmergencyLights.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               a.Equipment == "FH" ? _context.LocationFireHydrants.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               _context.LocationInergenTanks.Where(b => b.Id == a.HeaderId).FirstOrDefault().Area,

                    })
                    .Where(strFilter)
                    .Count();

                recordsTotal = recCount;
                int recFilter = recCount;


                var v =
                     _context.LocationItemDetails

                            .Where(a => a.Status == "Active")
                            .Where(a => a.CreatedDate.Date >= startDate.Date && a.CreatedDate.Date <= endDate.Date)

                            .Select(a => new
                            {
                                Location = a.Equipment == "FE" ? _context.LocationFireExtinguishers.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               a.Equipment == "EL" ? _context.LocationEmergencyLights.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               a.Equipment == "FH" ? _context.LocationFireHydrants.Where(b => b.Id == a.HeaderId).FirstOrDefault().Location :
                                               _context.LocationInergenTanks.Where(b => b.Id == a.HeaderId).FirstOrDefault().Area,
                                AreaId = a.Equipment == "FE" ? _context.LocationFireExtinguishers.Where(b => b.Id == a.HeaderId).FirstOrDefault().AreaId :
                                               a.Equipment == "EL" ? _context.LocationEmergencyLights.Where(b => b.Id == a.HeaderId).FirstOrDefault().AreaId :
                                               a.Equipment == "FH" ? _context.LocationFireHydrants.Where(b => b.Id == a.HeaderId).FirstOrDefault().AreaId :
                                               _context.LocationInergenTanks.Where(b => b.Id == a.HeaderId).FirstOrDefault().AreaId,
                                a.Items.EquipmentType,
                                ItemName = a.Items.Code + " - " + a.Items.Name,
                                TransferredDate = a.CreatedDate,
                                SafeKeepDate = a.Items.DatePurchased,
                                a.CreatedDate
                                    ,
                                a.Status
                            })
                            .Where(a => a.AreaId == AreaId)
                            .Where(strFilter)
                            .Skip(skip).Take(pageSize);







                bool desc = false;
                if (sortColumnDirection == "desc")
                {
                    desc = true;
                }
                v = v.OrderBy(sortColumn + (desc ? " descending" : ""));





                var data = v;

                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult getDataItemInventory(int AreaId)
        {
            string strFilter = "";
            try
            {

                DateTime? dt = new DateTime(1, 1, 0001);

                //FireExtinguishers Items
                var lst = new List<InventoryViewModel>();

                if (AreaId == -1) // All
                {
                    strFilter = "AreaId <> " + AreaId;
                }
                else if (AreaId == 0) //SAFEKEEP
                {
                    strFilter = "AreaId = " + AreaId;
                }
                else
                {
                    strFilter = "AreaId=" + AreaId;
                }

                var safeItems = _context.Items.Where(a => a.Status == "Active")
                    .Where(a=>a.IsIn == 1)
                    .Select(a => new
                    {
                        a.Code,
                        ItemName = a.Name,
                        a.SerialNo,
                        DatePurchased = a.DatePurchased == null ? dt : a.DatePurchased,
                        a.ItemStatus,
                        a.EquipmentType,
                        Location = "Safekeep",
                        Area = "Safekeep",
                        a.Warranty

                    });


                var feItems = _context.LocationFireExtinguishers.Where(a => a.Status == "Active")
                    .Where(strFilter)
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
                    ).SelectMany(
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                new
                                {
                                    B.Items.Code,
                                    ItemName = B.Items.Name,
                                    B.Items.SerialNo,
                                    DatePurchased = B.Items.DatePurchased == null ? dt : B.Items.DatePurchased,
                                    B.Items.ItemStatus,
                                    B.Items.EquipmentType,
                                    A.i.Location,
                                    Area = A.i.Areas.Name,
                                    B.Items.Warranty
                                }
                        );

                //Emergency Lights Items
                var elItems = _context.LocationEmergencyLights.Where(a => a.Status == "Active")
                    .Where(strFilter)
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
                    ).SelectMany(
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                new
                                {
                                    B.Items.Code,
                                    ItemName = B.Items.Name,
                                    B.Items.SerialNo,
                                    DatePurchased = B.Items.DatePurchased == null ? dt : B.Items.DatePurchased,
                                    B.Items.ItemStatus,
                                    B.Items.EquipmentType,
                                    A.i.Location,
                                    Area = A.i.Areas.Name,
                                    B.Items.Warranty
                                }
                        );

                //Fire Hydrant Items
                var fhItems = _context.LocationFireHydrants.Where(a => a.Status == "Active")
                   .Where(strFilter)
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
                    ).SelectMany(
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                new
                                {
                                    B.Items.Code,
                                    ItemName = B.Items.Name,
                                    B.Items.SerialNo,
                                    DatePurchased = B.Items.DatePurchased == null ? dt : B.Items.DatePurchased,
                                    B.Items.ItemStatus,
                                    B.Items.EquipmentType,
                                    A.i.Location,
                                    Area = A.i.Areas.Name,
                                    B.Items.Warranty
                                }
                        );

                //Inergen Tank Items
                var itItems = _context.LocationInergenTanks.Where(a => a.Status == "Active")
                    .Where(strFilter)
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
                    ).SelectMany(
                            temp => temp.g.Take(1).DefaultIfEmpty(), //gets data and transfer to B
                            (A, B) =>
                                new
                                {
                                    B.Items.Code,
                                    ItemName = B.Items.Name,
                                    B.Items.SerialNo,
                                    DatePurchased = B.Items.DatePurchased == null ? dt : B.Items.DatePurchased,
                                    B.Items.ItemStatus,
                                    B.Items.EquipmentType,
                                    Location = A.i.Area,
                                    Area = A.i.Areas.Name,
                                    B.Items.Warranty
                                }
                        );
                foreach (var item in feItems)
                {
                    var inventory = new InventoryViewModel()
                    {

                        Code = item.Code,
                        ItemName = item.ItemName,
                        SerialNo = item.SerialNo,
                        DatePurchased = item.DatePurchased,
                        ItemStatus = item.ItemStatus,
                        EquipmentType = item.EquipmentType,
                        Location = item.Location,
                        Area = item.Area,
                        Warranty = item.Warranty
                    };
                    lst.Add(inventory);
                }

                foreach (var item in elItems)
                {
                    var inventory = new InventoryViewModel()
                    {

                        Code = item.Code,
                        ItemName = item.ItemName,
                        SerialNo = item.SerialNo,
                        DatePurchased = item.DatePurchased,
                        ItemStatus = item.ItemStatus,
                        EquipmentType = item.EquipmentType,
                        Location = item.Location,
                        Area = item.Area,
                        Warranty = item.Warranty
                    };
                    lst.Add(inventory);
                }
                foreach (var item in fhItems)
                {
                    var inventory = new InventoryViewModel()
                    {

                        Code = item.Code,
                        ItemName = item.ItemName,
                        SerialNo = item.SerialNo,
                        DatePurchased = item.DatePurchased,
                        ItemStatus = item.ItemStatus,
                        EquipmentType = item.EquipmentType,
                        Location = item.Location,
                        Area = item.Area,
                        Warranty = item.Warranty
                    };
                    lst.Add(inventory);
                }
                foreach (var item in itItems)
                {
                    var inventory = new InventoryViewModel()
                    {

                        Code = item.Code,
                        ItemName = item.ItemName,
                        SerialNo = item.SerialNo,
                        DatePurchased = item.DatePurchased,
                        ItemStatus = item.ItemStatus,
                        EquipmentType = item.EquipmentType,
                        Location = item.Location,
                        Area = item.Area,
                        Warranty = item.Warranty
                    };
                    lst.Add(inventory);
                }
                if (AreaId <= 0 )
                {
                    foreach (var item in safeItems)
                    {
                        var inventory = new InventoryViewModel()
                        {

                            Code = item.Code,
                            ItemName = item.ItemName,
                            SerialNo = item.SerialNo,
                            DatePurchased = item.DatePurchased,
                            ItemStatus = item.ItemStatus,
                            EquipmentType = item.EquipmentType,
                            Location = item.Location,
                            Area = item.Area,
                            Warranty = item.Warranty
                        };
                        lst.Add(inventory);
                    }
                }
                




                var rec = lst.Where(a => a.ItemName != null).ToList();
                var model = new
                {

                    data = rec
                };

                return Json(model);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }





        }
        [BreadCrumb(Title = "Item History", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult ItemHistory()
        {
            ViewBag.Title = "Item History";
            ViewData["AreaId"] = new SelectList(_context.Areas, "ID", "Name");

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Reports",
                Url = string.Format(Url.Action("Index", "Reports")),
                Order = 1
            });
            return View();
        }
        [BreadCrumb(Title = "CheckList", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult CheckList()
        {
            ViewBag.Title = "Item CheckList";
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            var area = _context.Areas
                .Where(a => a.Status == "Active")
                .Where(a => compId.Contains(a.CompanyId))
                .Select(a => new
                {
                    a.ID,
                    Text = a.Name + " - " + a.Companies.Code
                });


            var lstarea = new List<AreaViewModel>();
            foreach (var item in area.ToList())
            {
                var _area = new AreaViewModel
                {
                    ID = item.ID,
                    Text = item.Text
                };
                lstarea.Add(_area);
            }

            //var customArea = new AreaViewModel();
            //customArea.ID = -1;
            //customArea.Text = "ALL";
            
            //lstarea.Add(customArea);
            

            ViewData["AreaId"] = new SelectList(lstarea.OrderBy(a => a.Text), "ID", "Text");

            var lstequipment = new List<EquipmentViewModel> {
                 //new EquipmentViewModel {ID = 0, Text = "ALL"}
                new EquipmentViewModel {ID = 1, Text = "Fire Extinguisher"}
                ,new EquipmentViewModel {ID = 2, Text = "Emergency Lights"}
                ,new EquipmentViewModel {ID = 3, Text = "Fire Hydrant"}
                ,new EquipmentViewModel {ID = 4, Text = "Inergen Tank"}
            };


            ViewData["EquipmentId"] = new SelectList(lstequipment.OrderBy(a => a.ID), "ID", "Text");


            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Reports",
                Url = string.Format(Url.Action("Index", "Reports")),
                Order = 1
            });



            return View();
        }

        [BreadCrumb(Title = "Item Inventory", Order = 2, IgnoreAjaxRequests = true)]
        public IActionResult ItemInventory()
        {
            ViewBag.Title = "Item Inventory";



            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Reports",
                Url = string.Format(Url.Action("Index", "Reports")),
                Order = 1
            });


            var area = _context.Areas.Where(a => a.Status == "Active")
                .Select(a => new
                {
                    a.ID,
                    Text = a.Name + " - " + a.Companies.Code
                });


            var lstarea = new List<AreaViewModel>();
            foreach (var item in area.ToList())
            {

                var _area = new AreaViewModel
                {
                    ID = item.ID,
                    Text = item.Text
                };
                lstarea.Add(_area);

            }

            var customArea = new AreaViewModel();
            customArea.ID = -1;
            customArea.Text = "ALL";
            var customArea2 = new AreaViewModel();
            customArea2.ID = 0;
            customArea2.Text = "SAFEKEEP";

            lstarea.Add(customArea);
            lstarea.Add(customArea2);

            ViewData["AreaId"] = new SelectList(lstarea.OrderBy(a => a.Text), "ID", "Text");
            return View();




        }
        public IActionResult printReport(ReportViewModel rvm)
        {

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                byte[] bytes = null;
                string xstring = JsonConvert.SerializeObject(rvm);



                string urilive = "http://californium/ScreeningToolCPCApi/api/printreport?rvm=";
                string uridev = "http://sodium2/semapi/api/printreport?rvm=";
                string urilocal = "http://localhost:57903/api/printreport?rvm=";

                response = client.GetAsync(uridev + xstring).Result;
                string byteToString = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
                bytes = Convert.FromBase64String(byteToString);

                string rpttype = "";
                switch (rvm.rptType)
                {
                    case "PDF":
                        rpttype = "application/pdf";
                        break;
                    case "Excel":
                        rpttype = "application/vnd.ms-excel";
                        break;
                    default:
                        break;
                }


                return File(bytes, rpttype);
            }
            catch (Exception e)
            {

                throw;
            }

        }

    }
}
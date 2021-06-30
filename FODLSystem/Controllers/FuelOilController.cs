using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ViewData["EquipmentId"] = new SelectList(_context.Equipments, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "List");


            //var itemlist = _context.Items;
            //var lstarea = new List<Item>();
            //foreach (var item in itemlist.ToList())
            //{
            //    var _area = new Item
            //    {
            //        Id = item.Id,
            //        Description = item.No + "|" + item.Description
            //    };
            //    lstarea.Add(_area);
            //}

            //ViewData["ItemNo"] = new SelectList(lstarea.OrderBy(a => a.Description), "Id", "Description");



            return View();
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
                  a.CreatedDate,
                  a.Shift,
                  UnitNo = a.Equipments.Name,
                  Location = a.Locations.List,
                  a.SMR,
                  a.Id
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
    }
}
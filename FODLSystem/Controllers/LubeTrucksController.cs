using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FODLSystem.Controllers
{
    public class LubeTrucksController : Controller
    {
        private readonly FODLSystemContext _context;

        public LubeTrucksController(FODLSystemContext context)
        {
            _context = context;
        }

        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            this.SetCurrentBreadCrumbTitle("LubeTruck");
            return View();
        }
        [HttpPost]
        public ActionResult SaveItem(int id, string No, string Description,string OldId)
        {
            string status = "";
            string message = "";
            try
            {

                if (id == 0)
                {

                    var item = new LubeTruck
                    {
                        No = No,
                        OldId = OldId,
                        Description = Description,
                       
                    };

                    _context.Add(item);
                    _context.SaveChanges();
                }
                else
                {
                    var item = _context.LubeTrucks.Find(id);
                    item.No = No;
                    item.OldId = OldId;
                    item.Description = Description;
                    item.DateModified = DateTime.Now.Date;
                    _context.Entry(item).State = EntityState.Modified;
                    _context.SaveChanges();
                }
               

                status = "success";
            }
            catch (Exception ex)
            {
                status = "fail";
                message = ex.InnerException.Message;
            }

            var model = new
            {
                status,
                message
            };

            return Json(model);
        }
        public JsonResult SearchLubeTruck(string q)
        {
            var model = _context.LubeTrucks
                .Where(a => a.Status != "Deleted")
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
        [HttpPost]

        public ActionResult Delete(int id)
        {
            string message = "";
            string status = "";
            try
            {
                LubeTruck item = _context.LubeTrucks.Find(id);
                item.Status = "Deleted_" + DateTime.Now.Date.Ticks;
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


                for (int i = 0; i < 3; i++)
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

                _context.LubeTrucks
                .Where(a => a.Status == "Active")

                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.LubeTrucks
                .Where(a => a.Status == "Active")
              .Where(strFilter)

              //.OrderBy(a => a.FileDate).ThenBy(a => a.Hour)
              .Skip(skip).Take(pageSize)
              .Select(a => new
              {
                  a.No,
                  a.Description,
                  a.OldId,

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
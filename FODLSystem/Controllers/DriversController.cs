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
    public class DriversController : Controller
    {
        private readonly FODLSystemContext _context;

        public DriversController(FODLSystemContext context)
        {
            _context = context;
        }

        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            this.SetCurrentBreadCrumbTitle("Item");
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveItem(int ID,string IdNumber, string Name,string Position, string Status)
        {
            string status = "";
            string message = "";
            int itemid = 0;
            try
            {
                if (ID == 0)
                {

                    var item = new Driver
                    {
                        IdNumber = IdNumber,
                        Name = Name,
                        Position = Position,
                        Status = "Enabled",

                    };
                    
                    _context.Add(item);
                    _context.SaveChanges();
                    itemid = item.ID;
                }
                else
                {
                    var item = _context.Drivers.Find(ID);
                    item.IdNumber = IdNumber;
                    item.Name = Name;
                    item.Position = Position;
                    item.Status = Status;
                    item.DateModified = DateTime.Now;
                    _context.Entry(item).State = EntityState.Modified;
                    _context.SaveChanges();

                    itemid = ID;
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
                message,
                itemid
        };

            return Json(model);
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

                _context.Drivers
                //.Where(a => a.Status == "Active")
               
                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.Drivers
              //.Where(a => a.Status == "Active")
              .Where(strFilter)
              
              .Skip(skip).Take(pageSize)
              .Select(a => new
              {
                  a.IdNumber,
                  a.Name,
                  a.Position,
                  a.Status,
                  a.ID
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
                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal = recordsTotal, data = data};
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
    }
    
}
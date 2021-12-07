using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FODLSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly FODLSystemContext _context;
        public ReportsController(FODLSystemContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string status = "Active,Default";
            string[] stat = status.Split(',').Select(n => n).ToArray();
            var lube = _context.LubeTrucks.Where(a => stat.Contains(a.Status)).Select(a => new
            {
                a.Id,
                Text = a.No + " - " + a.Description
            });
            ViewData["LubeTruckId"] = new SelectList(lube.OrderBy(a => a.Id), "Id", "Text");


           
            var disp = _context.Dispensers.Where(a => stat.Contains(a.Status)).Select(a => new
            {
                a.Id,
                Text = a.No + " - " + a.Name
            });
            ViewData["DispenserId"] = new SelectList(disp.OrderBy(a => a.Id), "Id", "Text");



            return View();
        }
        [HttpPost]
        public ActionResult getDataSummary(DateTime strStart, DateTime end,int lube,int disp)
        {
            string status = "";
           

            try
            {

                //DateTime startDate = DateTime.ParseExact(strStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //DateTime endDate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);





                var v =

               _context.FuelOilSubDetails
                   //.Where(a => fuelid.Contains(a.FuelOilDetails.FuelOilId))
                   .Where(a => a.FuelOilDetails.FuelOils.LubeTruckId == lube)
                   .Where(a => a.FuelOilDetails.FuelOils.DispenserId == disp)

                  .Where(a => a.FuelOilDetails.FuelOils.TransactionDate >= strStart && a.FuelOilDetails.FuelOils.TransactionDate <= end)
                  .Where(a => a.Status == "Active")
                .Where(a => a.FuelOilDetails.Status == "Active")
                  .Select(a => new
                  {
                      a.FuelOilDetails.FuelOils.ReferenceNo,

                      a.FuelOilDetails.FuelOils.Shift
                      ,SourceNo = a.FuelOilDetails.FuelOils.LubeTrucks.No == "na" ? a.FuelOilDetails.FuelOils.Dispensers.Name : a.FuelOilDetails.FuelOils.LubeTrucks.No
                      ,EquipmentNo = a.FuelOilDetails.Equipments.No
                      ,
                      EntryType = "Negative Adjmt.",
                      ItemNo = a.Items.No,
                      PostingDate = a.FuelOilDetails.FuelOils.TransactionDate,
                      DocumentDate = a.FuelOilDetails.FuelOils.CreatedDate,
                      Qty = a.VolumeQty,
                      EquipmentCode = a.FuelOilDetails.Equipments.No,
                      a.FuelOilDetails.Locations.OfficeCode,
                      FuelCode = a.Items.TypeFuel == "OIL-LUBE" ? a.FuelOilDetails.Equipments.FuelCodeOil : a.FuelOilDetails.Equipments.FuelCodeDiesel,
                      LocationCode = "SMPC-SITE",
                      a.FuelOilDetails.Equipments.DepartmentCode,
                      a.Id,
                      a.Status,
                      a.FuelOilDetailId
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
            catch (Exception ex)
            {
                var model = new
                {
                    status = "fail"
                 ,
                    message = ex.Message
                };
                return Json(model);
            }
        }
        public IActionResult printReport(ReportViewModel rvm)
        {

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                byte[] bytes = null;
                string xstring = JsonConvert.SerializeObject(rvm);



                string urilive = "http://californium/FODLApi/api/printreport?rvm=";
                string uridev = "http://sodium2/fodlapi/api/printreport?rvm=";
                string uridevminesite = "http://192.168.0.199/fodlapi/api/printreport?rvm=";
                string urilocal = "http://localhost:59455/api/printreport?rvm=";

                response = client.GetAsync(urilocal + xstring).Result;
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
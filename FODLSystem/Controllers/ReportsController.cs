using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FODLSystem.Models.View_Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FODLSystem.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
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



                string urilive = "http://californium/FODLApi/api/printreport?rvm=";
                string uridev = "http://sodium2/fodlapi/api/printreport?rvm=";
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
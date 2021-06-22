using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;

namespace FODLSystem.Controllers
{
    public class NotifyController : Controller
    {
        private readonly SEMSystemContext _context;
        public NotifyController(SEMSystemContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public string SendNotification(string docstatus, string equipmenttype, int id)
        {
            int compid = 0;
            var notify = new NotifyViewModel();
            var _area = _context.Areas;

            switch (equipmenttype)
            {
                case "fe":
                    var _fe = _context.FireExtinguisherHeaders.Where(a => a.Id == id).FirstOrDefault();
                    var areafe = _area.Find(_fe.AreaId);
                    notify.Area = areafe.Name;
                    notify.CompanyId = areafe.CompanyId;
                    notify.DocumentStatus = docstatus;
                    notify.Equipment = "Fire Extinguisher";

                    notify.ReferenceNo = _fe.ReferenceNo;
                    break;
                case "el":
                    var _el = _context.EmergencyLightHeaders.Where(a => a.Id == id).FirstOrDefault();
                    var areael = _area.Find(_el.AreaId);
                    notify.Area = areael.Name;
                    notify.CompanyId = areael.CompanyId;
                    notify.DocumentStatus = docstatus;
                    notify.Equipment = "Emergency Light";

                    notify.ReferenceNo = _el.ReferenceNo;
                    break;
                case "it":
                    var _it = _context.InergenTankHeaders.Where(a => a.Id == id).FirstOrDefault();
                    var areait = _area.Find(_it.AreaId);
                    notify.Area = areait.Name;
                    notify.CompanyId = areait.CompanyId;
                    notify.DocumentStatus = docstatus;
                    notify.Equipment = "Inergen Tank";

                    notify.ReferenceNo = _it.ReferenceNo;
                    break;
                case "fh":
                    var _fh = _context.FireHydrantHeaders.Where(a => a.Id == id).FirstOrDefault();
                    var areafh = _area.Find(_fh.AreaId);
                    notify.Area = areafh.Name;
                    notify.CompanyId = areafh.CompanyId;
                    notify.DocumentStatus = docstatus;
                    notify.Equipment = "Fire Hydrant";

                    notify.ReferenceNo = _fh.ReferenceNo;
                    break;

                case "bc":
                    var _bc = _context.BicycleEntryHeaders.Include(a=>a.Bicycles.Departments).Where(a => a.Id == id).FirstOrDefault();
                    notify.CompanyId = _bc.Bicycles.Departments.CompanyId;
                    notify.Equipment = "Bicycle";

                    notify.ReferenceNo = _bc.ReferenceNo;
                    break;

            };


            string status = "";

            string message = "";
            if (docstatus != "Approved")
            {
                message = SendEmail(notify);
            }

            if (message != "success")
            {
                status = "fail";
            }
            else
            {
                status = "success";
            }


            return message;
        }
        private string SendEmail(NotifyViewModel nvm)
        {

            string email = "";

            string message = "";
            string rply = "";
            string revieweremail = "";
            //string approveremail = "hblucy@semirarampc.com";
            string approveremail = "rpgustilo@semirarampc.com";
            string recipient = "";


            try
            {
                switch (nvm.DocumentStatus)
                {
                    case "For Review":
                        if (nvm.CompanyId == 1) //slpgc
                        {
                            ////gbarroyo
                            //revieweremail = "gbarroyo@slpowergen.com";
                            revieweremail = "kcmalapit@semirarampc.com";
                        }
                        else
                        {
                            ////eccueto
                            //revieweremail = "eccueto@semcalaca.com";
                            revieweremail = "rpgustilo@semirarampc.com";
                        }
                        message = "There's a form For Review for area " + nvm.Area + " with Reference No: " + nvm.ReferenceNo ;
                        break;
                    case "For Approval":
                        message = "There's a form For Approval for area " + nvm.Area + " with Reference No: " + nvm.ReferenceNo ;
                        break;
                    default:
                        break;
                }

               

                string msg = "Hi, <br /><br />" + message + " <br /><br />";

                var body = msg;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("webhelpdeskadmin@semcalaca.com", "Safety Equipment Monitoring System");


                if (nvm.DocumentStatus == "For Review")
                {
                    mail.To.Add(new MailAddress(revieweremail));
                    recipient = revieweremail;
                }
                else
                {
                    mail.To.Add(new MailAddress(approveremail));
                    recipient = approveremail;
                }



                mail.Subject = "Safety Equipment Monitoring System" + " - " + nvm.DocumentStatus.ToUpper();
                mail.Body = string.Format(body + " Click on this link to view details. http://192.168.30.182/SEM/");
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient()) //mail server
                {
                    try
                    {
                        smtp.Host = "mail.cpcaccess.com";
                        smtp.Credentials = new System.Net.NetworkCredential("webhelpdeskadmin@semcalaca.com", "System@1");
                        smtp.Port = 587;
                        smtp.EnableSsl = false;
                        smtp.Send(mail);
                        rply = "success";

                    }
                    catch (Exception e)
                    {
                        WriteLog(e.Message);
                        rply = e.Message;

                    }

                }


            }
            catch (Exception e)
            {
                WriteLog(e.Message);
                rply = e.Message.ToString();
            }

            Log log = new Log();
            log.Action = "Send Email";
            log.Descriptions = "Send EMAIL by Header Referenceno : " + nvm.ReferenceNo + ", Recipient : " + recipient;
            log.Status = rply;
            _context.Logs.Add(log);
            _context.SaveChanges();

            return rply;
        }
        private void WriteLog(string text)
        {
            text.WriteLog();
        }
    }

}
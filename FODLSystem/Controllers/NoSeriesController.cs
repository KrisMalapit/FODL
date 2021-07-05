using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FODLSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace FODLSystem.Controllers
{
    public class NoSeriesController : Controller
    {
        private readonly FODLSystemContext _context;

        public NoSeriesController(FODLSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public string GetNoSeries(string Code)
        {
            string str = "";
            int number = 0;

            try
            {
                //new setup for num series
                string lno = _context.NoSeries.Where(i => i.Code == Code).Select(n => n.LastNoUsed).FirstOrDefault();
                if (lno == null)
                {
                    var ns = new NoSeries
                    {
                        Code = Code,
                        LastNoUsed = "00000",
                        DateCreated = DateTime.Now.Date,
                        DateUpdated = DateTime.Now.Date
                    };
                    _context.Add(ns);
                    _context.SaveChanges();

                    number = 0;

                }
                else
                {
                    number = Convert.ToInt32(lno);
                }


                number += 1;

                str = number.ToString("D5");

            }
            catch (Exception e)
            {

                throw;
            }


            return str;
        }
        public string UpdateNoSeries(string str, string Code)
        {


            NoSeries ns = _context.NoSeries.SingleOrDefault(v => v.Code == Code);
            ns.LastNoUsed = str;
            ns.DateUpdated = DateTime.Now;
            _context.SaveChanges();
            return "Ok";


        }
    }
}
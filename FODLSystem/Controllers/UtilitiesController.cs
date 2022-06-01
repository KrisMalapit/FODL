using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using FODLSystem.Models.View_Model;
using LinqToExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace FODLSystem.Controllers
{
    public class UtilitiesController : Controller
    {
        private readonly FODLSystemContext _context;

        public UtilitiesController(FODLSystemContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Synchronize", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Synchronize()
        {
            this.SetCurrentBreadCrumbTitle("Synchronize");
            return View();
        }
        public IActionResult DownloadExcel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "syncdata_" + DateTime.Now.ToString("MMddyyyy") + ".xlsx";
            try
            {





                var depts = _context.Departments; //done
                var users = _context.Users; //done
                var items = _context.Items; 
                var components = _context.Components;
                var dispensers = _context.Dispensers;
                var equipments = _context.Equipments;
                var lubetrucks = _context.LubeTrucks;
                var drivers = _context.Drivers;

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Department");
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Code";
                    worksheet.Cell(1, 3).Value = "Name";
                    worksheet.Cell(1, 4).Value = "Status";
                    worksheet.Cell(1, 5).Value = "CompanyId";
                    int index = 1;

                    foreach (var item in depts)
                    {
                        worksheet.Cell(index + 1, 1).Value = item.ID;
                        worksheet.Cell(index + 1, 2).Value = item.Code;
                        worksheet.Cell(index + 1, 3).Value = item.Name;
                        worksheet.Cell(index + 1, 4).Value = item.Status;
                        worksheet.Cell(index + 1, 5).Value = item.CompanyId;
                        index++;
                    }


                    IXLWorksheet worksheet2 =
                     workbook.Worksheets.Add("Users");
                    worksheet2.Cell(1, 1).Value = "Id";
                    worksheet2.Cell(1, 2).Value = "Username";
                    worksheet2.Cell(1, 3).Value = "RoleId";
                    worksheet2.Cell(1, 4).Value = "Password";
                    worksheet2.Cell(1, 5).Value = "FirstName";
                    worksheet2.Cell(1, 6).Value = "LastName";
                    worksheet2.Cell(1, 7).Value = "Name";
                    worksheet2.Cell(1, 8).Value = "Status";
                    worksheet2.Cell(1, 9).Value = "Email";
                    worksheet2.Cell(1, 10).Value = "Domain";
                    worksheet2.Cell(1, 11).Value = "CompanyAccess";
                    worksheet2.Cell(1, 12).Value = "UserType";
                    worksheet2.Cell(1, 13).Value = "DepartmentId";
                    worksheet2.Cell(1, 14).Value = "DispenserAccess";
                    worksheet2.Cell(1, 15).Value = "LubeAccess";
                    worksheet2.Cell(1, 16).Value = "DateModified";
                    index = 1;

                    foreach (var item in users)
                    {
                        worksheet2.Cell(index + 1, 1).Value = item.Id;
                        worksheet2.Cell(index + 1, 2).Value = "'" + item.Username;
                        worksheet2.Cell(index + 1, 3).Value = item.RoleId;
                        worksheet2.Cell(index + 1, 4).Value = "'" + item.Password;
                        worksheet2.Cell(index + 1, 5).Value = "'" + item.FirstName;
                        worksheet2.Cell(index + 1, 6).Value = "'" + item.LastName;
                        worksheet2.Cell(index + 1, 7).Value = "'" + item.Name;
                        worksheet2.Cell(index + 1, 8).Value = "'" + item.Status;
                        worksheet2.Cell(index + 1, 9).Value = "'" + item.Email;
                        worksheet2.Cell(index + 1, 10).Value = "'" + item.Domain;
                        worksheet2.Cell(index + 1, 11).Value = "'" + item.CompanyAccess;
                        worksheet2.Cell(index + 1, 12).Value = "'" + item.UserType;
                        worksheet2.Cell(index + 1, 13).Value = item.DepartmentId;
                        worksheet2.Cell(index + 1, 14).Value = "'" + item.DispenserAccess;
                        worksheet2.Cell(index + 1, 15).Value = "'" + item.LubeAccess;
                        worksheet2.Cell(index + 1, 16).Value = "'" + item.DateModified;
                        index++;
                    }

                    IXLWorksheet wsItem =
                     workbook.Worksheets.Add("Items");
                    wsItem.Cell(1, 1).Value = "Id";
                    wsItem.Cell(1, 2).Value = "No";
                    wsItem.Cell(1, 3).Value = "Description";
                    wsItem.Cell(1, 4).Value = "Description2";
                    wsItem.Cell(1, 5).Value = "TypeFuel";
                    wsItem.Cell(1, 6).Value = "DescriptionLiquidation";
                    wsItem.Cell(1, 7).Value = "Status";
                    wsItem.Cell(1, 8).Value = "DateModified";
                   

                    index = 1;

                    foreach (var item in items)
                    {
                        wsItem.Cell(index + 1, 1).Value = item.Id;
                        wsItem.Cell(index + 1, 2).Value = "'" + item.No;
                        wsItem.Cell(index + 1, 3).Value = "'" + item.Description;
                        wsItem.Cell(index + 1, 4).Value = "'" + item.Description2;
                        wsItem.Cell(index + 1, 5).Value = "'" + item.TypeFuel;
                        wsItem.Cell(index + 1, 6).Value = "'" + item.DescriptionLiquidation;
                        wsItem.Cell(index + 1, 7).Value = "'" + item.Status;
                        wsItem.Cell(index + 1, 8).Value = "'" + item.DateModified;
                        index++;
                    }

                    IXLWorksheet wsComponents =
                     workbook.Worksheets.Add("Components");
                    wsComponents.Cell(1, 1).Value = "Id";
                    wsComponents.Cell(1, 2).Value = "Name";
                    wsComponents.Cell(1, 3).Value = "Status";
                    wsComponents.Cell(1, 4).Value = "DateModified";
                   


                    index = 1;

                    foreach (var item in components)
                    {
                        wsComponents.Cell(index + 1, 1).Value = item.Id;
                        wsComponents.Cell(index + 1, 2).Value = item.Name;
                        wsComponents.Cell(index + 1, 3).Value = item.Status;
                        wsComponents.Cell(index + 1, 4).Value = item.DateModified;
                       
                        index++;
                    }

                    IXLWorksheet wsDispensers =
                    workbook.Worksheets.Add("Dispensers");
                    wsDispensers.Cell(1, 1).Value = "Id";
                    wsDispensers.Cell(1, 2).Value = "Name";
                    wsDispensers.Cell(1, 3).Value = "Status";
                    wsDispensers.Cell(1, 4).Value = "DateModified";
                    wsDispensers.Cell(1, 5).Value = "No";

                    index = 1;


                    foreach (var item in dispensers)
                    {
                        wsDispensers.Cell(index + 1, 1).Value = item.Id;
                        wsDispensers.Cell(index + 1, 2).Value = "'" + item.Name;
                        wsDispensers.Cell(index + 1, 3).Value = "'" + item.Status;
                        wsDispensers.Cell(index + 1, 4).Value = "'" + item.DateModified;
                        wsDispensers.Cell(index + 1, 5).Value = "'" + item.No;
                        index++;
                    }




                    IXLWorksheet wsEquipments =
                    workbook.Worksheets.Add("Equipments");
                    wsEquipments.Cell(1, 1).Value = "Id";
                    wsEquipments.Cell(1, 2).Value = "No";
                    wsEquipments.Cell(1, 3).Value = "Name";
                    wsEquipments.Cell(1, 4).Value = "ModelNo";
                    wsEquipments.Cell(1, 5).Value = "Status";
                    wsEquipments.Cell(1, 6).Value = "DepartmentCode";
                    wsEquipments.Cell(1, 7).Value = "FuelCodeDiesel";
                    wsEquipments.Cell(1, 8).Value = "FuelCodeOil";
                    wsEquipments.Cell(1, 9).Value = "DateModified";

                    index = 1;

                    foreach (var item in equipments)
                    {
                        wsEquipments.Cell(index + 1, 1).Value = item.Id;
                        wsEquipments.Cell(index + 1, 2).Value = "'" + item.No;
                        wsEquipments.Cell(index + 1, 3).Value = "'" + item.Name;
                        wsEquipments.Cell(index + 1, 4).Value = "'" + item.ModelNo;
                        wsEquipments.Cell(index + 1, 5).Value = "'" + item.Status;
                        wsEquipments.Cell(index + 1, 6).Value = "'" + item.DepartmentCode;
                        wsEquipments.Cell(index + 1, 7).Value = "'" + item.FuelCodeDiesel;
                        wsEquipments.Cell(index + 1, 8).Value = "'" + item.FuelCodeOil;
                        wsEquipments.Cell(index + 1, 9).Value = "'" + item.DateModified;
                        index++;
                    }


                    IXLWorksheet wsLubetrucks =
                    workbook.Worksheets.Add("Lubetrucks");
                    wsLubetrucks.Cell(1, 1).Value = "Id";
                    wsLubetrucks.Cell(1, 2).Value = "No";
                    wsLubetrucks.Cell(1, 3).Value = "OldId";
                    wsLubetrucks.Cell(1, 4).Value = "Description";
                    wsLubetrucks.Cell(1, 5).Value = "Status";
                    wsLubetrucks.Cell(1, 6).Value = "DateModified";
                    index = 1;

                    foreach (var item in lubetrucks)
                    {
                        wsLubetrucks.Cell(index + 1, 1).Value = item.Id;
                        wsLubetrucks.Cell(index + 1, 2).Value = "'" + item.No;
                        wsLubetrucks.Cell(index + 1, 3).Value = "'" + item.OldId;
                        wsLubetrucks.Cell(index + 1, 4).Value = "'" + item.Description;
                        wsLubetrucks.Cell(index + 1, 5).Value = "'" + item.Status;
                        wsLubetrucks.Cell(index + 1, 6).Value = "'" + item.DateModified;
                        index++;
                    }


                    IXLWorksheet wsDrivers =
                    workbook.Worksheets.Add("Drivers");
                    wsDrivers.Cell(1, 1).Value = "ID";
                    wsDrivers.Cell(1, 2).Value = "IdNumber";
                    wsDrivers.Cell(1, 3).Value = "Name";
                    wsDrivers.Cell(1, 4).Value = "Position";
                    wsDrivers.Cell(1, 5).Value = "Status";
                    wsDrivers.Cell(1, 6).Value = "DateModified";
                    index = 1;

                    foreach (var item in drivers)
                    {
                        wsDrivers.Cell(index + 1, 1).Value = item.ID;
                        wsDrivers.Cell(index + 1, 2).Value = "'" + item.IdNumber;
                        wsDrivers.Cell(index + 1, 3).Value = "'" + item.Name;
                        wsDrivers.Cell(index + 1, 4).Value = "'" + item.Position;
                        wsDrivers.Cell(index + 1, 5).Value = "'" + item.Status;
                        wsDrivers.Cell(index + 1, 6).Value = "'" + item.DateModified;
                        index++;
                    }


                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }

                    

                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IActionResult UploadExcel()
        {
            string filePath = "";
            int hid = 0;
            string message = "";
            string status = "";
            IFormFile file = Request.Form.Files[0];
            string transferExcel;

            try
            {

                string strFilename = Path.GetFileNameWithoutExtension(file.FileName);

                int cntRec = _context.FileUploads.Where(a => a.FileName == strFilename).Count();
                if (cntRec > 0)
                {
                    var err = new
                    {
                        status = "failed",
                        message = "File already uploaded"
                    };

                   
                    return Json(err);
                }


                StringBuilder sb = new StringBuilder();
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    ISheet sheet2;
                    ISheet sheet3;
                    ISheet sheet4;
                    ISheet sheet5;
                    ISheet sheet6;
                    ISheet sheet7;
                    ISheet sheet8;



                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\fileuploads\", file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                           
                            sheet = hssfwb.GetSheet("Department"); //get first sheet from workbook 
                            sheet2 = hssfwb.GetSheet("Users");
                            sheet3 = hssfwb.GetSheet("Items");
                            sheet4 = hssfwb.GetSheet("Components");
                            sheet5 = hssfwb.GetSheet("Dispensers");
                            sheet6 = hssfwb.GetSheet("Equipments");
                            sheet7 = hssfwb.GetSheet("Lubetrucks");
                            sheet8 = hssfwb.GetSheet("Drivers");


                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                           
                            sheet = hssfwb.GetSheet("Department"); //get first sheet from workbook 
                            sheet2 = hssfwb.GetSheet("Users");
                            sheet3 = hssfwb.GetSheet("Items");
                            sheet4 = hssfwb.GetSheet("Components");
                            sheet5 = hssfwb.GetSheet("Dispensers");
                            sheet6 = hssfwb.GetSheet("Equipments");
                            sheet7 = hssfwb.GetSheet("Lubetrucks");
                            sheet8 = hssfwb.GetSheet("Drivers");

                        }

                      
                       

                        transferExcel = UploadExcelFinal(sheet, sheet2, sheet3, sheet4, sheet5, sheet6, sheet7,sheet8, fullPath);
                        
                    }
                    if (transferExcel == "success")
                    {
                        status = "success";
                        message = "Uploaded successfully!";
                    }
                    else
                    {
                        status = "failed";
                        message = transferExcel;
                    }
                    

                }
                
            }
            catch (Exception e)
            {



                status = "failed";
                message = e.Message.ToString();
            }

            var model = new
            {
                status,
                message
            };

            filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\fileuploads\" + file.FileName);
            System.IO.File.Delete(filePath);

            return Json(model);
        }
        public string UploadExcelFinal(ISheet sheet, ISheet sheet2, ISheet sheet3, ISheet sheet4, ISheet sheet5, ISheet sheet6, ISheet sheet7, ISheet sheet8, string fileName)
        {

            string status = "success";
            try
            {
                try
                {
                    int rowCount = sheet.LastRowNum;
                    int deptCount = 0;
                    int userCount = 0;
                    int dispenserCount = 0;
                    int itemCount = 0;
                    int componentCount = 0;
                    int lubetruckCount = 0;
                    int equipmentCount = 0;
                    int driverCount = 0;


                    //DEPARTMENT
                    deptCount = _context.Departments.Count();
                   

                    if (rowCount == deptCount)
                    {
                       
                    }
                    else
                    {
                        int cnt = 0;
                        int line = 1;

                        
                        //List<Department> svm = new List<Department>();
                        for (int i = (deptCount + 1); i <= rowCount; i++)
                        {

                            cnt = 0;
                            line = 1;
                            IRow headerRow = sheet.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                if (line > 5)
                                {
                                    break;
                                }


                                clc[cnt] = headerRow.GetCell(j).ToString();


                                cnt += 1;
                                if (cnt == 5)
                                {

                                    if (true)
                                    {

                                    }

                                    Department sv = new Department
                                    {
                                        Code = clc[1],
                                        Name = clc[2],
                                        Status = clc[3],
                                        CompanyId = Convert.ToInt32(clc[4])

                                    };

                                    _context.Departments.Add(sv);


                                    line += 1;

                                }

                            }
                           
                        }


                        _context.SaveChanges();



                    }

                    //USER
                    rowCount = sheet2.LastRowNum;
                    userCount = _context.Users.Count();
                    if (rowCount == userCount)
                    {
                       
                    }
                    else
                    {
                        int cnt = 0;
                        int line = 1;
                       
                       
                        for (int i = (userCount + 1); i <= rowCount; i++)
                        {

                            cnt = 0;
                            line = 1;
                            IRow headerRow = sheet2.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                


                                clc[cnt] = headerRow.GetCell(j).ToString();


                                cnt += 1;
                                if (cnt == 16)
                                {



                                    User sv = new User
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        Username = clc[1],
                                        RoleId = Convert.ToInt32(clc[2]),
                                        Password = clc[3],
                                        FirstName = clc[4],
                                        LastName = clc[5],
                                        Name = clc[6],
                                        Status = clc[7],
                                        Email = clc[8],
                                        Domain = clc[9],
                                        CompanyAccess = clc[10],
                                        UserType = clc[11],
                                        DepartmentId = Convert.ToInt32(clc[12]),
                                        DispenserAccess = clc[13],
                                        LubeAccess = clc[14],
                                        DateModified = DateTime.Now
                                    };
                                   
                                    _context.Users.Add(sv);


                             

                                }

                            }
                            
                        }
                        
                        _context.SaveChanges();



                    }


                    //ITEMS
                    rowCount = sheet3.LastRowNum;
                    componentCount = _context.Items.Count();
                    if (rowCount == componentCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                     
                      
                        //List<User> usr = new List<User>();
                        for (int i = (componentCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;
                         
                            IRow headerRow = sheet3.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 7)
                                {

                                    Item sv = new Item
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        No = clc[1],
                                        Description = clc[2],
                                        Description2 = clc[3],
                                        TypeFuel = clc[4],
                                        DescriptionLiquidation = clc[5],
                                        Status = clc[6],
                                        DateModified = DateTime.Now
                                    };

                                    _context.Items.Add(sv);




                                }

                            }

                        }
                        _context.SaveChanges();



                    }

                    //Components
                    rowCount = sheet4.LastRowNum;
                    componentCount = _context.Components.Count();
                    if (rowCount == componentCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                        //List<User> usr = new List<User>();
                        for (int i = (componentCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;

                            IRow headerRow = sheet4.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 4)
                                {

                                    Component sv = new Component
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        Name = clc[1],
                                       
                                        Status = clc[2],
                                    };

                                    _context.Components.Add(sv);
                                }

                            }

                        }
                        _context.SaveChanges();
                    }

                    //Dispensers
                    rowCount = sheet5.LastRowNum;
                    dispenserCount = _context.Dispensers.Count();
                    if (rowCount == dispenserCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                        //List<User> usr = new List<User>();
                        for (int i = (dispenserCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;

                            IRow headerRow = sheet5.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 5)
                                {

                                    Dispenser sv = new Dispenser
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        No = clc[4],
                                        Name = clc[1],

                                        Status = clc[2],
                                        DateModified = DateTime.Now
                                    };

                                    _context.Dispensers.Add(sv);
                                }

                            }

                        }
                        _context.SaveChanges();
                    }

                    //Equipments
                    rowCount = sheet6.LastRowNum;
                    equipmentCount = _context.Equipments.Count();
                    if (rowCount == equipmentCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                        //List<User> usr = new List<User>();
                        for (int i = (equipmentCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;

                            IRow headerRow = sheet6.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 9)
                                {

                                    Equipment sv = new Equipment
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        No = clc[1],
                                        Name = clc[2],
                                        ModelNo = clc[3],
                                        Status = clc[4],
                                        DepartmentCode = clc[5],
                                        FuelCodeDiesel = clc[6],
                                        FuelCodeOil = clc[7],
                                    };

                                    _context.Equipments.Add(sv);
                                }

                            }

                        }
                        _context.SaveChanges();
                    }


                    //Lubetrucks
                    rowCount = sheet7.LastRowNum;
                    lubetruckCount = _context.LubeTrucks.Count();
                    if (rowCount == lubetruckCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                        //List<User> usr = new List<User>();
                        for (int i = (lubetruckCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;

                            IRow headerRow = sheet7.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 6)
                                {

                                    LubeTruck sv = new LubeTruck
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        No = clc[1],
                                        OldId = clc[2],
                                        Description = clc[3],
                                        Status = clc[4],
                                       
                                    };

                                    _context.LubeTrucks.Add(sv);
                                }

                            }

                        }
                        _context.SaveChanges();
                    }

                    //Drivers
                    rowCount = sheet8.LastRowNum;
                    driverCount = _context.Drivers.Count();
                    if (rowCount == driverCount)
                    {

                    }
                    else
                    {
                        int cnt = 0;
                        //List<User> usr = new List<User>();
                        for (int i = (driverCount + 1); i <= rowCount; i++)
                        {
                            cnt = 0;

                            IRow headerRow = sheet8.GetRow(i); //Get Header Row
                            int cellCount = headerRow.LastCellNum;
                            string[] clc = new string[cellCount];

                            for (int j = 0; j < (cellCount); j++)
                            {
                                clc[cnt] = headerRow.GetCell(j).ToString();
                                cnt += 1;
                                if (cnt == 6)
                                {
                                    var defdDate = new DateTime(1900, 01, 01);
                                    DateTime dmodified = string.IsNullOrEmpty(clc[5].ToString()) ? defdDate : Convert.ToDateTime(clc[5]); 
                                    Driver sv = new Driver
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        IdNumber = clc[1],
                                        Name = clc[2],
                                        Position = clc[3],
                                        Status = clc[4],
                                        DateModified = dmodified,


                                    };

                                    _context.Drivers.Add(sv);
                                }

                            }

                        }
                        _context.SaveChanges();
                    }

                    //var dtItems = ConvertToDatatable(sheet3);
                    //var importdata = from row in dtItems.AsEnumerable()
                    //                 select new Item()
                    //                 {
                    //                     Id = row.Field<int>(0),
                    //                     No = row.Field<string>(1),
                    //                     Description = row.Field<string>(2),
                    //                     Description2 = row.Field<string>(3),
                    //                     TypeFuel = row.Field<string>(4),
                    //                     DescriptionLiquidation = row.Field<string>(5),
                    //                     Status = row.Field<string>(6),
                    //                     DateModified = row.Field<DateTime>(7),
                    //                 };

                    //IQueryable<IImportRow> data = importdata as IQueryable<IImportRow>;


                    DateTime lastDateModified = DateTime.Now;

                    var si = _context.SynchronizeInformations.Find(1);
                    if (si != null)
                    {
                        lastDateModified = si.LastModifiedDate;
                        si.LastModifiedDate = DateTime.Now;
                        si.ModifiedBy = User.Identity.GetFullName();
                        _context.SynchronizeInformations.Update(si); 
                    }
                    else
                    {
                        SynchronizeInformation sIn = new SynchronizeInformation();
                        sIn.LastModifiedDate = DateTime.Now;
                        sIn.ModifiedBy = User.Identity.GetFullName();
                        _context.Add(sIn);
                        _context.SaveChanges();

                        lastDateModified = si.LastModifiedDate;
                    }


                    var userStatus = UpdateUsers(fileName, "Users", lastDateModified); //Users
                    

                    var itemStatus = UpdateItems(fileName, "Items", lastDateModified); //Items
           

                    var componentsStatus = UpdateComponents(fileName, "Components", lastDateModified); //Components

                    var dispensersStatus = UpdateDispensers(fileName, "Dispensers", lastDateModified); //Dispensers

                    var equipmentsStatus = UpdateEquipments(fileName, "Equipments", lastDateModified); //Equipments
                    var lubetrucksStatus = UpdateLubeTrucks(fileName, "Lubetrucks", lastDateModified); //Lubetrucks
                    var driversStatus = UpdateDrivers(fileName, "Drivers", lastDateModified); //Lubetrucks

                    Log log = new Log
                    {
                        Action = "Upload",
                        CreatedDate = DateTime.Now,
                        Descriptions = "Upload Excel File Synchronize. Users : " + userStatus + "  Item : " + itemStatus + " Components : " + componentsStatus + " Dispensers : " + dispensersStatus +
                                        " Equipments : " + equipmentsStatus + " Lubetrucks : " + lubetrucksStatus ,
                        Status = "success",
                        UserId = User.Identity.GetUserId().ToString()
                    };

                    _context.Add(log);
                    _context.SaveChanges();

                    return status;
                }
                catch (Exception e)
                {
                    status = e.ToString();
                    return status;
                }
            }
            catch (Exception e)
            {

                status = e.ToString();
                return status;
            }


        }
        string UpdateUsers(string fileName, string sheetName, DateTime LastDateModified)
        {
            string.Format("Update users started..");
            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<User> items = new List<User>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    var dmodified = string.IsNullOrEmpty(dtexcel.Rows[i]["DateModified"].ToString()) ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);
                    string.Format("Row : " + (i+1) + " DateModified " + dmodified);

                    User item = new User();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.Username = dtexcel.Rows[i]["Username"].ToString();
                    item.RoleId = Convert.ToInt32(dtexcel.Rows[i]["RoleId"]);
                    item.Password = dtexcel.Rows[i]["Password"].ToString();
                    item.FirstName = dtexcel.Rows[i]["FirstName"].ToString();
                    item.LastName = dtexcel.Rows[i]["LastName"].ToString();
                    item.Name = dtexcel.Rows[i]["Name"].ToString();
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    item.Email = dtexcel.Rows[i]["Email"].ToString();
                    item.Domain = dtexcel.Rows[i]["Domain"].ToString();
                    item.CompanyAccess = dtexcel.Rows[i]["CompanyAccess"].ToString();
                    item.UserType = dtexcel.Rows[i]["UserType"].ToString();
                    item.DepartmentId = Convert.ToInt32(dtexcel.Rows[i]["DepartmentId"]);
                    item.DispenserAccess = dtexcel.Rows[i]["DispenserAccess"].ToString();
                    item.LubeAccess = dtexcel.Rows[i]["LubeAccess"].ToString();

                    item.DateModified = dmodified;
                   
                    items.Add(item);
                }
              
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);

                int itemCount = itemsToUpdate.Count();


                string.Format("User to be modified Count " + itemCount);


                foreach (var item in itemsToUpdate)
                {
                    int _id = item.Id;
                    string.Format("User Id : " + _id);

                    //if (item.Id == 10)
                    //{
                    //     _id = item.Id;
                    //}

                    try
                    {
                        //var it = _context.Users.Find(item.Id);
                        var it = _context.Users.Where(a=>a.Username == item.Username).FirstOrDefault();
                        it.Username = item.Username;
                        it.RoleId = item.RoleId;
                        it.Password = item.Password;
                        it.FirstName = item.FirstName;
                        it.LastName = item.LastName;
                        it.Name = item.Name;
                        it.Status = item.Status;
                        it.Email = item.Email;
                        it.Domain = item.Domain;
                        it.CompanyAccess = item.CompanyAccess;
                        it.UserType = item.UserType;
                        it.DepartmentId = item.DepartmentId;
                        it.DispenserAccess = item.DispenserAccess;
                        it.LubeAccess = item.LubeAccess;
                        it.DateModified = item.DateModified;
                        //_context.Update(it);
                        _context.Entry(it).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {

                        var err = _id;
                        string s;
                    }
                    
                }

                return "Ok";

            }
            catch (Exception e)
            {
                string.Format("User update error " + e.Message);
                return e.Message;
            }
        }
        
        string UpdateItems(string fileName,string sheetName,DateTime LastDateModified) {

            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<Item> items = new List<Item>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    Item item = new Item();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.No = dtexcel.Rows[i]["No"].ToString();
                    item.Description = dtexcel.Rows[i]["Description"].ToString();
                    item.Description2 = dtexcel.Rows[i]["Description2"].ToString();
                    item.TypeFuel = dtexcel.Rows[i]["TypeFuel"].ToString();
                    item.DescriptionLiquidation = dtexcel.Rows[i]["DescriptionLiquidation"].ToString();
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    item.DateModified = dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);

                    items.Add(item);
                }
                var itemsToUpdate = items.Where(a=>a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
                foreach (var item in itemsToUpdate)
                {
                    var it = _context.Items.Find(item.Id);
                    it.Description = item.Description;
                    it.Description2 = item.Description2;
                    it.DescriptionLiquidation = item.DescriptionLiquidation;
                    it.TypeFuel = item.TypeFuel;
                    it.No = item.No;
                    it.Status = item.Status;
                    //_context.Update(it);
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {
                
                return e.Message;
            }
        }

        string UpdateComponents(string fileName, string sheetName, DateTime LastDateModified)
        {

            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<Component> items = new List<Component>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    Component item = new Component();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.Name = dtexcel.Rows[i]["Name"].ToString();
                    item.DateModified = dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    items.Add(item);
                }
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
                foreach (var item in itemsToUpdate)
                {
                    var it = _context.Components.Find(item.Id);
                    it.Name = item.Name;
                    it.Status = item.Status;
                    //_context.Update(it);
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        string UpdateDispensers(string fileName, string sheetName, DateTime LastDateModified)
        {   

            try
            {
               

                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                

                List<Dispenser> items = new List<Dispenser>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                   
                    var dm =  dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate.ToString() : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"].ToString()).ToString();
                    string.Format("Date Formatted : " + dm).WriteLog();
                    Dispenser item = new Dispenser();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.No = dtexcel.Rows[i]["No"].ToString();
                    item.Name = dtexcel.Rows[i]["Name"].ToString();
                    item.DateModified = Convert.ToDateTime(dm);
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    items.Add(item);
                }

              
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
             

                foreach (var item in itemsToUpdate)
                {
                    var it = _context.Dispensers.Find(item.Id);
                    it.No = item.No;
                    it.Name = item.Name;
                    it.Status = item.Status;
                    it.DateModified = DateTime.Now;
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {
                string.Format("Error Dispenser : " + e.Message).WriteLog();
                return e.Message;
            }
        }
        string UpdateEquipments(string fileName, string sheetName, DateTime LastDateModified)
        {

            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<Equipment> items = new List<Equipment>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    Equipment item = new Equipment();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.No = dtexcel.Rows[i]["No"].ToString();
                    item.Name = dtexcel.Rows[i]["Name"].ToString();
                    item.ModelNo = dtexcel.Rows[i]["ModelNo"].ToString();
                    item.DepartmentCode = dtexcel.Rows[i]["DepartmentCode"].ToString();
                    item.FuelCodeDiesel = dtexcel.Rows[i]["FuelCodeDiesel"].ToString();
                    item.FuelCodeOil = dtexcel.Rows[i]["FuelCodeOil"].ToString();
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    item.DateModified = dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);

                    items.Add(item);
                }
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
                foreach (var item in itemsToUpdate)
                {
                    var it = _context.Equipments.Find(item.Id);
                    it.Name = item.Name;
                    it.ModelNo = item.ModelNo;
                    it.DepartmentCode = item.DepartmentCode;
                    it.FuelCodeDiesel = item.FuelCodeDiesel;
                    it.FuelCodeOil = item.FuelCodeOil;
                    it.No = item.No;
                    it.Status = item.Status;
                    //_context.Update(it);
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        string UpdateLubeTrucks(string fileName, string sheetName, DateTime LastDateModified)
        {

            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<LubeTruck> items = new List<LubeTruck>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    LubeTruck item = new LubeTruck();
                    item.Id = Convert.ToInt32(dtexcel.Rows[i]["Id"]);
                    item.No = dtexcel.Rows[i]["No"].ToString();
                    item.OldId = dtexcel.Rows[i]["OldId"].ToString();
                    item.Description = dtexcel.Rows[i]["Description"].ToString();
                    item.DateModified = dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    items.Add(item);
                }
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
                foreach (var item in itemsToUpdate)
                {
                    var it = _context.LubeTrucks.Find(item.Id);
                    it.No = item.No;
                    it.OldId = item.OldId;
                    it.Description = item.Description;
                    it.Status = item.Status;
                    //_context.Update(it);
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        string UpdateDrivers(string fileName, string sheetName, DateTime LastDateModified)
        {

            try
            {
                FileInfo fs = new FileInfo(fileName);
                ExcelPackage package = new ExcelPackage(fs);
                DataTable dtexcel = new DataTable();
                dtexcel = ExcelToDataTable(package, sheetName);
                DateTime defdDate = new DateTime(1900, 01, 01);

                int dtRows = dtexcel.Rows.Count;
                List<Driver> items = new List<Driver>();
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    Driver item = new Driver();
                    item.ID = Convert.ToInt32(dtexcel.Rows[i]["ID"]);
                    item.IdNumber = dtexcel.Rows[i]["IdNumber"].ToString();
                    item.Name = dtexcel.Rows[i]["Name"].ToString();
                    item.Position = dtexcel.Rows[i]["Position"].ToString();
                    item.DateModified = dtexcel.Rows[i]["DateModified"].ToString() == "" ? defdDate : Convert.ToDateTime(dtexcel.Rows[i]["DateModified"]);
                    item.Status = dtexcel.Rows[i]["Status"].ToString();
                    items.Add(item);
                }
                var itemsToUpdate = items.Where(a => a.DateModified >= LastDateModified);
                int itemCount = itemsToUpdate.Count();
                foreach (var item in itemsToUpdate)
                {
                    var it = _context.Drivers.Find(item.ID);
                    it.IdNumber = item.IdNumber;
                    it.Name = item.Name;
                    it.Position = item.Position;
                    it.Status = item.Status;
                    it.DateModified = item.DateModified;
                    //_context.Update(it);
                    _context.Entry(it).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return "Ok";

            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        static DataTable ExcelToDataTable(ExcelPackage package,string sheetName)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets[sheetName];
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }

        static DataTable ConvertToDatatable(ISheet sheet)
        {
            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();
            //ISheet sheet;
            //using (var stream = new FileStream("TestData.xlsx", FileMode.Open))
            //{
            //    stream.Position = 0;
            //    XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
            //   shtItem = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    {
                        dtTable.Columns.Add(cell.ToString());
                    }
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                rowList.Add(row.GetCell(j).ToString());
                            }
                        }
                    }
                    if (rowList.Count > 0)
                        dtTable.Rows.Add(rowList.ToArray());
                    rowList.Clear();
                }
            //}
            return dtTable;
        }

        public JsonResult uploadNavision(string batchno,string refid)
        {
            string status = "";
            string message = "";
            try
            {
                //string apiUrl = @"http://192.168.0.199/FODLApi/api/"; //SMPC DEV
                string apiUrl = @"http://localhost/fodlapi/api/"; //SMPC Live
                //string apiUrl = @"http://sodium2/FODLApi/api/"; //SMPC DEV
                //string apiUrl = @"http://localhost:59455/api/"; //LOCAL

                NavisionViewModel nvm = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    var responseTask = client.GetAsync("uploadnav?batchno=" + batchno + "&referenceno=" + refid);
                    responseTask.Wait();

                    var response = responseTask.Result;
                    if (response.IsSuccessStatusCode)
                    {


                        var readTask = response.Content.ReadAsAsync<NavisionViewModel>();

                      

                        try
                        {
                            readTask.Wait();

                            nvm = readTask.Result;
                            //if (nvm.message != "success")
                            //{
                            //    status = "failed";
                            //    message = nvm.message;
                            //}
                            //else
                            //{
                            //    status = "success";
                            //    message = "Uploaded to Navision Successfully";
                            //}
                            if (nvm.status == "success")
                            {
                                status = "success";
                                message = "Uploaded to Navision Successfully";
                               
                            }
                            if (nvm.status == "partial_success")
                            {
                                status = "warning";
                                message = nvm.message;

                            }
                            else
                            {
                                status = "failed";
                                message = nvm.message;
                            }


                        }
                        catch (Exception e)
                        {

                            status = "failed";
                            message = e.Message.ToString();

                        }




                    }
                    else
                    {
                        status = "failed";
                        message = response.ReasonPhrase;
                    }
                }



                Log log = new Log();
                log.Action = "Save";
                log.CreatedDate = DateTime.Now;
                log.Descriptions = "Send data to API";
                log.Status = status + " " + message;
                _context.Add(log);
                _context.SaveChanges();

               
            }
            catch (Exception e)
            {

                status = "failed";
                message = e.Message.ToString();
            }
            var model = new
            {
                status, message
            };

            return Json(model);
        }

    }
}
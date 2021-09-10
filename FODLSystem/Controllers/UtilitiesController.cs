using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DNTBreadCrumb.Core;
using FODLSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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

                    index = 1;

                    foreach (var item in users)
                    {
                        worksheet2.Cell(index + 1, 1).Value = item.Id;
                        worksheet2.Cell(index + 1, 2).Value = item.Username;
                        worksheet2.Cell(index + 1, 3).Value = item.RoleId;
                        worksheet2.Cell(index + 1, 4).Value = "'" + item.Password;
                        worksheet2.Cell(index + 1, 5).Value = item.FirstName;
                        worksheet2.Cell(index + 1, 6).Value = item.LastName;
                        worksheet2.Cell(index + 1, 7).Value = item.Name;
                        worksheet2.Cell(index + 1, 8).Value = item.Status;
                        worksheet2.Cell(index + 1, 9).Value = item.Email;
                        worksheet2.Cell(index + 1, 10).Value = item.Domain;
                        worksheet2.Cell(index + 1, 11).Value = "'" + item.CompanyAccess;
                        worksheet2.Cell(index + 1, 12).Value = item.UserType;
                        worksheet2.Cell(index + 1, 13).Value = item.DepartmentId;
                        worksheet2.Cell(index + 1, 14).Value = "'" + item.DispenserAccess;
                        worksheet2.Cell(index + 1, 15).Value = "'" + item.LubeAccess;
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
                        wsItem.Cell(index + 1, 2).Value = item.No;
                        wsItem.Cell(index + 1, 3).Value = "'" + item.Description;
                        wsItem.Cell(index + 1, 4).Value = "'" + item.Description2;
                        wsItem.Cell(index + 1, 5).Value = item.TypeFuel;
                        wsItem.Cell(index + 1, 6).Value = "'" + item.DescriptionLiquidation;
                        wsItem.Cell(index + 1, 7).Value = item.Status;
                        wsItem.Cell(index + 1, 8).Value = item.DateModified;
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


                    index = 1;


                    foreach (var item in dispensers)
                    {
                        wsDispensers.Cell(index + 1, 1).Value = item.Id;
                        wsDispensers.Cell(index + 1, 2).Value = item.Name;
                        wsDispensers.Cell(index + 1, 3).Value = item.Status;
                        wsDispensers.Cell(index + 1, 4).Value = item.DateModified;
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
                        wsEquipments.Cell(index + 1, 2).Value = item.No;
                        wsEquipments.Cell(index + 1, 3).Value = item.Name;
                        wsEquipments.Cell(index + 1, 4).Value = "'" + item.ModelNo;
                        wsEquipments.Cell(index + 1, 5).Value = item.Status;
                        wsEquipments.Cell(index + 1, 6).Value = "'" + item.DepartmentCode;
                        wsEquipments.Cell(index + 1, 7).Value = "'" + item.FuelCodeDiesel;
                        wsEquipments.Cell(index + 1, 8).Value = "'" + item.FuelCodeOil;
                        wsEquipments.Cell(index + 1, 9).Value = "'" + item.FuelCodeOil;
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

                        }

                      
                       

                        var transferExcel = UploadExcelFinal(sheet, sheet2, sheet3, sheet4, sheet5, sheet6, sheet7);
                        
                    }
                }
                status = "success";
                message = "Uploaded successfully!";
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
        public string UploadExcelFinal(ISheet sheet, ISheet sheet2, ISheet sheet3, ISheet sheet4, ISheet sheet5, ISheet sheet6, ISheet sheet7)
        {

            
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
                       
                        //List<User> usr = new List<User>();
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
                                if (cnt == 15)
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
                                if (cnt == 4)
                                {

                                    Dispenser sv = new Dispenser
                                    {
                                        //Id = Convert.ToInt32(clc[0]),
                                        Name = clc[1],

                                        Status = clc[2],
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


                  

















                    var si = _context.SynchronizeInformations.Find(1);
                    if (si != null)
                    {
                        si.LastModifiedDate = DateTime.Now.Date;
                        si.ModifiedBy = User.Identity.GetFullName();
                        _context.SynchronizeInformations.Update(si);
                    }
                    else
                    {
                        SynchronizeInformation sIn = new SynchronizeInformation();
                        sIn.LastModifiedDate = DateTime.Now.Date;
                        sIn.ModifiedBy = User.Identity.GetFullName();
                        _context.Add(sIn);
                        _context.SaveChanges();
                    }
                   

                    Log log = new Log
                    {
                        Action = "Upload",
                        CreatedDate = DateTime.Now,
                        Descriptions = "Upload Excel File Synchronize" ,
                        Status = "success",
                        UserId = User.Identity.GetUserId().ToString()
                    };

                    _context.Add(log);
                    _context.SaveChanges();

                    return "Ok";
                }
                catch (Exception e)
                {

                    return e.ToString();
                }
            }
            catch (Exception e)
            {

                return e.ToString();
            }


        }

    }
}
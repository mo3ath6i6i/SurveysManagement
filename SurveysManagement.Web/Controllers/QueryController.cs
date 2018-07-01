using ClosedXML.Excel;
using SurveysManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveysManagement.Web.Controllers
{
    public class QueryController : BaseController
    {
        // GET: Query
        // GET: Surveys
        [Authorize(Roles = "Admin")]
        public ActionResult Query()
        {
            QueryViewModel model = new QueryViewModel();
            //var surveys = db.Surveys.Include(s => s.Category);
            return View(model);
        }

        // GET: Surveys
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Query(QueryViewModel model)
        { 
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DownloadExcel(string data, string typ)
        {
            string selectedSurveys = data.Replace(",,", ",");

            if (typ == "full")
            {
                var param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                param.Value = selectedSurveys;
                param.ParameterName = "@surveyIds";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SurveyModel"].ConnectionString))
                using (var cmd = new SqlCommand("getSurveyAnswersReport", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(table);
                }
                //XLWorkbook wb = new XLWorkbook();
                //wb.Worksheets.Add(table, "Worksheet");
                ///////////////////////
                string handle = Guid.NewGuid().ToString();
                ///////////////////////
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(table, "Worksheet");
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.Position = 0;
                        TempData[handle] = MyMemoryStream.ToArray();
                    }
                }
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = "TestReportOutput.xlsx" }
                };
            }
            else if (typ == "raw")
            {
                var param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                param.Value = selectedSurveys.Replace(",", "");
                param.ParameterName = "@surveyId";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SurveyModel"].ConnectionString))
                using (var cmd = new SqlCommand("surveys_raw_data", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(table);
                }
                //XLWorkbook wb = new XLWorkbook();
                //wb.Worksheets.Add(table, "Worksheet");
                ///////////////////////
                string handle = Guid.NewGuid().ToString();
                ///////////////////////
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(table, "Worksheet");
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.Position = 0;
                        TempData[handle] = MyMemoryStream.ToArray();
                    }
                }
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = "TestReportOutput.xlsx" }
                };
            }
            else
            {
                return null;
            }
        }


        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
    }
}
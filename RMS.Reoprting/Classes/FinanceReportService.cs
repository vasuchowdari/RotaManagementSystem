using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RMS.Reoprting.ViewModels;

namespace RMS.Reoprting.Classes
{
    public static class FinanceReportService
    {
        private static IAmazonS3 _awss3Client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);

        public static bool GenerateMonthlyPayrollReport(ICollection<MonthlyPayrollReportModel> dataList)
        {
            try
            {
                const string fileName = "Monthly_Payroll_Report.xlsx";

                using (var package = new ExcelPackage(new FileInfo(@fileName)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("PayrollRpt");

                    worksheet.Cells[1, 1].Value = "Pay Period";
                    worksheet.Cells[1, 2].Value = "Base Location";
                    worksheet.Cells[1, 3].Value = "Base Sub-Location";
                    worksheet.Cells[1, 4].Value = "First Name";
                    worksheet.Cells[1, 5].Value = "Last Name";
                    worksheet.Cells[1, 6].Value = "Payroll Reference";
                    worksheet.Cells[1, 7].Value = "RMS Reference";
                    worksheet.Cells[1, 8].Value = "Burberry";
                    worksheet.Cells[1, 9].Value = "Mulberry";
                    worksheet.Cells[1, 10].Value = "Oakley Female";
                    worksheet.Cells[1, 11].Value = "Oakley Male";
                    worksheet.Cells[1, 12].Value = "Radley";
                    worksheet.Cells[1, 13].Value = "Howe Dell";
                    worksheet.Cells[1, 14].Value = "Coach House";
                    worksheet.Cells[1, 15].Value = "Eltisley Manor";
                    worksheet.Cells[1, 16].Value = "Greenwood";
                    worksheet.Cells[1, 17].Value = "Lavender";
                    worksheet.Cells[1, 18].Value = "Winnett";
                    worksheet.Cells[1, 19].Value = "Training";
                    worksheet.Cells[1, 20].Value = "Total Hours Worked";
                    worksheet.Cells[1, 21].Value = "Overtime";
                    worksheet.Cells[1, 22].Value = "Holiday";
                    worksheet.Cells[1, 23].Value = "Sickness";
                    worksheet.Cells[1, 24].Value = "Suspension";
                    worksheet.Cells[1, 25].Value = "Maternity/Paternity";
                    worksheet.Cells[1, 26].Value = "Other";

                    var counter = 2;
                    foreach (var mprModel in dataList)
                    {
                        worksheet.Cells["A" + counter].Value = mprModel.Period;

                        worksheet.Cells["B" + counter].Value = mprModel.BaseLocation;
                        worksheet.Cells["C" + counter].Value = mprModel.BaseSubLocation;
                        worksheet.Cells["D" + counter].Value = mprModel.Firstname;
                        worksheet.Cells["E" + counter].Value = mprModel.Lastname;
                        worksheet.Cells["F" + counter].Value = mprModel.PayrollRef;
                        worksheet.Cells["G" + counter].Value = mprModel.RmsRef;
                        worksheet.Cells["H" + counter].Value = mprModel.Burberry;
                        worksheet.Cells["I" + counter].Value = mprModel.Mulberry;
                        worksheet.Cells["J" + counter].Value = mprModel.OakleyF;
                        worksheet.Cells["K" + counter].Value = mprModel.OakleyM;
                        worksheet.Cells["L" + counter].Value = mprModel.Radley;
                        worksheet.Cells["M" + counter].Value = mprModel.HoweDell;
                        worksheet.Cells["N" + counter].Value = mprModel.CoachHouse;
                        worksheet.Cells["O" + counter].Value = mprModel.EltisleyManor;
                        worksheet.Cells["P" + counter].Value = mprModel.Greenwood;
                        worksheet.Cells["Q" + counter].Value = mprModel.Lavender;
                        worksheet.Cells["R" + counter].Value = mprModel.Winnett;
                        worksheet.Cells["S" + counter].Value = mprModel.Training;
                        worksheet.Cells["T" + counter].Value = mprModel.TotalHours;
                        worksheet.Cells["U" + counter].Value = mprModel.Overtime;
                        worksheet.Cells["V" + counter].Value = mprModel.AnnualLeave;
                        worksheet.Cells["W" + counter].Value = mprModel.SickLeave;
                        worksheet.Cells["X" + counter].Value = mprModel.Suspension;
                        worksheet.Cells["Y" + counter].Value = mprModel.MatPatLeave;
                        worksheet.Cells["Z" + counter].Value = mprModel.Other;

                        counter++;
                    }

                    // Format the values;
                    using (var range = worksheet.Cells[1, 1, 1, 26])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    worksheet.Cells["A1:Z1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:Z1"].Style.Font.Bold = true;

                    // Autofit columns for all cells
                    worksheet.Cells.AutoFitColumns(0);

                    // Layout Orientation
                    worksheet.View.PageLayoutView = true;

                    // Set some document properties
                    package.Workbook.Properties.Title = "Payroll Report";
                    package.Workbook.Properties.Author = "The RMS Report Fairy";

                    // Set some extended property values
                    package.Workbook.Properties.Company = "Nouvita";

                    // Set some custom property values
                    package.Workbook.Properties.SetCustomPropertyValue("Checked by", "The Administrator");
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "RMS");

                    var stream = new MemoryStream(package.GetAsByteArray());
                    var request = new PutObjectRequest
                    {
                        BucketName = "nouvitareports",
                        Key = fileName,
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        InputStream = stream
                    };

                    _awss3Client.PutObject(request);

                    return false;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                }
                else
                {
                    Console.WriteLine(
                     "Error occurred. Message:'{0}' when listing objects",
                     amazonS3Exception.Message);
                }

                return true;
            }
        }

        public static bool GenerateDailyActivityReport(ICollection<DailyActivityReportModel> dataList)
        {
            try
            {
                const string fileName = "Daily_Activity_Report.xlsx";

                using (var package = new ExcelPackage(new FileInfo(@fileName)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Daily Activity Report");

                    worksheet.Cells[1, 1].Value = "Period";
                    worksheet.Cells[1, 2].Value = "Site Location";
                    worksheet.Cells[1, 3].Value = "Sub Site Location";
                    worksheet.Cells[1, 4].Value = "Role";
                    worksheet.Cells[1, 5].Value = "ZKT System ID";
                    worksheet.Cells[1, 6].Value = "Payroll Ref";
                    worksheet.Cells[1, 7].Value = "First Name";
                    worksheet.Cells[1, 8].Value = "Last Name";
                    worksheet.Cells[1, 9].Value = "Type";
                    worksheet.Cells[1, 10].Value = "Date";
                    worksheet.Cells[1, 11].Value = "RMS Shift Start";
                    worksheet.Cells[1, 12].Value = "RMS Shift End";
                    worksheet.Cells[1, 13].Value = "Shift Hours";
                    worksheet.Cells[1, 14].Value = "Unpaid Break";
                    worksheet.Cells[1, 15].Value = "ZKTime Clock-in";
                    worksheet.Cells[1, 16].Value = "ZKTime Clock-out";
                    worksheet.Cells[1, 17].Value = "Actual Start";
                    worksheet.Cells[1, 18].Value = "Actual End";
                    worksheet.Cells[1, 19].Value = "Actual Hours";
                    worksheet.Cells[1, 20].Value = "Actual Status";
                    worksheet.Cells[1, 21].Value = "Approval";
                    worksheet.Cells[1, 22].Value = "Modified";
                    worksheet.Cells[1, 23].Value = "Mod/Reason";
                    worksheet.Cells[1, 24].Value = "Mod/Notes";

                    var counter = 2;
                    foreach (var darModel in dataList)
                    {
                        worksheet.Cells["A" + counter].Value = darModel.Period;
                        worksheet.Cells["B" + counter].Value = darModel.SiteLocation;
                        worksheet.Cells["C" + counter].Value = darModel.SubSiteLocation;
                        worksheet.Cells["D" + counter].Value = darModel.Role;
                        worksheet.Cells["E" + counter].Value = darModel.ZKTimeSystemId;
                        worksheet.Cells["F" + counter].Value = darModel.PayrollReferenceNumber;
                        worksheet.Cells["G" + counter].Value = darModel.Firstname;
                        worksheet.Cells["H" + counter].Value = darModel.Lastname;
                        worksheet.Cells["I" + counter].Value = darModel.Type;
                        worksheet.Cells["J" + counter].Value = darModel.Date;
                        worksheet.Cells["K" + counter].Value = darModel.RmsShiftStart;
                        worksheet.Cells["L" + counter].Value = darModel.RmsShiftEnd;
                        worksheet.Cells["M" + counter].Value = darModel.ShiftHours;
                        worksheet.Cells["N" + counter].Value = darModel.UnpaidBreak;
                        worksheet.Cells["O" + counter].Value = darModel.ZKTimeClockIn;
                        worksheet.Cells["P" + counter].Value = darModel.ZKTimeClockOut;
                        worksheet.Cells["Q" + counter].Value = darModel.ActualStart;
                        worksheet.Cells["R" + counter].Value = darModel.ActualEnd;
                        worksheet.Cells["S" + counter].Value = darModel.ActualHours;
                        worksheet.Cells["T" + counter].Value = darModel.ActualStatus;
                        worksheet.Cells["U" + counter].Value = darModel.Approval;
                        worksheet.Cells["V" + counter].Value = darModel.Modified;
                        worksheet.Cells["W" + counter].Value = darModel.Reason;
                        worksheet.Cells["X" + counter].Value = darModel.Notes;

                        counter++;
                    }

                    // Format the values;
                    using (var range = worksheet.Cells[1, 1, 1, 24])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    worksheet.Cells["A1:X1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:X1"].Style.Font.Bold = true;

                    // Autofit columns for all cells
                    worksheet.Cells.AutoFitColumns(0);

                    // Layout Orientation
                    worksheet.View.PageLayoutView = true;

                    // Set some document properties
                    package.Workbook.Properties.Title = "Daily Activity Report";
                    package.Workbook.Properties.Author = "The RMS Report Fairy";

                    // Set some extended property values
                    package.Workbook.Properties.Company = "Nouvita";

                    // Set some custom property values
                    package.Workbook.Properties.SetCustomPropertyValue("Checked by", "The Administrator");
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "RMS");

                    var stream = new MemoryStream(package.GetAsByteArray());
                    var request = new PutObjectRequest
                    {
                        BucketName = "nouvitareports",
                        Key = fileName,
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        InputStream = stream
                    };

                    var response = _awss3Client.PutObject(request);

                    // return this bool (for now, maybe go off status code) 
                    // for use in the Admin view to hide/show a link
                    return false;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                }
                else
                {
                    Console.WriteLine(
                     "Error occurred. Message:'{0}' when listing objects",
                     amazonS3Exception.Message);
                }

                return true;
            }
        }
    }
}

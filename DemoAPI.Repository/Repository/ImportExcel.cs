using DemoAPI.Data.Entities;
using DemoAPI.Repository.IRepository;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAPI.Repository.Repository
{
    public class ImportExcel : IImportExcel
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImportExcel(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        public async Task<DemoResponse<string>> Import(IFormFile formFile)
        {
            List<User> userList = new List<User>();
            //List data missing
            List<User> userErrorList = new List<User>();

            var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                var columnCount = worksheet.Dimension.Columns;
                for (int row = 2; row <= rowCount; row++)
                {
                    var deviceId = worksheet.Cells[row, 1].Value?.ToString().Trim().Replace("'", "");
                    var deviceFirstName = worksheet.Cells[row, 2].Value?.ToString().Trim().Replace("'", "");
                    var deviceLastName = worksheet.Cells[row, 3].Value?.ToString().Trim().Replace("'", "");
                    var devicePhone = worksheet.Cells[row, 4].Value?.ToString().Trim().Replace("'", "");
                    //var deviceCode = worksheet.Cells[row, 3].Value?.ToString().Trim().Replace("'", "");
                    //var ip = worksheet.Cells[row, 4].Value?.ToString().Trim().Replace("'", "");
                    //var areaCode = worksheet.Cells[row, 5].Value?.ToString().Trim().Replace("'", "");
                    //var deviceType = worksheet.Cells[row, 6].Value?.ToString().Trim().Replace("'", "");
                    //var isActive = worksheet.Cells[row, 7].Value?.ToString().Trim().Replace("'", "");
                    userList.Add(new User
                    {
                        id = deviceId,
                        first_name = deviceFirstName,
                        last_name = deviceLastName,
                        phone = devicePhone,
                    });
                }
            }

            string folder = _hostingEnvironment.WebRootPath;
            string localhost = "https://localhost:5001";
            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string downloadUrl1 = string.Format("{0}://{1}/{2}", localhost, excelName);
            FileInfo file = new FileInfo(Path.Combine(folder, excelName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(folder, excelName));
            }

            // query data from database
            await Task.Yield();

            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(userList, true);
                package.Save();
            }

            return DemoResponse<string>.GetResult(0, "OK", downloadUrl1, userList);
        }

        public Task<FileStreamResult> Export()
        {
            throw new NotImplementedException();
        }
    }

    //public async Task<FileStreamResult> Export()
    //{
    //    // query data from database
    //    await Task.Yield();
    //    var list = new List<User>() {
    //    new User { id = "catcher", first_name = "18" },
    //    new User { id = "james", first_name = "20" },
    //};

    //    var stream = new MemoryStream();

    //    using (var package = new ExcelPackage(stream))
    //    {
    //        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
    //        workSheet.Cells.LoadFromCollection(list, true);
    //        package.Save();
    //    }
    //    stream.Position = 0;
    //    string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

    //    FileStreamResult rslt = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    //    rslt.FileDownloadName = "OutputFile_User.xlsx";

    //    return rslt;

    //}
}
using _6.demoAPI.Common.Security;
using demoAPI.Common.Enum;
using DemoAPI.Data.Entities;
using DemoAPI.Middleware;
using DemoAPI.Models;
using DemoAPI.Repository;
using DemoAPI.Services.UserService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserController(IUserService userService, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("all")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var listUser = this._userService.GetUsers();
            if (listUser == null)
            {
                return NotFound();
            }
            return Ok(listUser);
        }

        //api/user/{id}
        [Route("{id}")]
        [CustomAuthorization(UserRoleEnum.Administrator)]
        [HttpGet]
        public IActionResult FindUserById(string id)
        {
            var user = this._userService.FindUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("addUser")]
        [HttpPost]
        public IActionResult InsertUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(user);
            }
            var result = this._userService.InsertUser(user);

            return Ok(result);
        }

        [Route("deleteUser/{id}")]
        [HttpDelete]
        public IActionResult DeleteUser(string id)
        {
            var result = this._userService.DeleteUser(id);

            return Ok(result);
        }

        [Route("updateUser/{id}")]
        [HttpPut]
        public IActionResult UpdateUser(string id, User user)
        {
            var result = this._userService.UpdateUser(id, user);

            return Ok(result);
        }

        //api/user/{id}
        [Route("import")]
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile formFile)
        {
            var users = new List<User>();
            List<string> error = new List<string>();
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

                    error.Add(" ");
                    if (deviceId == null)
                    {
                        error.Add("Id không được để trống");
                    }

                    if (deviceFirstName == null)
                    {
                        error.Add("First Name không được để trống");
                    }

                    if (deviceLastName == null)
                    {
                        error.Add("Last Name không được để trống");
                    }

                    var message = string.Join(",", error);

                    users.Add(new User
                    {
                        id = deviceId,
                        first_name = deviceFirstName,
                        last_name = deviceLastName,
                        phone = devicePhone,
                        Errors = message == string.Empty ? " " : message
                    });
                }
            }
            DemoResponse<string> fileError = await GetFileError(users);

            return Ok(new ImportResponse
            {
                Result = fileError,
            });
        }

        private async Task<DemoResponse<string>> GetFileError(List<User> users)
        {
            string folder = _hostingEnvironment.WebRootPath;
            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, excelName);
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

                workSheet.Cells.LoadFromCollection(users, true);

                var rowCount = workSheet.Dimension.Rows;
                var columnCount = workSheet.Dimension.Columns;
                for (int row = 2; row <= rowCount; row++)
                {
                    var deviceId = workSheet.Cells[row, 1].Value?.ToString().Trim().Replace("'", "");
                    var deviceFirstName = workSheet.Cells[row, 2].Value?.ToString().Trim().Replace("'", "");
                    var deviceLastName = workSheet.Cells[row, 3].Value?.ToString().Trim().Replace("'", "");

                    if (deviceId == null || deviceFirstName == null || deviceLastName == null)
                    {
                        workSheet.Row(row).Style.Fill.PatternType = ExcelFillStyle.DarkHorizontal;
                        workSheet.Row(row).Style.Fill.BackgroundColor.SetColor(Color.Red);
                    }
                }

                package.Save();
            }

            return DemoResponse<string>.GetResult(0, "OK", downloadUrl, users);
        }
    }
}
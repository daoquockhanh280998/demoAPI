using DemoAPI.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoAPI.Repository.IRepository
{
    public interface IImportExcel
    {
        Task<DemoResponse<string>> Import(IFormFile formFile);
        Task<FileStreamResult> Export();

    }
}

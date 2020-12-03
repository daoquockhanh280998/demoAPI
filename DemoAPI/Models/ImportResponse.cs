using DemoAPI.Data.Entities;
using DemoAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public class ImportResponse
    {
        public DemoResponse<string> Result { get; set; }
        public int Success { get; set; }
        public int Code { get; set; }
        //public int Total { get; set; }
        //public List<User> listUser { get; set; }
    }
}
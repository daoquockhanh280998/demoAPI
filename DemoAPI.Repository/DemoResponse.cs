using DemoAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Repository
{
    public class DemoResponse<T>
    {
        public int Total { get; set; }

        public string Msg { get; set; }

        public T Link { get; set; }
        public List<User> Data { get; set; }

        public static DemoResponse<T> GetResult(int total, string msg, T link, List<User> data)
        {
            return new DemoResponse<T>
            {
                Total = total,
                Msg = msg,
                Link = link,
                Data = data
            };
        }
    }
}
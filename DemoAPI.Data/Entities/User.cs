using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Data.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public string id { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? gender { get; set; }
        public DateTime? birthday { get; set; }
        public bool birthday_year_only { get; set; }
        public string address { get; set; }
        public string ward_id { get; set; }
        public string district_id { get; set; }
        public string province_id { get; set; }
        public string ic_number { get; set; }
        public DateTime? ic_issued_date { get; set; }
        public string ic_type { get; set; }
        public string phone { get; set; }
        public bool is_active { get; set; }
        public string hashvalue { get; set; }
        public string secure_code { get; set; }
        public DateTime? secure_expired { get; set; }
        public bool is_approve { get; set; }
        public string hash_password { get; set; }

        public string image { get; set; }

        public string Errors { get; set; }
        public string Role { get; set; }
    }
}
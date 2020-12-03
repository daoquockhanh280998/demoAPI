using DemoAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoAPI.Services.JwtManager
{
    public interface IJwtManager
    {
        public string GenerateToken(User user);
    }
}
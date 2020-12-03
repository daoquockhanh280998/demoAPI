using demoAPI.ViewModel.User;
using DemoAPI.Data.EF;
using DemoAPI.Data.Entities;
using DemoAPI.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoAPI.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DemoAPIContext _context;
        private readonly IConfiguration _config;

        public UserRepository(DemoAPIContext demoContext, IConfiguration configuration)
        {
            _context = demoContext;
            _config = configuration;
        }

        public bool AddListUser(List<User> users)
        {
            foreach (var item in users)
            {
                var user = InsertUser(item);
            }
            return true;
        }

        public bool DeleteUser(string id)
        {
            var user = FindUserById(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public User FindUserById(string id)
        {
            var userId = _context.Users.Where(x => x.id == id).FirstOrDefault();
            return userId;
        }

        public User FindUserByUserName(LoginRequest login)
        {
            var userId = _context.Users.Where(x => x.UserName == login.UserName && x.Password == login.Password).FirstOrDefault();
            if (userId == null)
            {
                return null;
            }
            return userId;
        }

        public List<User> GetUsers()
        {
            var listUser = _context.Users.Where(x => x.is_active).ToList();
            return listUser;
        }

        public bool InsertUser(User rq)
        {
            var user = FindUserById(rq.id);
            if (user != null)
            {
                return false;
            }

            _context.Add(rq);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateUser(string userId, User rq)
        {
            var user = FindUserById(userId);
            if (user == null)
            {
                return false;
            }

            user.first_name = rq.first_name;
            user.last_name = rq.last_name;
            user.phone = rq.phone;
            user.address = rq.address;
            //user.secure_expired = DateTime.Now;
            //user.ic_issued_date = DateTime.Now;

            _context.SaveChanges();
            return true;
        }
    }
}
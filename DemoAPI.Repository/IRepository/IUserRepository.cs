using demoAPI.ViewModel.User;
using DemoAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        User FindUserById(string id);

        bool InsertUser(User rq);

        bool DeleteUser(string id);

        User FindUserByUserName(LoginRequest login);

        bool UpdateUser(string userId, User rq);

        bool AddListUser(List<User> users);
    }
}
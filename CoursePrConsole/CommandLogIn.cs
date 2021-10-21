using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePrConsole
{
    public class CommandLogIn
    {
        PersonDAL personDal;

        public CommandLogIn(string connStr)
        {
            personDal = new PersonDAL(connStr);
        }

        public bool LogInPerson(string email, string password)
        {
            return personDal.GetAll().Exists(x => x.RoleId == (int)Roles.Admin && x.Email == email && x.Password == password);
        }
    }
}

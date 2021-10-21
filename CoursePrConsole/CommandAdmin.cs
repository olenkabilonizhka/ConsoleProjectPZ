using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePrConsole
{
    class CommandAdmin
    {
        PersonDAL personDal;

        public CommandAdmin(string connStr)
        {
            personDal = new PersonDAL(connStr);
        }

        public void CreateAdmin()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Create new admin ");
                PersonDTO p = new PersonDTO();
                Console.WriteLine("First name: ");
                p.Firstname = Console.ReadLine();
                Console.WriteLine("Last name: ");
                p.Lastname = Console.ReadLine();
                Console.WriteLine("Email: ");
                p.Email = Console.ReadLine();
                Console.WriteLine("Password: ");
                p.Password = Console.ReadLine();
                p.RoleId = (int)Roles.Admin;
                p = personDal.CreatePerson(p);
                Console.WriteLine($"\nAdded!\nPerson id: {p.PersonId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

        public void GetAdmins()
        {
            try
            {
                Console.Clear();
                foreach (var item in personDal.GetAll().FindAll(x => x.RoleId == (int)Roles.Admin))
                {
                    Console.WriteLine($"{item.PersonId} - {item.Firstname} {item.Lastname}, email: {item.Email} - {item.RoleName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

        public void AdminToUser()
        {
            Console.WriteLine("Admin id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            PersonDTO p = personDal.GetAll().Find(x => x.PersonId == id && x.RoleId == (int)Roles.Admin);
            if (p == null)
                throw new Exception("No admin with this Id");
            p.RoleId = (int)Roles.User;
            personDal.UpdatePersonInfo(p,true);
        }

        public void UserToAdmin()
        {
            Console.WriteLine("User id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            PersonDTO p = personDal.GetAll().Find(x => x.PersonId == id && x.RoleId == (int)Roles.User);
            if (p == null)
                throw new Exception("No user with this Id");
            p.RoleId = (int)Roles.Admin;
            personDal.UpdatePersonInfo(p, true);
        }

    }
}

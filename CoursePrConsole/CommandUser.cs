using DAL.Repositories;
using DTO;
using System;

namespace CoursePrConsole
{
    public class CommandUser
    {
        PersonDAL personDal;

        public CommandUser(string connStr)
        {
            personDal = new PersonDAL(connStr);
        }

        public void CreateUser()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Create new user ");
                PersonDTO p = new PersonDTO();
                Console.WriteLine("First name: ");
                p.Firstname = Console.ReadLine();
                Console.WriteLine("Last name: ");
                p.Lastname = Console.ReadLine();
                Console.WriteLine("Email: ");
                p.Email = Console.ReadLine();
                Console.WriteLine("Password: ");
                p.Password = Console.ReadLine();
                p.RoleId = (int)Roles.User;
                p.Status = true;
                p = personDal.CreatePerson(p);
                Console.WriteLine($"\nAdded!\nUser id: {p.PersonId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

        public void GetUsers()
        {
            Console.Clear();
            foreach (var item in personDal.GetAll().FindAll(x => x.RoleId == (int)Roles.User))
            {
                Console.WriteLine($"{item.PersonId} - {item.Firstname} {item.Lastname}, email: {item.Email} - {((bool)item.Status ? "active" : "blocked")}");
            }
            Console.Read();
        }

        public void ChangeStatusUser()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("User id you want to activate/block: ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("1 : activate\n2 : block ");
                bool status;
                switch (Console.ReadLine())
                {
                    case "1":
                        status = true;
                        break;
                    case "2":
                        status = false;
                        break;
                    default:
                        status = false;
                        break;
                }
                var p = personDal.GetAll().Find(x => x.PersonId == id);
                if (p == null)
                    throw new Exception("No user with this Id");
                p.Status = status;
                personDal.UpdateStatus(p);
                Console.WriteLine($"\nSuccessful!\nUser is {((bool)p.Status ? "actived" : "blocked")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

        public void DeleteUser()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("User id you want to delete: ");
                int id = Convert.ToInt32(Console.ReadLine());
                var p = personDal.GetAll().Find(x => x.PersonId == id);
                if (p == null)
                    throw new Exception("No user with this Id");
                personDal.Delete(p);
                Console.WriteLine("\nDeleted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

        public void UpdateUserInfo()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Input user id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                PersonDTO p = personDal.GetAll().Find(x => x.PersonId == id && x.RoleId == (int)Roles.User);
                if (p == null)
                    throw new Exception("No user with this id.");
                Console.WriteLine(@"What you want to update:
1 -> first name
2 -> last name
3 -> email
4 -> password
");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Input first name: ");
                        p.Firstname = Console.ReadLine();
                        break;
                    case "2":
                        Console.WriteLine("Input last name: ");
                        p.Lastname = Console.ReadLine();
                        break;
                    case "3":
                        Console.WriteLine("Input email: ");
                        p.Email = Console.ReadLine();
                        break;
                    case "4":
                        Console.WriteLine("Input password: ");
                        p.Password = Console.ReadLine();
                        break;
                    default:
                        break;
                }
                personDal.UpdatePersonInfo(p);
                Console.WriteLine("\nChanged!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }

    }
}

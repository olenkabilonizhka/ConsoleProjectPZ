using System;
using System.Configuration;

namespace CoursePrConsole
{
    public class Command
    {
        private string connStr;
        CommandLogIn commandLogIn;
        CommandUser commandUser;
        CommandAdmin commandAdmin;

        public Command()
        {
            connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            commandLogIn = new CommandLogIn(connStr);
            commandUser = new CommandUser(connStr);
            commandAdmin = new CommandAdmin(connStr);
        }

        public void Menu()
        {
            string adminChoice = "";
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Enter your email: ");
                    string userEmail = Console.ReadLine();
                    Console.WriteLine("Enter your password: ");
                    string password = Console.ReadLine();
                    switch (commandLogIn.LogInPerson(userEmail, password))
                    {
                        case true:
                            do
                            {
                                Console.Clear();
                                Console.WriteLine(@"
1 -> Get all users              
2 -> Create user                
3 -> Update user info                
4 -> Activate/block user        
5 -> Delete user    
6 -> Get admins
7 -> User to Admin
0 -> Log out
q -> Exit
");
                                adminChoice = Console.ReadLine();
                                switch (adminChoice)
                                {
                                    case "1":
                                        commandUser.GetUsers();
                                        break;
                                    case "2":
                                        commandUser.CreateUser();
                                        break;
                                    case "3":
                                        commandUser.UpdateUserInfo();
                                        break;
                                    case "4":
                                        commandUser.ChangeStatusUser();
                                        break;
                                    case "5":
                                        commandUser.DeleteUser();
                                        break;
                                    case "6":
                                        commandAdmin.GetAdmins();
                                        break;
                                    case "7":
                                        commandAdmin.UserToAdmin();
                                        break;
                                    default:
                                        break;
                                }
                            } while (adminChoice != "0" && adminChoice != "q");
                            break;
                        case false:
                            Console.WriteLine("You are not admin or incorrect inputs\nPress Enter to relogin\n\tq to exit");
                            adminChoice = Console.ReadLine();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }while (adminChoice != "q") ;
        }
    }
}

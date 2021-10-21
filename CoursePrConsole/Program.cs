using CoursePrConsole;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPrConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Command cm = new Command();
            cm.Menu();
        }
    }
}

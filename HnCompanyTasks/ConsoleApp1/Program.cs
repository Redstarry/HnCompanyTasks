using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string date = "0:0:0:0";
            var aa = date.Split(':');
            foreach (var item in aa)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}

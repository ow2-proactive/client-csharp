using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRestClient;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--->2  yeah");

            SchedulerClient sc = new SchedulerClient();
            string sid = sc.connect("http://localhost:8080/rest", "admin","admin");

            Console.WriteLine("---> yeah " + sid);

        }
    }
}

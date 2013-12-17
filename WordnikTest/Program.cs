using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordnik;

namespace WordnikTest {
    class Program {
        private static string myAPIkey = "5be5efbb336502851d00501955301b3b4a63a2fd4af07e5e1";
        static void Main(string[] args) {
            APIClient swag = new APIClient(myAPIkey,"http://api.wordnik.com/v4");

            Console.ReadKey();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordnik;
using Wordnik.Models;

namespace WordnikTest {
    class Program {
        private static string myAPIkey = "5be5efbb336502851d00501955301b3b4a63a2fd4af07e5e1";
        static void Main(string[] args) {
            WordApi test = new WordApi(myAPIkey, "http://api.wordnik.com/v4");
            Task<ExampleSearchResults> output = test.GetExamples("dog");
            output.ContinueWith((t) => {
                List<Example> examples = t.Result.Examples;
                foreach (Example e in examples) {
                    Console.Out.WriteLine("\"{0}\"\n— {1} ({2})\n", e.Text, e.Title, e.Year);
                }
            });
            Console.ReadKey();
        }
    }
}

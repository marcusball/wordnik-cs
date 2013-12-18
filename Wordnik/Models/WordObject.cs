using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wordnik.Models {
    public class WordObject {
        public int ID { get; set; }
        public string Word { get; set; }
        public string OriginalWord { get; set; }
        public List<string> MyProperty { get; set; }
        public string CanonicalForm { get; set; }
        public string Vulgar { get; set; }
    }
}
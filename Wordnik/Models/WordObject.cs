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
        public List<string> Suggestions { get; set; }
        public string CanonicalForm { get; set; }
        public string Vulgar { get; set; }
        public bool Scrabble { get; set; }
        public int LookupCount { get; set; }
    }
}
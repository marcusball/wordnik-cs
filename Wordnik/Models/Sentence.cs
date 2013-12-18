using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class Sentence {
        public bool HasScoredWords { get; set; }
        public int ID { get; set; }
        public List<ScoredWord> ScoredWords { get; set; }
        public string Display { get; set; }
        public int Rating { get; set; }
        public int DocumentMetadataID { get; set; }
    }
}
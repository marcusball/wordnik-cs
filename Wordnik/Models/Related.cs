using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class Related {
        public string Label { get; set; }
        public string RelationshipType { get; set; }
        public string Label2 { get; set; }
        public string Label3 { get; set; }
        public List<string> Words { get; set; }
        public string Gram { get; set; }
        public string Label4 { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class Facet {
        public List<FacetValue> FacetValues { get; set; }
        public string Name { get; set; }
    }
}

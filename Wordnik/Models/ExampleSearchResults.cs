using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class ExampleSearchResults {
        public List<Facet> Facets { get; set; }
        public List<Example> Examples { get; set; }
    }
}

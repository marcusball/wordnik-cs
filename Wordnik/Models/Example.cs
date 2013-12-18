using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class Example {
        public int ID { get; set; }
        public int ExampleID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public ScoredWord Score { get; set; }
        public Sentence Sentence { get; set; }
        public string Word { get; set; }
        public ContentProvider Provider { get; set; }
        public int Year { get; set; }
        public float Rating { get; set; }
        public int DocumentID { get; set; }
        public string Url { get; set; }
    }
}
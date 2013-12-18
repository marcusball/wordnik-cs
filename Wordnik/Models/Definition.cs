using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class Definition {
        public string ExtendedText { get; set; }
        public string Text { get; set; }
        public string SourceDictionary { get; set; }
        public List<Citation> Citations { get; set; }
        public List<Label> Labels { get; set; }
        public float Score { get; set; }
        public List<ExampleUsage> ExampleUses { get; set; }
        public string AttributionUrl { get; set; }
        public string SeqString { get; set; }
        public string AttributionText { get; set; }
        public List<Related> RelatedWords { get; set; }
        public string PartOfSpeech { get; set; }
        public string Word { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordnik.Models {
    public class ScoredWord {
        public int Position { get; set; }
        public int ID { get; set; }
        public int DocTermCount { get; set; }
        public string Lemma { get; set; }
        public string WordType { get; set; }
        public float Score { get; set; }
        public int SentenceID { get; set; }
        public string Word { get; set; }
        public bool Stopword { get; set; }
        public float BaseWordScore { get; set; }
        public string PartOfSpeech { get; set; }
    }
}
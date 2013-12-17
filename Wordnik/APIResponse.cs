using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;

namespace Wordnik {
    public class APIResponse {
        public HttpStatusCode Status { get; set; }
        public Object Deserialized { get; set; }
        public string ResponseText { get; set; }
    }
}

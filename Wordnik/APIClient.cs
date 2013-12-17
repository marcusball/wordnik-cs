using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Wordnik
{
    public class APIClient
    {
        private string _apiKey;
        private string _apiServer;

        /// <summary>
        /// Constructor for the Wordnik API client
        /// </summary>
        /// <param name="apiKey">Your API key</param>
        /// <param name="apiServer">The address of the API server</param>
        public APIClient(string apiKey, string apiServer){
            this._apiKey = apiKey;
            this._apiServer = apiServer;
        }

        /// <summary>
        /// Executes an API call asynchronosly
        /// </summary>
        /// <param name="resourcePath">Path to method endpoint</param>
        /// <param name="method">Method to call</param>
        /// <param name="queryParams">Parameters to be placed in query URL</param>
        /// <param name="postParams">Parameters to be placed in POST body</param>
        /// <param name="headerParams">Parameters to be placed in request header</param>
        public async Task<APIResponse> CallAPI(string resourcePath, string method, IDictionary<string,string> queryParams, IDictionary<string,object> postParams, IDictionary<string,string> headerParams){
            if(resourcePath == null || String.IsNullOrWhiteSpace(resourcePath)){
                throw new ArgumentException("CallAPI resourcePath parameter cannot be null or empty!");
            }

            HttpServerUtility httpUtil = HttpContext.Current.Server;

            StringBuilder urlBuilder = new StringBuilder(this._apiServer);
            urlBuilder.Append(resourcePath);

            if(queryParams != null && queryParams.Count > 0){
                string initialDelimiter = (resourcePath.Contains('?'))?"&":"?"; 
                //If the resourcePath already contains GET parameters (following a '?'), we'll just append to them.
                //Otherwise, we'll need to start the GET parameters with the question mark.

                string getParams = String.Join("&",queryParams.Select((KeyValuePair<string,string> pair) => httpUtil.UrlEncode(pair.Key) + "&" + httpUtil.UrlEncode(pair.Value)));
                urlBuilder.Append(initialDelimiter).Append(getParams);
            }
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString());
            string postData = JsonConvert.SerializeObject(postParams);

            byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postData.Length;
            request.ContentType = "application/json";
            request.Method = "POST";

            using (Stream requestStream = await request.GetRequestStreamAsync()){
                  requestStream.Write(postDataBytes, 0, postDataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync())){
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream())){
                    string responseText = streamReader.ReadToEnd();
                    return new APIResponse(){ Status = response.StatusCode, Deserialized = JsonConvert.DeserializeObject(responseText), ResponseText = responseText };
                }
            }
        }
    }
}

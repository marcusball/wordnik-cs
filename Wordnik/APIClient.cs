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
    public class ApiClient
    {
        private string _apiKey;
        private string _apiServer;

        /// <summary>
        /// Constructor for the Wordnik API client
        /// </summary>
        /// <param name="apiKey">Your API key</param>
        /// <param name="apiServer">The address of the API server</param>
        public ApiClient(string apiKey, string apiServer){
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
        public async Task<ApiResponse> CallAPI(string resourcePath, ApiMethod method, IDictionary<string,string> queryParams, IDictionary<string,object> postParams, IDictionary<string,string> headerParams){
            if(resourcePath == null || String.IsNullOrWhiteSpace(resourcePath)){
                throw new ArgumentException("CallAPI resourcePath parameter cannot be null or empty!");
            }

            StringBuilder urlBuilder = new StringBuilder(this._apiServer);
            urlBuilder.Append(resourcePath);

            if(queryParams != null && queryParams.Count > 0){
                string initialDelimiter = (resourcePath.Contains('?'))?"&":"?"; 
                //If the resourcePath already contains GET parameters (following a '?'), we'll just append to them.
                //Otherwise, we'll need to start the GET parameters with the question mark.

                string getParams = String.Join("&", queryParams.Select((KeyValuePair<string, string> pair) => HttpUtility.UrlEncode(pair.Key) + "&" + HttpUtility.UrlEncode(pair.Value)));
                urlBuilder.Append(initialDelimiter).Append(getParams);
            }
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString()); //Create the request object

            //Add in all of the necessary and requested headers
            request.Headers.Add("api_key", this._apiKey); //We need the api_key header to use the API
            if (headerParams != null && headerParams.Count > 0) { //If there are any other headers specified in the method call, we'll add them in
                var headerEnum = headerParams.GetEnumerator();
                while (headerEnum.MoveNext()) { //Iterate over all of the requested headers
                    request.Headers.Add(headerEnum.Current.Key, headerEnum.Current.Value);
                }
            }

            //Set the HTTP request method (GET, POST, etc).
            request.Method = method.ToString();

            if (method != ApiMethod.GET) {
                //Now we'll configure the data we'll be sending
                //No data is sent for GET requests, so we only do with if this is not a GET request.
                string postData = JsonConvert.SerializeObject(postParams); //JSON encode the specified post parameter
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);

                request.ContentLength = postData.Length;
                
                try{
                    using (Stream requestStream = request.GetRequestStream()) {
                        if (requestStream != null) {
                            requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                        }
                    }
                }
                catch(WebException e){
                    Console.Error.WriteLine("Wordnik ApiClient WebException caught: [{0}] {1}",e.HResult, e.Message);
                }
                catch(ObjectDisposedException e){
                    Console.Error.WriteLine("Wordnik ApiClient ObjectDisposedException caught: [{0}] {1}",e.HResult, e.Message);
                }
            }

            try{
                using (HttpWebResponse response = (HttpWebResponse)(request.GetResponse())) {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream())) {
                        string responseText = streamReader.ReadToEnd();
                        return new ApiResponse() { Status = response.StatusCode, ResponseText = responseText};
                    }
                }
            }
            catch (ProtocolViolationException e) {
                Console.Error.WriteLine("Wordnik ApiClient ProtocolViolationException caught: [{0}] {1}", e.HResult, e.Message);
            }
            catch (NotSupportedException e) {
                Console.Error.WriteLine("Wordnik ApiClient NotSupportedException caught: [{0}] {1}", e.HResult, e.Message);
            }
            catch (WebException e) {
                Console.Error.WriteLine("Wordnik ApiClient WebException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }
        protected string ToPathValue(string value) {
            return HttpUtility.UrlEncode(value);
        }
    }

    /// <summary>
    /// Here's a nice enum class for the different API methods we'll allow
    /// </summary>
    public sealed class ApiMethod{
        private readonly string methodValue;

        public static readonly ApiMethod GET = new ApiMethod("GET");
        public static readonly ApiMethod POST = new ApiMethod("POST");
        public static readonly ApiMethod PUT = new ApiMethod("PUT");
        public static readonly ApiMethod DELETE = new ApiMethod("DELETE");

        private ApiMethod(string mv){
            this.methodValue = mv;
        }
        public override string ToString(){
            return this.methodValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Wordnik.Models;

namespace Wordnik {
    public class WordApi : ApiClient {
        public WordApi(string apiKey, string apiServer)
            : base(apiKey, apiServer) {

        }

        /// <summary>
        /// Returns examples for a word.
        /// </summary>
        /// <param name="word">The word to return examples for.</param>
        /// <returns>ExampleSearchResults in the form of an async Task.</returns>
        public async Task<ExampleSearchResults> GetExamples(string word) {
            return await this.GetExamples(word, false, false, 0, 0);
        }

        /// <summary>
        /// Returns examples for a word.
        /// </summary>
        /// <param name="word">The word to return examples for.</param>
        /// <param name="includeDuplicates">Show duplicate examples from different sources.</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <returns>ExampleSearchResults in the form of an async Task.</returns>
        public async Task<ExampleSearchResults> GetExamples(string word, bool includeDuplicates, bool useCanonical) {
            return await this.GetExamples(word,includeDuplicates,useCanonical,0,0);
        }

        /// <summary>
        /// Returns examples for a word.
        /// </summary>
        /// <param name="word">The word to return examples for.</param>
        /// <param name="includeDuplicates">Show duplicate examples from different sources.</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="skip">Results to skip</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <returns>ExampleSearchResults in the form of an async Task.</returns>
        public async Task<ExampleSearchResults> GetExamples(string word, bool includeDuplicates, bool useCanonical, int skip, int limit) {
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}/examples";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(includeDuplicates){
                queryParams.Add("includeDuplicates","true");
            }
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }
            if(skip > 0){
                queryParams.Add("skip",skip.ToString());
            }
            if(limit > 0){
                queryParams.Add("limit",limit.ToString());
            }

            Console.Out.WriteLine("Hi");
            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<ExampleSearchResults>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }
    }
}


/**
         * getExamples
         * Returns examples for a word
* word, string: Word to return examples for (required)
* includeDuplicates, string: Show duplicate examples from different sources (optional)
* useCanonical, string: If true will try to return the correct word root ('cats' -&gt; 'cat'). If false returns exactly what was requested. (optional)
* skip, int: Results to skip (optional)
* limit, int: Maximum number of results to return (optional)
* @return ExampleSearchResults
        

                  $responseObject = $this->apiClient->deserialize($response,
                   'ExampleSearchResults');
                  return $responseObject; */
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
        /// This overload sets includeDuplicates and useCanonical to False.
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

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<ExampleSearchResults>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }

        /// <summary>
        /// Given a word as a string, returns the WordObject that represents it.
        /// Assumes useCanonical and includeSuggestions to be False.
        /// </summary>
        /// <param name="word">String value of WordObject to return</param>
        /// <returns>WordObject in the form of an async Task</returns>
        public async Task<WordObject> GetWord(string word){
            return await this.GetWord(word,false,false);
        }
        /// <summary>
        /// Given a word as a string, returns the WordObject that represents it
        /// </summary>
        /// <param name="word">String value of WordObject to return</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="includeSuggestions">Return suggestions (for correct spelling, case variants, etc.)</param>
        /// <returns>WordObject in the form of an async Task</returns>
        public async Task<WordObject> GetWord(string word, bool useCanonical, bool includeSuggestions){
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(includeSuggestions){
                queryParams.Add("includeSuggestions","true");
            }
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<WordObject>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }
    }
}
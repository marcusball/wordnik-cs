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

        public WordApi(ApiClient api) : base(api) {
        }

#region GetExamples Overloads
        /// <summary>
        /// Returns examples for a word.
        /// This overload sets includeDuplicates to false and useCanonical to true.
        /// </summary>
        /// <param name="word">The word to return examples for.</param>
        /// <returns>ExampleSearchResults in the form of an async Task.</returns>
        public async Task<ExampleSearchResults> GetExamples(string word) {
            return await this.GetExamples(word, false, true, 0, 0);
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
#endregion 

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

#region GetWord Overloads
        /// <summary>
        /// Given a word as a string, returns the WordObject that represents it.
        /// Assumes useCanonical to be true and includeSuggestions to be False.
        /// </summary>
        /// <param name="word">String value of WordObject to return</param>
        /// <returns>WordObject in the form of an async Task</returns>
        public async Task<WordObject> GetWord(string word){
            return await this.GetWord(word,true,false);
        }
#endregion

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

#region GetDefinitions Overloads
        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary and partOfSpeech as empty string, limit as zero, useCanonical as true, and includeTags as false.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word){
            return await this.GetDefinitions(word,"","",0,true,false);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary and partOfSpeech as empty string, limit as zero, and useCanonical as true.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, bool includeTags){
            return await this.GetDefinitions(word,"","",0,true,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary and partOfSpeech as empty string, and limit as zero.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="useCanonical">Return related words with definitions</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, bool useCanonical, bool includeTags){
            return await this.GetDefinitions(word,"","",0,useCanonical,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary and partOfSpeech as empty string.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <param name="useCanonical">Return related words with definitions</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, int limit, bool useCanonical, bool includeTags){
            return await this.GetDefinitions(word,"","",limit,useCanonical,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary as an empty string, limit as zero, and useCanonical as true.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, bool includeTags){
            return await this.GetDefinitions(word,partOfSpeech,"",0,true,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary as an empty string, and limit as zero. 
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="useCanonical">Return related words with definitions</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, bool useCanonical, bool includeTags){
            return await this.GetDefinitions(word,partOfSpeech,"",0,useCanonical,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload sends sourceDictionary as an empty string.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <param name="useCanonical">Return related words with definitions</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, int limit, bool useCanonical, bool includeTags){
            return await this.GetDefinitions(word,partOfSpeech,"",limit,useCanonical,includeTags);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload will set the limit to zero, useCanonical to true, and includeTags to false.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="sourceDictionaries">Source dictionary to return definitions from. If 'all' is received, results are returned from all sources. If multiple values are received (e.g. 'century,wiktionary'), results are returned from the first specified dictionary that has definitions. If left blank, results are returned from the first dictionary that has definitions. By default, dictionaries are searched in this order: ahd, wiktionary, webster, century, wordnet</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, string sourceDictionaries) {
            return await this.GetDefinitions(word, partOfSpeech, sourceDictionaries, 0, true, false);
        }

        /// <summary>
        /// Return definitions for a word
        /// This overload will set useCanonical to true and includeTags to false.
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="sourceDictionaries">Source dictionary to return definitions from. If 'all' is received, results are returned from all sources. If multiple values are received (e.g. 'century,wiktionary'), results are returned from the first specified dictionary that has definitions. If left blank, results are returned from the first dictionary that has definitions. By default, dictionaries are searched in this order: ahd, wiktionary, webster, century, wordnet</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, string sourceDictionaries, int limit) {
            return await this.GetDefinitions(word, partOfSpeech, sourceDictionaries, limit, true, false);
        }
#endregion
        
        /// <summary>
        /// Return definitions for a word
        /// </summary>
        /// <param name="word">Word to return definitions for</param>
        /// <param name="partOfSpeech">CSV list of part-of-speech types</param>
        /// <param name="sourceDictionaries">Source dictionary to return definitions from. If 'all' is received, results are returned from all sources. If multiple values are received (e.g. 'century,wiktionary'), results are returned from the first specified dictionary that has definitions. If left blank, results are returned from the first dictionary that has definitions. By default, dictionaries are searched in this order: ahd, wiktionary, webster, century, wordnet</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <param name="useCanonical">Return related words with definitions</param>
        /// <param name="includeTags">Return a closed set of XML tags in response</param>
        /// <returns>A List of Definition objects in the form of an async Task</returns>
        public async Task<List<Definition>> GetDefinitions(string word, string partOfSpeech, string sourceDictionaries, int limit, bool useCanonical, bool includeTags){
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}/definitions";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(!String.IsNullOrWhiteSpace(partOfSpeech)){
                queryParams.Add("partOfSpeech",this.ToPathValue(partOfSpeech));
            }
            if(!String.IsNullOrWhiteSpace(sourceDictionaries)){
                queryParams.Add("sourceDictionaries",this.ToPathValue(sourceDictionaries));
            }
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }
            if(includeTags){
                queryParams.Add("includeTags","true");
            }
            if(limit > 0){
                queryParams.Add("limit",limit.ToString());
            }

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<List<Definition>>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }

#region GetTopExample Overloads
        /// <summary>
        /// Returns a top example for a word
        /// This overload will send useCanonical as true.
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <returns>An Example in the form of an async Task</returns>
        public async Task<Example> GetTopExample(string word){
            return await this.GetTopExample(word,true);
        }
#endregion

        /// <summary>
        /// Returns a top example for a word
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <returns>An Example in the form of an async Task</returns>
        public async Task<Example> GetTopExample(string word, bool useCanonical){
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}/topExample";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<Example>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }

#region GetRelatedWords Overloads
        /// <summary>
        /// Given a word as a string, returns relationships from the Word Graph
        /// This overload leaves relationshipTypes empty, and sets useCanonical to true and limitPerRelationshipType to zero.
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <returns>A List of Related words in the the form of an async Task</returns>
        public async Task<List<Related>> GetRelatedWords(string word) {
            return await this.GetRelatedWords(word, "", true, 0);
        }

        /// <summary>
        /// Given a word as a string, returns relationships from the Word Graph
        /// This overload leaves relationshipTypes empty, and sets useCanonical to true.
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <param name="limitPerRelationshipType">Restrict to the supplied relationship types</param>
        /// <returns>A List of Related words in the the form of an async Task</returns>
        public async Task<List<Related>> GetRelatedWords(string word, int limitPerRelationshipType) {
            return await this.GetRelatedWords(word, "", true, limitPerRelationshipType);
        }

        /// <summary>
        /// Given a word as a string, returns relationships from the Word Graph
        /// This overload leaves relationshipTypes empty, and sets limitPerRelationshipType to zero.
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <returns>A List of Related words in the the form of an async Task</returns>
        public async Task<List<Related>> GetRelatedWords(string word, bool useCanonical) {
            return await this.GetRelatedWords(word, "", useCanonical, 0);
        }

        /// <summary>
        /// Given a word as a string, returns relationships from the Word Graph
        /// This overload leaves relationshipTypes empty.
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="limitPerRelationshipType">Restrict to the supplied relationship types</param>
        /// <returns>A List of Related words in the the form of an async Task</returns>
        public async Task<List<Related>> GetRelatedWords(string word, bool useCanonical, int limitPerRelationshipType) {
            return await this.GetRelatedWords(word, "", useCanonical, limitPerRelationshipType);
        }
#endregion

        /// <summary>
        /// Given a word as a string, returns relationships from the Word Graph
        /// </summary>
        /// <param name="word">Word for which to fetch examples</param>
        /// <param name="relationshipTypes">Limits the total results per type of relationship type.</param>
        /// <param name="useCanonical">If true will try to return the correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="limitPerRelationshipType">Restrict to the supplied relationship types</param>
        /// <returns>A List of Related words in the the form of an async Task</returns>
        public async Task<List<Related>> GetRelatedWords(string word, string relationshipTypes, bool useCanonical, int limitPerRelationshipType){
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}/relatedWords";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(!String.IsNullOrWhiteSpace(relationshipTypes)){
                queryParams.Add("relationshipTypes",this.ToPathValue(relationshipTypes));
            }
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }
            if(limitPerRelationshipType > 0){
                queryParams.Add("limitPerRelationshipType",limitPerRelationshipType.ToString());
            }

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<List<Related>>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }

        #region GetTextPronunciation Overloads
        /// <summary>
        /// Returns text pronunciations for a certain word
        /// This overload sends useCanonical as true.
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word){
            return await this.GetTextPronunciations(word,"","",true,0);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// This overload sends useCanonical as true.
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="sourceDictionary">Get from a single dictionary</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, string sourceDictionary){
            return await this.GetTextPronunciations(word,sourceDictionary,"",true,0);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="useCanonical">If true will try to return a correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, bool useCanonical){
            return await this.GetTextPronunciations(word,"","",useCanonical,0);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// This overload sends useCanonical as true.
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, int limit){
            return await this.GetTextPronunciations(word,"","",true,limit);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="useCanonical">If true will try to return a correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, bool useCanonical, int limit){
            return await this.GetTextPronunciations(word,"","",useCanonical,limit);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="sourceDictionary">Get from a single dictionary</param>
        /// <param name="useCanonical">If true will try to return a correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, string sourceDictionary, bool useCanonical){
            return await this.GetTextPronunciations(word,sourceDictionary,"",useCanonical,0);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// This overload sends useCanonical as true.
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="sourceDictionary">Get from a single dictionary</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, string sourceDictionary, int limit){
            return await this.GetTextPronunciations(word,sourceDictionary,"",true,limit);
        }

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="sourceDictionary">Get from a single dictionary</param>
        /// <param name="useCanonical">If true will try to return a correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, string sourceDictionary, bool useCanonical, int limit){
            return await this.GetTextPronunciations(word,sourceDictionary,"",useCanonical,limit);
        }
        #endregion

        /// <summary>
        /// Returns text pronunciations for a certain word
        /// </summary>
        /// <param name="word">Word for which to fetch pronunciations</param>
        /// <param name="sourceDictionary">Get from a single dictionary</param>
        /// <param name="typeFormat">Text pronunciation type</param>
        /// <param name="useCanonical">If true will try to return a correct word root ('cats' -> 'cat'). If false returns exactly what was requested.</param>
        /// <param name="limit">Maximum number of results to return</param>
        /// <returns>Returns a List of TextPron objects in the form of an async Task</returns>
        public async Task<List<TextPron>> GetTextPronunciations(string word, string sourceDictionary, string typeFormat, bool useCanonical, int limit){
            if(word == null || String.IsNullOrWhiteSpace(word)){
                throw new ArgumentException("Word argument cannot be null or empty.");
            }
            string resourcePath = "/word.{0}/{1}/pronunciations";
            resourcePath = String.Format(resourcePath,"json",this.ToPathValue(word));

            Dictionary<string,string> queryParams = new Dictionary<string,string>();
            if(!String.IsNullOrWhiteSpace(typeFormat)){
                queryParams.Add("typeFormat",this.ToPathValue(typeFormat));
            }
            if(!String.IsNullOrWhiteSpace(sourceDictionary)){
                queryParams.Add("sourceDictionary",this.ToPathValue(sourceDictionary));
            }
            if(useCanonical){
                queryParams.Add("useCanonical","true");
            }
            if(limit > 0){
                queryParams.Add("limit",limit.ToString());
            }

            ApiResponse response = await this.CallAPI(resourcePath,ApiMethod.GET,queryParams,null,null);
            try {
                return JsonConvert.DeserializeObject<List<TextPron>>(response.ResponseText);
            }
            catch (JsonException e) {
                Console.Error.WriteLine("Wordnik WordApi JsonException caught: [{0}] {1}", e.HResult, e.Message);
            }
            return null;
        }
    }
}


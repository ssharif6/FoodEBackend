using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using FoodE.Models;

namespace FoodE.Drivers
{
    public class ImageTaggingDriver
    {
        private static readonly string _baseUrl = "https://api.clarifai.com/v1/token";
        private static readonly string _clientId = "ZgwgAqryLIM1zSc4impeZ0uF-7CupdcYKMNDW6ln";
        private static readonly string _clientSecret = "YYZIn0iePOUKh9862NOuqULOmPR11PjUDkmS-PZf";
        private static readonly string _tagBaseUrl = "https://api.clarifai.com/v1/tag/";
        ImgurDriver _driver = new ImgurDriver();

        [HttpPost]
        private async Task<string> GetUserToken()
        {
            using (var client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials" },
                    {"client_id", _clientId },
                    {"client_secret", _clientSecret }
                };
                var content = new FormUrlEncodedContent(parameters);

                var response = await client.PostAsync(_baseUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                UserTokenModel userToken = JsonConvert.DeserializeObject<UserTokenModel>(responseString);
                Console.WriteLine(userToken.AccessToken);
                return userToken.AccessToken;
            }
        }

        public async Task<List<TagModel>> GetTags(string imgurUrl)
        {
            var userToken = await GetUserToken();
            Console.WriteLine();
            var tagType = "food-items-v1.0";
            using (var client = new HttpClient())
            {
                var url = String.Format("{0}?model={1}&url={2}", _tagBaseUrl, tagType, imgurUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetUserToken());
                var response = await client.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseString);
                var ingredientsObj = jObject["results"][0]["result"]["tag"]["classes"];
                var probabilityObj = jObject["results"][0]["result"]["tag"]["probs"];
                var docIdObj = jObject["results"][0]["result"]["tag"]["concept_ids"];
                // How to get the array of values
                var ingredientList = JsonConvert.DeserializeObject<List<string>>(ingredientsObj.ToString());
                var probabilityList = JsonConvert.DeserializeObject<List<string>>(probabilityObj.ToString());
                var docIdList = JsonConvert.DeserializeObject<List<string>>(docIdObj.ToString());

                var taggingList = new List<TagModel>();

                var count = 0;
                foreach(var ingredient in ingredientList)
                {
                    taggingList.Add(new TagModel
                    {
                        IngredientName = ingredient,
                        Confidence = Convert.ToDouble(probabilityList[count]),
                        Doc_Id = docIdList[count]
                    });
                    count++;
                }

                var confidentList = taggingList.Where(x => x.Confidence >= 0.80);

                return confidentList.ToList();
            }
        }

        private string UrlDecode(string value)
        {
            return System.Web.HttpUtility.UrlDecode(value);
        }

        private string UrlEncode(string value)
        {
            return System.Web.HttpUtility.UrlEncode(value);
        }


    }

}
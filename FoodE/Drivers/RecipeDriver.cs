using FoodE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FoodE.Drivers
{
    public class RecipeDriver
    {
        private const string _baseUrl = "http://api.yummly.com/v1";
        // example
        //http://api.yummly.com/v1/api/recipes?_app_id=3027d943&_app_key=1aba4f55d648ec990d72f18647ab621e&q=cumin,pepper

        public async Task<List<int>> GetRecipeIds(List<string> ingredients)
        {
            var idLIst = new List<int>();
            if(ingredients == null || ingredients.Count == 0)
            {
                return new List<int>();
            }
            var requestString = generateRequestString(ingredients);
            var url = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByIngredients?fillIngredients=false&ingredients=" + UrlEncode(requestString) + "&limitLicense=false&number=5&ranking=1";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Mashape-Key", "ZmKGv5QEgOmshwUoGWvT61p4QPFVp1CNlHDjsn7RBSB0oYkRWs");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await httpClient.GetAsync(url);

            string content = await response.Content.ReadAsStringAsync();

            var recipeArr = JArray.Parse(content);

            foreach(var obj in recipeArr)
            {
                idLIst.Add(Int32.Parse(obj["id"].ToString()));
            }

            return idLIst;
        }

        public async Task<List<RecipeResponseModel>> GetRecipesFromId(List<int> list)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Mashape-Key", "ZmKGv5QEgOmshwUoGWvT61p4QPFVp1CNlHDjsn7RBSB0oYkRWs");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var res = new List<RecipeResponseModel>();
            foreach(var id in list)
            {
                var url = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/" + id + "/information?includeNutrition=true";
                var response = await httpClient.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                var contentObject = JObject.Parse(content);
                var ingredientslist = new List<IngredientModel>();
                var ingredientsArray = JArray.Parse(contentObject["extendedIngredients"].ToString()); // array
                foreach(var ingredient in ingredientsArray)
                {
                    ingredientslist.Add(new IngredientModel
                    {
                        Amount = ingredient["amount"] == null ? -1.0 : Convert.ToDouble(ingredient["amount"]),
                        Id = ingredient["id"] == null ? -1 : Convert.ToInt32(ingredient["id"]),
                        ImageUrl = ingredient["image"] == null ? "None" : ingredient["image"].ToString(),
                        Name = ingredient["name"] == null ? "None" : ingredient["name"].ToString(),
                        OriginalString = ingredient["originalString"] == null ? "None" : ingredient["originalString"].ToString()
                    });
                }
                var recipeTitle = contentObject["title"];
                var prepTime = contentObject["readyInMinutes"];
                var image = contentObject["image"];
                var recipeSourceUrl = contentObject["sourceUrl"].ToString();
                res.Add(new RecipeResponseModel
                {
                    Id = id.ToString(),
                    ImageUrl = image.ToString(),
                    IngredientsList = ingredientslist,
                    RecipeName = recipeTitle.ToString(),
                    RecipeTotalTime = Convert.ToInt32(prepTime),
                    RecipeUrl = recipeSourceUrl
                });

            }
            return res;
        }

        
        public async Task<List<RecipeResponseModel>> GetRecipes(List<string> ingredients)
        {
            if(ingredients == null || ingredients.Count == 0)
            {
                return null;
            }
            var requestString = generateRequestString(ingredients);
            
            var url = "http://api.yummly.com/v1/api/recipes?_app_id=3027d943&_app_key=1aba4f55d648ec990d72f18647ab621e&q=" + requestString.ToString();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            string content = await response.Content.ReadAsStringAsync();
            //var jsonRecipeObject = JsonConvert.DeserializeObject(content);
            var recipeResponseList = new List<RecipeResponseModel>();

            var jObject = JObject.Parse(content);
            var matchesList = jObject["matches"];

            foreach(var recipe in matchesList)
            {
                recipeResponseList.Add(new RecipeResponseModel
                {
                    Id = recipe["id"].ToString(),
                    ImageUrl = recipe["imageUrlsBySize"] == null ? "None" : recipe["imageUrlsBySize"]["90"].ToString() ?? "None",
                    Ingredients = recipe["ingredients"].ToObject<List<string>>(),
                    RecipeName = recipe["recipeName"].ToString(),
                    RecipeTotalTime = recipe["totalTimeInSeconds"].ToObject<int>()
                });
            }


            return recipeResponseList;
        }

        private string generateRequestString(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            var requestString = new StringBuilder();
            requestString.Append(list[0]);
            for (var i = 1; i < list.Count; i++)
            {
                requestString.Append(",");
                requestString.Append(list[i]);
            }
            return requestString.ToString();
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
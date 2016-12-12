using FoodE.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace FoodE.Drivers
{
    public class NutritionixDriver
    {
        // Take in List of TagModel
        // Loop through each model in the list and send request to Nutritionix
        private static string NUTRITIONIX_URL = "https://api.nutritionix.com/v1_1/search/";
        private static string PARAM_STRINGS = "?fields=item_name%2Citem_id%2Cbrand_name%2Cnf_calories%2Cnf_total_fat&appId=305d2c2b&appKey=15b1ac2310d3f1b4707829a7ad9f7741";

        public async Task<List<NutritionixIngredientModel>> GetNutritionalInformation(List<TagModel> list)
        {
            var tagList = new List<NutritionixIngredientModel>();
            foreach(var tag in list)
            {
                var url = NUTRITIONIX_URL + tag.IngredientName + PARAM_STRINGS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    // Get only the first object and it to a list
                    var tagObj = JObject.Parse(responseString);
                    var firstItem = tagObj["hits"][0]["fields"];
                    tagList.Add(new NutritionixIngredientModel
                    {
                        Id = firstItem["item_id"].ToString(),
                        Calories = firstItem["nf_calories"].ToString(),
                        OriginalIngredientName = tag.IngredientName,
                        Total_Fat = firstItem["nf_total_fat"].ToString()
                    });
                }
                   
            }

            return tagList;
        }
    }
}
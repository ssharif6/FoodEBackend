using FoodE.Drivers;
using FoodE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FoodE.Managers
{
    public class FoodProcessingManager
    {
        public static ImgurDriver _imgurDriver = new ImgurDriver();
        public static ImageTaggingDriver _taggingDriver = new ImageTaggingDriver();
        public static NutritionixDriver _nutritionixDriver = new NutritionixDriver();
        public static RecipeDriver _recipeDriver = new RecipeDriver();

        public async Task<List<NutritionixIngredientModel>> GenerateNutritionalTags(string base64string)
        {
            var imageUrl = await _imgurDriver.GenerateUrl(base64string);
            var tagList = await _taggingDriver.GetTags(imageUrl);
            // Hit nutritionix api, this is the data that should be returned.
            var nutritionalList = await _nutritionixDriver.GetNutritionalInformation(tagList);
            return nutritionalList;
        } 

        public async Task<List<RecipeResponseModel>> GenerateRecipesFromIngredients(List<string> list)
        {
            var recipeId = await _recipeDriver.GetRecipeIds(list);
            var recipeModels = await _recipeDriver.GetRecipesFromId(recipeId);

            return recipeModels;
            //var recipeModels = await _yummlyDriver.GetRecipes(list);
            //return recipeModels;
        }

    }
}
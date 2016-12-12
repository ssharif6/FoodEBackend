using FoodE.Drivers;
using FoodE.Managers;
using FoodE.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FoodE.Controllers
{
    public class ImageProcessingController : ApiController
    {

        ImageTaggingDriver _taggingDriver = new ImageTaggingDriver();
        ImgurDriver _imgurDriver = new ImgurDriver();
        FoodProcessingManager _manager = new FoodProcessingManager();

        [HttpPost]
        [Route("api/imageProcessing")]
        public async Task<List<NutritionixIngredientModel>> testBase64(ImgurRequest model)
        {
            var ingredientsList = await _manager.GenerateNutritionalTags(model.Base64EncodedImage);
            return ingredientsList;
        }

        [HttpPost]
        [Route("api/imageProcessing/findByIngredients")]
        public async Task<List<RecipeResponseModel>> findRecipesByIngredient(RecipeRequestModel model)
        {
            var recipes = await _manager.GenerateRecipesFromIngredients(model.IngredientList);
            return recipes;
        }
    }
}

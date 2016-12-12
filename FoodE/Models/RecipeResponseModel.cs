using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodE.Models
{
    public class RecipeResponseModel
    {
        public string RecipeUrl { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Ingredients { get; set; }
        public string Id { get; set; }
        public string RecipeName { get; set; }
        public int RecipeTotalTime { get; set; }
        public List<IngredientModel> IngredientsList { get; set; }
    }
}
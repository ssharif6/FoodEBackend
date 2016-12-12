using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodE.Models
{
    public class ResultModel
    {
        public string ImgurUrl { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
    }
}
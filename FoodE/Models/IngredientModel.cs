using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodE.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string OriginalString { get; set; }
    }
}
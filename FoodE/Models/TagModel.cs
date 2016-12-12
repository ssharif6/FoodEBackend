using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodE.Models
{
    public class TagModel
    {
        /// <summary>
        /// Ingredient Name from Clarafai Tagging api
        /// </summary>
        public string IngredientName { get; set; }
        /// <summary>
        /// Returns the confidence probability out of 1.0
        /// </summary>
        public double Confidence { get; set; }
        /// <summary>
        /// Clarafai's Tagging Api
        /// </summary>
        public string Doc_Id { get; set; }
    }
}
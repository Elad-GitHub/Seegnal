using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Seegnal.Models
{
    public class IngredientData
    {
        //Name of active ingredient
        public string term { get; set; }
        //Number of reported cases which the ingredient has been found active 
        public int count { get; set; }
        [JsonIgnore]
        public int Precentage { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Seegnal.Models
{
    public class DrugData
    {
        [JsonIgnore]
        public string meta { get; set;}
        public IngredientData[] results { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Seegnal.Models
{
    public class DrugData
    {
        [JsonIgnore]
        public string meta { get; set;}
        public IngredientData[] results { get; set; }
    }
}

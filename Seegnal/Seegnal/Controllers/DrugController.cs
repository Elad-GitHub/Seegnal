using Microsoft.AspNetCore.Mvc;
using Seegnal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace Seegnal.Controllers
{
    public class DrugController : ApiController
    {
        public async Task<List<IngredientData>> GetMainIngredients(string reaction)
        {
            var result = await GetExternalResponse(reaction);

            return result;
        }

        private async Task<List<IngredientData>> GetExternalResponse(string reaction)
        {
            var client = new HttpClient();
            
            HttpResponseMessage response = await client.GetAsync($"https://api.fda.gov/drug/event.json?search=patient.reaction.reactionmeddrapt.exact:%22{reaction}%22&count=patient.drug.medicinalproduct.exact");
            
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsStringAsync();

            DrugData drugsData = JsonSerializer.Deserialize<DrugData>(result);

            //var ingredientList = drugsData.results.ToList();

            var orderByQuery = from i in drugsData.results
                               orderby i.count
                               select i;

            var sortedIngredientList = new List<IngredientData>();

            Array.ForEach<IngredientData>(orderByQuery.ToArray<IngredientData>(), p => sortedIngredientList.Add(p));

            return drugsData.results.ToList();
        }
    }
}

using Seegnal.Models;
using System;
using System.Collections.Generic;
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

            DrugData drugsData = JsonSerializer.Deserialize<DrugData>(result);

            List<IngredientData> mostProminentIngredients = GetMostProminentIngredients(drugsData);

            CalculateRelativePrecentage(mostProminentIngredients);

            return mostProminentIngredients;
        }

        private async Task<string> GetExternalResponse(string reaction)
        {
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"https://api.fda.gov/drug/event.json?search=patient.reaction.reactionmeddrapt.exact:%22{reaction}%22&count=patient.drug.medicinalproduct.exact");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private List<IngredientData> GetMostProminentIngredients(DrugData drugsData)
        {
            var orderByQuery = from i in drugsData.results
                               orderby i.count
                               select i;

            var sortedIngredientList = new List<IngredientData>();

            Array.ForEach<IngredientData>(orderByQuery.ToArray<IngredientData>(), p => sortedIngredientList.Add(p));

            var mostProminentIngredients = sortedIngredientList.Skip(Math.Max(0, sortedIngredientList.Count() - 10)).ToList();

            return mostProminentIngredients;
        }

        private static void CalculateRelativePrecentage(List<IngredientData> mostProminentIngredients)
        {
            double totalNumOfReaction = 0;

            foreach (IngredientData ing in mostProminentIngredients)
            {
                totalNumOfReaction += ing.count;
            }

            foreach (IngredientData ing in mostProminentIngredients)
            {
                ing.precentage = (double)(ing.count / (totalNumOfReaction * 100));
            }
        }
    }
}

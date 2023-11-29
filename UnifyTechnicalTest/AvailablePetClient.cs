using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnifyTechnicalTest
{
    public interface IAvailablePetClient
    {
        public Task ListPetsSortedByCategoryAndNameAsync(CancellationToken stoppingToken);
    }
    public class Pet
    {
        public string? Name { get; set; }
        public Category? Category { get; set; }
    }
    public class Category
    {
        public string? Name { get; set; }
    }
        

    public class AvailablePetClient : IAvailablePetClient
    {
        private readonly HttpClient _client;
        public AvailablePetClient(HttpClient client)
        {
            _client = client;
        }

        public async Task ListPetsSortedByCategoryAndNameAsync(CancellationToken stoppingToken)
        {
            var pets = await LoadPetsAsync(stoppingToken);
            var sortedPets = SortPets(pets);
            ListPets(sortedPets);
        }
        public async Task<ICollection<Pet>> LoadPetsAsync(CancellationToken stoppingToken)
        {
            //todo URL should be loaded from config
            var pets = await _client.GetFromJsonAsync<ICollection<Pet>>("https://petstore.swagger.io/v2/pet/findByStatus?status=available", stoppingToken);
            if (pets == null)
            {
                throw new FormatException("Error accessing API");
            }
            return pets;
        }

        public IOrderedEnumerable<Pet> SortPets(ICollection<Pet> pets)
        {
            return pets.OrderBy(p => p.Category?.Name).ThenByDescending(p => p.Name);
        }
        public void ListPets(IEnumerable<Pet> pets)
        {
            string? categoryName = null;
            bool first = true;
            foreach (var pet in pets)
            {
                if (categoryName != pet.Category?.Name || first)
                {
                    if (!first)
                    {
                        Console.WriteLine();
                    }
                    categoryName = pet.Category?.Name;
                    Console.WriteLine(categoryName == null ? "No Category" : categoryName);
                    Console.WriteLine("----------");
                }
                Console.WriteLine(pet.Name);
                first = false;
            }
        }
    }
}

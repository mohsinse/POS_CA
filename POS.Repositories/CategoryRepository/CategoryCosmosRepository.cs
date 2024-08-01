using POS.Model;
using POS.Model.Enum;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repositories.CategoryRepository
{
    public class CategoryCosmosRepository
    {
        private readonly Container _container;

        public CategoryCosmosRepository(CosmosClient cosmosClient)
        {
            var database = cosmosClient.GetDatabase("POSDB");
            _container = database.GetContainer("Categories");
        }

        public async Task<bool> AddCategory(Category category, POS.Model.User user)
        {
            if (user.UserRole == UserRole.Admin)
            {
                //category.Id = Guid.NewGuid().ToString(); // Ensure each category has a unique ID
                try
                {
                    await _container.CreateItemAsync(category, new PartitionKey(category.Id));
                    return true;
                }
                catch (CosmosException ex)
                {
                    // Handle exception (logging, rethrowing, etc.)
                    Console.WriteLine($"Error adding category: {ex.Message}");
                }
            }
            return false;
        }

        // Add other methods for Category as needed
    }
}

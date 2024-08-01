using POS.Model;
using POS.Model.Enum;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repositories.ProductRepository
{
    public class ProductCosmosRepository : IProductRepository
    {
        private readonly Container _container;

        public ProductCosmosRepository(CosmosClient cosmosClient)
        {
            var database = cosmosClient.GetDatabase("POSDB");
            _container = database.GetContainer("Products");
        }

        public async Task<bool> AddProduct(Product product, POS.Model.User user)
        {
            if (user.UserRole == UserRole.Admin)
            {
                product.id = Guid.NewGuid().ToString();
                await _container.CreateItemAsync(product, new PartitionKey(product.id));
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProduct(Product product, POS.Model.User user)
        {
            if (user.UserRole == UserRole.Admin)
            {
                await _container.ReplaceItemAsync(product, product.id, new PartitionKey(product.id));
                return true;
            }
            return false;
        }

        public async Task RemoveProduct(string productName, POS.Model.User  user)
        {
            if (user.UserRole == UserRole.Admin)
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @Name")
                    .WithParameter("@Name", productName);

                var resultSetIterator = _container.GetItemQueryIterator<Product>(query);

                while (resultSetIterator.HasMoreResults)
                {
                    var response = await resultSetIterator.ReadNextAsync();
                    var product = response.FirstOrDefault();
                    if (product != null)
                    {
                        await _container.DeleteItemAsync<Product>(product.id, new PartitionKey(product.id));
                        break;
                    }
                }
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var resultSetIterator = _container.GetItemQueryIterator<Product>(query);
            var products = new List<Product>();

            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();
                products.AddRange(response);
            }

            return products;
        }
    }
}

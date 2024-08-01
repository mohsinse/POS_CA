using POS.Model;
using POS.Model.Enum;
using Microsoft.Azure.Cosmos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repositories.UserRepository
{
    public class UserCosmosRepository : IUserRepository
    {
        private readonly Container _container;

        public UserCosmosRepository(CosmosClient cosmosClient)
        {
            var database = cosmosClient.GetDatabase("POSDB");
            _container = database.GetContainer("Users");
        }

        public async Task RegisterUser(POS.Model.User user)
        {
            try
            {
                user.id = Guid.NewGuid().ToString();
                await _container.CreateItemAsync(user, new PartitionKey(user.id));
            }
            catch(CosmosException ex) {
                throw new ArgumentException(ex.Message);

            }
        }

        public POS.Model.User AuthenticateUser(string email, string password)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @Email AND c.Password = @Password")
                .WithParameter("@Email", email)
                .WithParameter("@Password", password);

            var resultSetIterator = _container.GetItemQueryIterator<POS.Model.User>(query);

            while (resultSetIterator.HasMoreResults)
            {
                var response = resultSetIterator.ReadNextAsync().GetAwaiter().GetResult();
                var user = response.FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }

        public IEnumerable<POS.Model.User> GetAllUsers()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var resultSetIterator = _container.GetItemQueryIterator<POS.Model.User>(query);
            var users = new List<POS.Model.User>();

            while (resultSetIterator.HasMoreResults)
            {
                var response = resultSetIterator.ReadNextAsync().GetAwaiter().GetResult();
                users.AddRange(response);
            }

            return users;
        }

        public async Task SetUserRole(POS.Model.User user, UserRole role)
        {
            user.UserRole = role;
            await _container.ReplaceItemAsync(user, user.id, new PartitionKey(user.id));
        }
    }
}

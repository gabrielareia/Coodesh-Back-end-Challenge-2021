using CoodeshPharmaIncAPI.Database;
using CoodeshPharmaIncAPI.JsonConvertion.ContractResolver;
using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.ImportData
{
    public class ImportData
    {
        private const int MAX_USERS = 2000;
        private const int USERS_PER_CALL = 100;
        private readonly string API_URL = $"https://randomuser.me/api?results={USERS_PER_CALL}";

        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new IgnoreUserPropertiesContractResolver()
        };

        public async Task<int> Populate(IServiceScopeFactory scopeFactory)
        {
            int count = 0;

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                PharmaContext ctx = scope.ServiceProvider.GetRequiredService<PharmaContext>();

                await DeleteAllUsers(ctx);
                
                int iterations = MAX_USERS / USERS_PER_CALL;

                for (int i = 0; i < iterations; i++)
                {
                    User[] clients = await GetUsersFromApi(scope);

                    await ctx.User.AddRangeAsync(clients);

                    count += clients.Length;
                }

                ctx.SaveChanges();
            }

            return count;
        }

        #region Methods

        private async Task<User[]> GetUsersFromApi(IServiceScope scope)
        {
            HttpClient client = scope.ServiceProvider.GetRequiredService<HttpClient>();

            HttpResponseMessage response = await client.GetAsync(API_URL);

            string json = await response.Content.ReadAsStringAsync();

            RootImport root = JsonConvert.DeserializeObject<RootImport>(json, jsonSettings);

            User[] clients = await ParseUsers(client, root);

            return clients;
        }

        private static async Task<User[]> ParseUsers(HttpClient client, RootImport root)
        {
            User[] clients = new User[root.Length];

            for (int i = 0; i < root.Length; i++)
            {
                //Get picture bytes from API picture address
                if (!string.IsNullOrEmpty((string)root[i].Picture))
                {
                    HttpResponseMessage pictureResp = await client.GetAsync((string)root[i].Picture);

                    root[i].Picture = await pictureResp.Content.ReadAsByteArrayAsync();
                }

                //Convert from "import models" to "base models"
                clients[i] = root[i].ToBaseUser();
            }

            return clients;
        }

        private async Task DeleteAllUsers(PharmaContext ctx)
        {
            User[] users = await ctx.User
               .Include(t => t.Name)
               .Include(t => t.Contact)
               .Include(t => t.Location)
               .ThenInclude(t => t.TimeZone)
               .Include(t => t.Login)
               .Include(t => t.Picture)
               .ToArrayAsync();


            if (users == null)
            {
                return;
            }

            ctx.Name.RemoveRange(users.Select(t => t.Name));
            ctx.Login.RemoveRange(users.Select(t => t.Login));
            ctx.Contact.RemoveRange(users.Select(t => t.Contact));
            ctx.TimeZone.RemoveRange(users.Select(t => t.Location.TimeZone));
            ctx.Location.RemoveRange(users.Select(t => t.Location));
            ctx.Picture.RemoveRange(users.Select(t => t.Picture));
            ctx.User.RemoveRange(users);
        }

        #endregion
    }
}

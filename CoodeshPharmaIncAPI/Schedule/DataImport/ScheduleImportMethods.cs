using CoodeshPharmaIncAPI.Database;
using CoodeshPharmaIncAPI.JsonConvertion.ContractResolver;
using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Schedule
{
    public partial class ScheduleImport
    {
        private const string API_URL = "https://randomuser.me/api/?seed=foobar&results=100";

        public async Task<int> Populate()
        {
            HttpResponseMessage response = await _client.GetAsync(API_URL);

            string json = await response.Content.ReadAsStringAsync();


            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new IgnoreUserPropertiesContractResolver()
            };

            RootImport root = JsonConvert.DeserializeObject<RootImport>(json, jsonSettings);

            User[] clients = new User[root.Length];

            //Get picture bytes from API picture address
            for (int i = 0; i < root.Length; i++)
            {
                if (!string.IsNullOrEmpty((string)root[i].Picture))
                {
                    HttpResponseMessage pictureResp = await _client.GetAsync((string)root[i].Picture);

                    root[i].Picture = await pictureResp.Content.ReadAsByteArrayAsync();
                }

                clients[i] = root[i].ToBaseUser();
            }

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                PharmaContext ctx = scope.ServiceProvider.GetRequiredService<PharmaContext>();

                await DeleteAllUsers(ctx);

                await ctx.User.AddRangeAsync(clients);
                ctx.SaveChanges();
            }


            return clients.Length;

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

            //ctx.SaveChanges();

        }
    }
}

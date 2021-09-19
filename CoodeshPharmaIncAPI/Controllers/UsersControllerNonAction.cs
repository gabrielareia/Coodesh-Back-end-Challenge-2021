using CoodeshPharmaIncAPI.Database;
using CoodeshPharmaIncAPI.JsonConvertion.ContractResolver;
using CoodeshPharmaIncAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Controllers
{
    public partial class UsersController
    {
        private readonly PharmaContext _ctx;

        public UsersController(PharmaContext ctx)
        {
            _ctx = ctx;
        }

        //Methods that are not actions but are used for communicating with the database

        #region GET users/

        [NonAction]
        private async Task<User[]> SelectAllUsers()
        {
            User[] users = await QueryAllUsers();

            if (users == null)
            {
                return null;
            }

            UpdateUsersPictureAddress(users);

            return users;
        }

        [NonAction]
        private static void UpdateUsersPictureAddress(User[] users)
        {
            for (int i = 0; i < users.Length; i++)
            {
                ChangePictureAddress(users[i]);
            }
        }

        [NonAction]
        private async Task<User[]> QueryAllUsers()
        {
            return await FillUser(_ctx.User)
                .ToArrayAsync();
        }

        #endregion

        #region GET users/userId
        [NonAction]
        private User SelectUser(int id)
        {
            User user = QueryOneUser(id);

            if (user == null)
            {
                return null;
            }

            ChangePictureAddress(user);

            return user;
        }

        [NonAction]
        private User QueryOneUser(int id)
        {
            return FillUser(_ctx.User.Where(u => u.Id == id))
                .AsNoTracking() //So it won't interfere with the context.Update method
                .FirstOrDefault();
        }

        #endregion

        #region PUT users/userId
        private bool UpdateUserById(int userId, User user)
        {
            User oldUser = QueryOneUser(userId);

            if (oldUser == null)
            {
                return false;
            }

            //Make sure that the related objects will be updated accordingly.
            user.Contact.Id = oldUser.Contact.Id;
            user.Name.Id = oldUser.Name.Id;
            user.Location.Id = oldUser.Location.Id;
            user.Location.TimeZone.Id = oldUser.Location.TimeZone.Id;
            user.Picture.Id = oldUser.Picture.Id;
            user.Login.Id = oldUser.Login.Id;
            user.Imported_T = oldUser.Imported_T;

            user.Id = userId;

            _ctx.User.UpdateRange(user);
            _ctx.SaveChanges();

            return true;
        }
        #endregion

        #region DELETE users/userId
        private bool RemoveUser(int userId)
        {
            User user = QueryOneUser(userId);

            if (user == null)
            {
                return false;
            }

            _ctx.Name.Remove(user.Name);
            _ctx.Login.Remove(user.Login);
            _ctx.Contact.Remove(user.Contact);
            _ctx.Location.Remove(user.Location);
            _ctx.TimeZone.Remove(user.Location.TimeZone);
            _ctx.Picture.Remove(user.Picture);
            _ctx.User.Remove(user);

            _ctx.SaveChanges();

            return true;
        }
        #endregion

        #region Shared
        [NonAction]
        private static string SerializeToJson<T>(T user)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new PictureAddressContractResolver()
            };

            string json = JsonConvert.SerializeObject(user, serializerSettings);
            return json;
        }

        [NonAction]
        private static void ChangePictureAddress(User user)
        {
            user.Picture.SetDefaultAddress($"/api/users/{user.Id}/picture");
        }

        [NonAction]
        public IIncludableQueryable<User, Picture> FillUser(IQueryable<User> user)
        {
            return user
               .Include(t => t.Name)
               .Include(t => t.Contact)
               .Include(t => t.Location)
               .ThenInclude(t => t.TimeZone)
               .Include(t => t.Login)
               .Include(t => t.Picture);
        }
        #endregion



    }
}

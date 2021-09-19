using CoodeshPharmaIncAPI.Models;
using CoodeshPharmaIncAPI.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Controllers
{
    [ApiController]
    [Route("api/users/")]
    [Produces("application/json")]
    public partial class UsersController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            User[] users = await SelectAllUsers();

            if (users == null || users.Length == 0)
            {
                return NotFound($"Users not found. There are no users in the database.");
            }

            string json = SerializeToJson(users);

            return Ok(json);
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetUser(int userId)
        {
            User user = SelectUser(userId);

            if (user == null)
            {
                return NotFound($"User not found. There is no match for id: {userId}");
            }

            string json = SerializeToJson(user);

            return Ok(json);
        }


        [HttpPut]
        [Route("{userId}")]
        [ApiKey]
        public IActionResult PutUser(int userId, [FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!UpdateUserById(userId, user))
                    {
                        return NotFound($"User not found. There is no match for id: {userId}");
                    }

                    return Ok($"The user {userId} was updated.");
                }

                return BadRequest("Couldn't resolve user.");
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(DbUpdateException))
                {
                    return BadRequest("Something wen't wrong, user could not be updated. Check if the properties meet all the conditions of the database, like size and types for example.");
                }
                return StatusCode(500, "Something wen't wrong and the user could not be updated, please try again.");
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        [ApiKey]
        public IActionResult DeleteUser(int userId)
        {
            if (!RemoveUser(userId))
            {
                return NotFound($"User not found. There is no match for id: {userId}");
            }

            return NoContent();
        }

    }
}

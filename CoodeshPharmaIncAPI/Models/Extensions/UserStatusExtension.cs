using System.Collections.Generic;
using System.Linq;

namespace CoodeshPharmaIncAPI.Models.Extensions
{
    public static class UserStatusExtension
    {
        private static Dictionary<UserStatus, string> map = new Dictionary<UserStatus, string>()
        {
            {UserStatus.Draft, "draft" },
            {UserStatus.Trash, "trash" },
            {UserStatus.Published, "published" }
        };

        public static string GetName(this UserStatus status)
        {
            return map[status];
        }
        public static UserStatus GetStatus(this string status)
        {
            return map.First(c => c.Value == status).Key;
        }
    }
}

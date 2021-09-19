using System;

namespace CoodeshPharmaIncAPI.Models
{
    public struct UserDate
    {
        public DateTime Date { get; set; }
        public int Age => GetAge();


        /// <summary>
        /// Converts the Date into an age
        /// </summary>
        /// <returns>The difference between today and the Date property</returns>
        private int GetAge()
        {
            DateTime t = DateTime.Today;

            //Converting from DateTime to an integer with the format "yyyyMMdd"
            //It is the same as int.parse(Date.ToString("yyyyMMdd"), but faster.
            int today = (t.Year * 100 + t.Month) * 100 + t.Day; ;
            int date = (Date.Year * 100 + Date.Month) * 100 + Date.Day;

            int result = today - date;

            int age = (int)result / 10000;

            return age;
        }

        public static implicit operator int(UserDate d) => d.Age;
        public static implicit operator DateTime(UserDate d) => d.Date;

        public static implicit operator UserDate(DateTime d) => new UserDate() { Date = d };
    }
}

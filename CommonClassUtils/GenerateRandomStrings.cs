using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassUtils
{
    public class GenerateRandomStrings
    {
        public static string GenerateEmail(int numberOfLetters, int numberOfDigits)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm";
            const string digits = "0123456789";
            var random = new Random();
            var letters = new string(
                Enumerable.Repeat(chars, numberOfLetters)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            var numbers = new string(
                Enumerable.Repeat(digits, numberOfDigits)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());

            var myNewEmail = letters + numbers + "@" + letters + ".com";
            return myNewEmail;
        }

        public static string GetDateMinusNumberOfYears(int numberOfYears)
        {
            if (numberOfYears != 0)
            {
                DateTime thisDay = DateTime.Today;
                DateTime todayXYearsAgo = thisDay.AddYears(-numberOfYears);
                string dateWeWant = todayXYearsAgo.ToShortDateString();
                return dateWeWant;
            }
            else
            {
                string dateWeWant = DateTime.Today.ToString();
                return dateWeWant;
            }
        }


        public static string GenerateRandomZeroEightMobileNumber(int numberOfDigitsAfterZeroEight)
        {
            //Random random = null;
            const string digits = "0123456789";
            var random = new Random();

            var numbers = new string(
                Enumerable.Repeat(digits, numberOfDigitsAfterZeroEight)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());

            return "08" + numbers;
        }

        public static string GenerateRandomNumber(int i)
        {
            const string digits = "0123456789";
            var random = new Random();

            var numbers = new string(
                Enumerable.Repeat(digits, i)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());

            return numbers;
        }

        public static object GetTodaysDate()
        {
            DateTime thisDay = DateTime.Today;

            string dateWeWant = thisDay.ToShortDateString();
            return dateWeWant;
        }
    }
}

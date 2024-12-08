using System.Net.Mail;
using System.Text.RegularExpressions;

namespace JobCandidate.Service.Helpers
{
    public static class EmailHelper
    {
        private static readonly Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return emailRegex.IsMatch(email);
        }
    }
}

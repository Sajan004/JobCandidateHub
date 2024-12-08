using System.Net.Mail;

namespace JobCandidateHub.Helpers
{
    public class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }

            catch
            {
                return false;
            }
        }
    }
}

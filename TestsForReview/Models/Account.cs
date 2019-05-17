using TestsForReview.Utils;

namespace TestsForReview.Models
{
    public class Account
    {
        public string UserName;
        public string Password;
        public string FirstName;
        public string LastName;

        public bool IsAdmin;

        public string DisplayName => $"{FirstName} {LastName}";

        public static Account GetDefault()
        {
            return Config.DefaultAccount;
        }
    }
}

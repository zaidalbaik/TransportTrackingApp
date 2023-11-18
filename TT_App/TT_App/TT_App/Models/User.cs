namespace TT_App.Models
{
    public class User
    {
        public int userID { get; set; }

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string password { get; set; }

        public string confirmPassword { get; set; }
    }
}

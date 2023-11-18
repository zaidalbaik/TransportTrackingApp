namespace TT_App.Models
{
    public class Driver
    {
        public int Id { get; set; }

        public string DriverName { get; set; }
   
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        public int BusId { get; set; }
 }
}

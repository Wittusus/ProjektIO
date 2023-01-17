using Microsoft.AspNetCore.Identity;

namespace ProjektIO.Entities
{
    public class Salaries
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public decimal Salary { get; set; }  
    }
}

using System.ComponentModel.DataAnnotations;

namespace Authenticate.Models.SignUp
{
    public class UserViewModel
    {
       
            [Key]
            public int userid { get; set; }
            public string? username { get; set; }
            public string? Email { get; set; }
            public string? password { get; set; }
        
    }
}

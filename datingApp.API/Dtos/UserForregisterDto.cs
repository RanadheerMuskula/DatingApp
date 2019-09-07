using System.ComponentModel.DataAnnotations;

namespace datingApp.API.Dtos
{
    public class UserForregisterDto
    {
      [Required]
      public string Username { get; set; }  
      [Required]
      [StringLength(8,MinimumLength=4,ErrorMessage="Pleale enter the passcharacters between 8 and 4")]
      public string  Password { get; set; }
    }
}
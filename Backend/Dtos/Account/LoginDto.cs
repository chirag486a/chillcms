using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Account;
public class LoginDto
{

  [EmailAddress(ErrorMessage = "Invalid Email Address")]
  public string Email { get; set; } = string.Empty;

  [Required]
  public string Password { get; set; } = string.Empty;
}
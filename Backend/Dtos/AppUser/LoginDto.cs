using System.ComponentModel.DataAnnotations;
using Backend.Dtos.AppUser;

public class LoginDto
{
  public string UserName { get; set; } = string.Empty;

  [EmailAddress(ErrorMessage = "Invalid Email Address")]
  public string Email { get; set; } = string.Empty;

  [Required]
  public string Password { get; set; } = string.Empty;
}
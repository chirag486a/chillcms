
using Backend.Models.Users;

namespace Backend.Models.Contents;


public class ContentMeta
{
  public int Id { get; set; }
  public string UserId { get; set; } = Guid.NewGuid().ToString();
  public User? User { get; set; }
  public string? ContentTitle { get; set; }
  public string? ContentSlug { get; set; }
  public string? ContentDescription { get; set; }
  public bool ContentDeleted { get; set; }
  public DateTime CreatedAt { get; set; }

}
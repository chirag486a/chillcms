
using Backend.Models.Users;

namespace Backend.Models.Contents;


public class ContentMeta
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string UserId { get; set; } = string.Empty;
  public User? User { get; set; }
  public string? ContentTitle { get; set; }
  public string? ContentSlug { get; set; }
  public string? ContentDescription { get; set; }
  public bool ContentDeleted { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
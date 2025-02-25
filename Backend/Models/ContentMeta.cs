
namespace Backend.Models;

public class ContentMeta
{
  public int Id { get; set; }
  public string AppUserId { get; set; }
  public AppUser AppUser { get; set; }
  public string ContentTitle { get; set; }
  public string ContentSlug { get; set; }
  public string ContentDescription { get; set; }
  public bool ContentDeleted { get; set; }
  public DateTime CreatedAt { get; set; }

}

namespace Backend.Models;

public class Content
{
  public int Id { get; set; }
  public int AppUserId { get; set; }
  public string ContentTitle { get; set; }
  public string ContentSlug { get; set; }
  public string ContentDescription { get; set; }
  public bool ContentDeleted { get; set; }
  public DateTime CreatedAt { get; set; }

}
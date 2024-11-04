using System.ComponentModel.DataAnnotations;

public class Activities
{
    public int ActivitySystemId { get; set; }
    [Required]
    public string ActivityName { get; set; }
}
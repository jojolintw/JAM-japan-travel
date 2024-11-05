using System.ComponentModel.DataAnnotations;

public class SearchForm
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public int ActivityId { get; set; } = 0;
    public string SortBy { get; set; } = "popular";
}
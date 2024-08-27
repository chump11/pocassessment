namespace POCAssessment.Domain;

public class Product()
{
    public string Title { get; set; } = "";
    public int Price { get; set; } = 0;
    public string[] Sizes { get; set; } = [];
    public string Description { get; set; } = "";

    /// <summary>
    /// Handles the domain logic of enriching the description with highlights
    /// </summary>
    /// <param name="highlights"></param>
    /// <returns>the enriched string</returns>
    public string EnrichedDescription(string? highlights)
    {
        if (string.IsNullOrWhiteSpace(highlights)) return Description;
        string enrichedDescription = Description;
        foreach (var highlight in highlights.Split(","))
        {
            enrichedDescription = enrichedDescription.Replace(highlight.Trim(), $"<em>{highlight}</em>");
        }
        return enrichedDescription;
    }

}

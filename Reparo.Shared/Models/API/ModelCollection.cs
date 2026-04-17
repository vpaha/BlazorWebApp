public class ModelCollection<T>
{
    //[SwaggerSchema("Identifies the total number of all items that are returned.")]
    public long? TotalCount { get; set; }

    public long? Take { get; set; }

    public long? Skip { get; set; }

    public required IEnumerable<T> Results { get; set; }

}
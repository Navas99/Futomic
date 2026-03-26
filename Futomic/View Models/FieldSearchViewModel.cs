namespace Futomic.View_Models
{
    public class FieldSearchViewModel
    {
        public string? SearchQuery { get; set; }
        public List<FieldResult> Results { get; set; } = new();
    }

    public class FieldResult
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? PlusCode { get; set; }
    }
}

namespace TreeService.DTOs
{
    public class TreeNodeFlatDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }
}

namespace TreeService.DTOs
{
    public class UpdateTreeNodeDto
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }
}

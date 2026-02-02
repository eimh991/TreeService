namespace TreeService.DTOs
{
    public class CreateTreeNodeDto
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }
}

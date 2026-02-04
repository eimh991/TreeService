namespace TreeService.DTOs
{
    public class TreeNodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public List<TreeNodeDto> Children { get; set; } =  new List<TreeNodeDto>();
    }
}

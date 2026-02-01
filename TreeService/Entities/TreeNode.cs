namespace TreeService.Entities
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    

        public int? ParentId { get; set; }

        public TreeNode? Parent { get; set; }

        public ICollection<TreeNode> Children { get; set; } = new List<TreeNode>(); 
    }
}

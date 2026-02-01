using TreeService.Entities;

namespace TreeService.Services
{
    public interface ITreeNodeService
    {
        Task<List<TreeNode>> GetAllAsync(CancellationToken cancellationToken);
        Task<TreeNode?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<TreeNode> CreateAsync(TreeNode node, CancellationToken cancellationToken);
        Task UpdateAsync(int id, TreeNode node, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task<List<TreeNode>> GetTreesNodesAsync(CancellationToken cancellationToken);
    }
}

using TreeService.Entities;

namespace TreeService.Repositories
{
    public interface ITreeNodeRepository
    {
        Task<TreeNode> GetByIdAsync(int id, CancellationToken cancellationToken );
        Task<List<TreeNode>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(TreeNode node, CancellationToken cancellationToken);
        Task DeleteAsync(TreeNode node, CancellationToken cancellationToken);
        Task UpdateAsync(TreeNode node, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    }
}

using TreeService.DTOs;
using TreeService.Entities;

namespace TreeService.Services
{
    public interface ITreeNodeService
    {
        Task<List<TreeNode>> GetAllAsync(CancellationToken cancellationToken);
        Task<TreeNode?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<TreeNode> CreateAsync(CreateTreeNodeDto dto, CancellationToken cancellationToken);
        Task UpdateAsync(int id, UpdateTreeNodeDto dto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task<List<TreeNode>> GetTreesNodesAsync(CancellationToken cancellationToken);
    }
}

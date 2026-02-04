using TreeService.DTOs;
using TreeService.Entities;

namespace TreeService.Repositories
{
    public interface ITreeNodeRepository
    {
        Task<TreeNodeDto> GetByIdAsync(int id, CancellationToken cancellationToken );
        Task<List<TreeNodeDto>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(TreeNode node, CancellationToken cancellationToken);
        Task DeleteAsync(TreeNode node, CancellationToken cancellationToken);
        Task UpdateAsync(TreeNode node, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

        Task<List<TreeNodeFlatDto>> GetAllToExportAsync(CancellationToken cancellationToken);
    }
}

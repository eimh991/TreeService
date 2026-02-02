using Microsoft.EntityFrameworkCore;
using TreeService.Data;
using TreeService.Entities;

namespace TreeService.Repositories
{
    public class TreeNodeRepository : ITreeNodeRepository
    {
        private readonly AppDbContext _dbContext;
        public TreeNodeRepository(AppDbContext dbContext) {
            _dbContext = dbContext; 
        }
        public async Task AddAsync(TreeNode node, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(node, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TreeNode node, CancellationToken cancellationToken)
        {
            _dbContext.TreeNodes.Remove(node);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<TreeNode>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.TreeNodes
                            .Include(n => n.Children)
                            .ToListAsync(cancellationToken);
        }

        public async Task<TreeNode> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.TreeNodes
                   .Include(n => n.Children)
                   .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(TreeNode node, CancellationToken cancellationToken)
        {
            _dbContext.TreeNodes.Update(node);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.TreeNodes
                .AnyAsync(n=>n.Id == id, cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TreeService.Data;
using TreeService.DTOs;
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

        public async Task<List<TreeNodeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var allNodes = await _dbContext.TreeNodes
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            
            return BuildTree(allNodes, null);
        }

        public async Task<TreeNodeDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var allNodes = await _dbContext.TreeNodes
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);

            var node = allNodes.FirstOrDefault(n => n.Id == id);
            if (node == null) return null;

            return BuildSubtree(allNodes, node.Id);
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

        private List<TreeNodeDto> BuildTree(List<TreeNode> nodes, int? parentId)
        {
            return nodes
                .Where(n => n.ParentId == parentId)
                .Select(n => new TreeNodeDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    ParentId = n.ParentId,
                    Children = BuildTree(nodes, n.Id) 
                })
                .ToList();
        }

        private TreeNodeDto BuildSubtree(List<TreeNode> allNodes, int nodeId)
        {
            var node = allNodes.First(n => n.Id == nodeId);

            var dto = new TreeNodeDto
            {
                Id = node.Id,
                Name = node.Name,
                ParentId = node.ParentId
            };

            var children = allNodes.Where(n => n.ParentId == nodeId).ToList();
            if (children.Any())
            {
                dto.Children = children.Select(c => BuildSubtree(allNodes, c.Id)).ToList();
            }

            return dto;
        }

        public async Task<List<TreeNodeFlatDto>> GetAllToExportAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.TreeNodes
                .AsNoTracking()
                .Select(n=> new TreeNodeFlatDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    ParentId = n.ParentId,
                })
                .ToListAsync(cancellationToken);
        }
    }
}

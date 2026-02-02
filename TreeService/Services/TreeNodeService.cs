using TreeService.Data;
using TreeService.DTOs;
using TreeService.Entities;
using TreeService.Repositories;

namespace TreeService.Services
{
    public class TreeNodeService : ITreeNodeService
    {
        private ITreeNodeRepository _treeNodeRepository;
        private AppDbContext _db;

        public TreeNodeService(ITreeNodeRepository treeNoderepository, AppDbContext db)
        {
            _treeNodeRepository = treeNoderepository;
            _db = db;
        }

        public async Task<TreeNode> CreateAsync(CreateTreeNodeDto dto, CancellationToken cancellationToken)
        {
            if (dto.ParentId.HasValue)
            {
                var parent =  await _treeNodeRepository.ExistsAsync(dto.ParentId.Value, cancellationToken);

                if (parent)
                {
                    throw new Exception("Родительский узел не найден");
                }
            }
            var node = new TreeNode
            {
                Name = dto.Name,
                ParentId = dto.ParentId
            };

            await _treeNodeRepository.AddAsync(node, cancellationToken);
            return node;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var correctNode = await _treeNodeRepository.GetByIdAsync(id,cancellationToken);
            if(correctNode == null)
            {
                throw new Exception("Узел не найден");
            }

            if(correctNode.Children != null)
            {
                throw new Exception("Нельзя удалить узел, укоторого есть связь с другими узлами");
            }

            await _treeNodeRepository.DeleteAsync(correctNode, cancellationToken);
        }

        public async Task<List<TreeNode>> GetAllAsync(CancellationToken cancellationToken)
        {
            return  await _treeNodeRepository.GetAllAsync(cancellationToken);
        }

        public async Task<TreeNode?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _treeNodeRepository.GetByIdAsync(id, cancellationToken);    
        }

        public async Task UpdateAsync(int id, UpdateTreeNodeDto dto, CancellationToken cancellationToken)
        {
            var correctNode = await _treeNodeRepository.GetByIdAsync(id, cancellationToken);
            if (correctNode == null)
            {
                throw new Exception("Узел не найден");
            }

            if (dto.ParentId.HasValue)
            {
                if (await ChekCycle(id, dto.ParentId, cancellationToken))
                {
                    throw new Exception("Невозможно установить родителя, это приведет к зацикленности");
                }
            }

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

            correctNode.Name = dto.Name;
            correctNode.Parent = dtoe.Parent;

            await _treeNodeRepository.UpdateAsync(correctNode, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

        }

        public async Task<List<TreeNode>> GetTreesNodesAsync(CancellationToken cancellationToken)
        {
            var allNodes = await _treeNodeRepository.GetAllAsync(cancellationToken);

            var rootNodes = allNodes
                .Where(n=>n.ParentId == null)
                .ToList();

            foreach(var root in rootNodes)
            {
                PopulateChildren(root, allNodes);
            }

            return rootNodes;
        }

        private async Task<bool> ChekCycle(int nodeId, int? newParentId, CancellationToken cancellationToken)
        {
            if (!newParentId.HasValue)
            {
                return false;
            }

            int? currentId = newParentId;

            while(currentId != null)
            {
                if(currentId == nodeId)
                {
                    return true;
                }

                var parent = await _treeNodeRepository.GetByIdAsync(currentId.Value, cancellationToken);

                currentId = parent?.ParentId;
            }

            return false;
        }

        private void PopulateChildren(TreeNode node, List<TreeNode> allNodes)
        {
            node.Children = allNodes
                .Where(n=>n.ParentId == node.Id)
                .ToList();

            foreach (var children in node.Children)
            {
                PopulateChildren(children, allNodes);
            }
        }
    }
}

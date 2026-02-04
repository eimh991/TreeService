using System.Xml.Linq;
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

                if (!parent)
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
            var correctNodeDto = await _treeNodeRepository.GetByIdAsync(id,cancellationToken);
            if(correctNodeDto == null)
            {
                throw new Exception("Узел не найден");
            }

            if(correctNodeDto.Children.Count >= 1)
            {
                throw new Exception("Нельзя удалить узел, у которого есть связь с другими узлами");
            }
            
            var node = ConvertTreeNodeDto(correctNodeDto);

            await _treeNodeRepository.DeleteAsync(node, cancellationToken);
        }

        public async Task<List<TreeNodeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return  await _treeNodeRepository.GetAllAsync(cancellationToken);
        }

        public async Task<TreeNodeDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _treeNodeRepository.GetByIdAsync(id, cancellationToken);    
        }

        public async Task UpdateAsync(int id, UpdateTreeNodeDto dto, CancellationToken cancellationToken)
        {
            var correctNodeDto = await _treeNodeRepository.GetByIdAsync(id, cancellationToken);
            if (correctNodeDto == null)
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

            correctNodeDto.Name = dto.Name;
            correctNodeDto.ParentId = dto.ParentId;

            var node = ConvertTreeNodeDto(correctNodeDto);

            await _treeNodeRepository.UpdateAsync(node, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

        }

        public async Task<List<TreeNodeFlatDto>> GetTreesNodesAsync(CancellationToken cancellationToken)
        {
            var allNodes = await _treeNodeRepository.GetAllToExportAsync(cancellationToken);

        
            return allNodes;
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

        private TreeNode ConvertTreeNodeDto(TreeNodeDto nodeDto)
        {
           var node = new TreeNode
           {
               Id = nodeDto.Id,
               ParentId = nodeDto.ParentId,
               Name = nodeDto.Name,
               Children = new List<TreeNode>()
           };

           foreach (var childDto in nodeDto.Children)
           {
               var childEntity = ConvertTreeNodeDto(childDto);
               childEntity.Parent = node; 
               node.Children.Add(childEntity);
           }

           return node;
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreeService.Auth;
using TreeService.DTOs;
using TreeService.Entities;
using TreeService.Services;

namespace TreeService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TreeNodeController: ControllerBase
    {
        private readonly ITreeNodeService _treeNodeService;

        public TreeNodeController(ITreeNodeService treeNodeService)
        {
            _treeNodeService = treeNodeService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var allNode = await _treeNodeService.GetAllAsync(cancellationToken);
            return Ok(allNode); 
        }

        [HttpGet("id")]

        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var node = await _treeNodeService.GetByIdAsync(id, cancellationToken);

            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTreeNodeDto dto, CancellationToken cancellationToken)
        {
            var createdNode = await _treeNodeService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdNode.Id }, createdNode);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTreeNodeDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _treeNodeService.UpdateAsync(id, dto, cancellationToken);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _treeNodeService.DeleteAsync(id, cancellationToken);
                return NoContent();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportTree(CancellationToken cancellationToken)
        {
            var tree  = await _treeNodeService.GetTreesNodesAsync(cancellationToken);
            return Ok(tree);
        }
    }
}

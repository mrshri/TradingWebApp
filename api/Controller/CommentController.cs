using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [ApiController]
    [Route("api/Comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

       [HttpGet]
       public async Task<IActionResult> GetAllComments()
       {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
           }
             var comments = await _commentRepository.GetAllCommentsAsync();
             var commentDto = comments.Select(c => c.ToCommentDto()).ToList();

                if (comments == null || comments.Count == 0)
                {
                    return NotFound("No comments found.");
                }
                return Ok(comments);
       }

         [HttpGet("{id:int}")]
         public async Task<IActionResult> GetCommentById([FromRoute] int id)
         {
             if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
           }

            var comment = await _commentRepository.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound($"Comment with id {id} not found.");
                }
                return Ok(comment.ToCommentDto());

        }

        [HttpPost("{StockId:int}")]
        [Authorize]        
        public async Task<IActionResult> CreateComment([FromRoute] int StockId, [FromBody] CreateCommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepository.IsStockExists(StockId))
            {
                return BadRequest($"Stock does not exist with id {StockId}");
            }

            var commentModel = comment.ToCommentFromCreate(StockId);
            await _commentRepository.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]  
        [Authorize]         
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.UpdateCommentAsync(id, commentModel.ToCommentFromUpdate());
            if (comment == null)
            {
                return NotFound($"Comment with id {id} not found.");
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete("{id:int}")]
        [Authorize]        
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.DeleteCommentAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with id {id} not found.");
            }
            return Ok(comment.ToCommentDto());
        }
        
    }
}
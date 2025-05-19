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
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, IFMPService fmpService)
        {
            _fmpService = fmpService;
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

        [HttpPost("{symbol:Alpha}")]
        [Authorize]        
        public async Task<IActionResult> CreateComment([FromRoute] string symbol, [FromBody] CreateCommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fmpService.FindBySymbolAsync(symbol);
                if (stock == null)
                {
                    return NotFound($"Stock with symbol {symbol} not found.");
                }
                var stockModel = await _stockRepository.CreateStockAsync(stock);
            }

            var commentModel = comment.ToCommentFromCreate(stock.Id);
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
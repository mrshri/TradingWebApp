using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel){
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedAt = commentModel.CreatedAt,
                StockId = commentModel.StockId
            };
        }

        
        public static Comment ToCommentFromCreate(this CreateCommentDto CommentDto, int stockId){
            return new Comment
            {
                
                Title = CommentDto.Title,
                Content = CommentDto.Content,
                StockId = stockId
            };
        }

          public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto CommentDto){
            return new Comment
            {
                
                Title = CommentDto.Title,
                Content = CommentDto.Content,
            };
        }
    }
}
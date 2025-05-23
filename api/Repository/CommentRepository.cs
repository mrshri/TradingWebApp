using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
           var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return null;
            }
            _context.Comments.Remove(comment);
            await  _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateCommentAsync(int id,Comment commentModel)
        {
            var existingComment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
                if (existingComment == null)
                {
                    return null;
                }
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();
            return commentModel;
        }

       
    }
}
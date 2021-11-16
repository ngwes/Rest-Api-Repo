using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public interface ICommentService
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetCommentsAsync(UserFilter filter, PaginationFilter paginationFilter = null);
        Task<bool> CreateCommentAsync(Comment comment);
        Task<bool> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Guid id);
        Task<bool> UserOwnsComment(Guid id, string userId);
        Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId);
    }
}

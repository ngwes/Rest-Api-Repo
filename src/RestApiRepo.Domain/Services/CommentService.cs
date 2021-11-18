using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Entitites;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            _commentRepository.InsertComment(comment);
            return await _commentRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment is null)
                return false;
            _commentRepository.DeleteComment(comment);
            return await _commentRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetAllCommentsAsync(x => x.Id.Equals(id), null, "User,Post");
            return comment.FirstOrDefault();
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(UserFilter userFilter, PaginationFilter paginationFilter = null)
        {
            var skip = paginationFilter.PageNumber <= 1 ? 0 : paginationFilter.PageNumber * paginationFilter.PageSize;

            IEnumerable<Comment> posts;
            if (!string.IsNullOrEmpty(userFilter?.UserId))
                posts = await _commentRepository.GetAllCommentsAsync(x => x.UserId.Equals(userFilter.UserId), null, "User,Post", skip, paginationFilter.PageSize);
            else
            {
                posts = await _commentRepository.GetAllCommentsAsync(null, null, "User,Post", skip, paginationFilter.PageSize);
            }
            return posts;
        }

        public async Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId, UserFilter userFilter, PaginationFilter paginationFilter = null)
        {
            var skip = paginationFilter.PageNumber <= 1 ? 0 : paginationFilter.PageNumber * paginationFilter.PageSize;

            IEnumerable<Comment> posts;
            if (!string.IsNullOrEmpty(userFilter?.UserId))
                posts = await _commentRepository.GetAllCommentsAsync(filter: x => x.UserId.Equals(userFilter.UserId) && x.PostId.Equals(postId),includeProperties: "User,Post", skip: skip, take: paginationFilter.PageSize);
            else
            {
                posts = await _commentRepository.GetAllCommentsAsync(filter:x=>x.PostId.Equals(postId), includeProperties: "User,Post", skip: skip, take: paginationFilter.PageSize);
            }
            return posts;
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            _commentRepository.UpdateComment(comment);
            return await _commentRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> UserOwnsComment(Guid id, string userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment is null)
                return false;
            return comment.UserId.Equals(userId);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Entitites
{
    public class Comment
    {
        public Guid Id{ get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public IdentityUser User{ get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreateAt { get; set; }
    }
}

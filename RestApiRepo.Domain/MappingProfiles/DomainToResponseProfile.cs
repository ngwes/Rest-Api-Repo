using AutoMapper;
using Rest_Api_Repo.Domain.Responses.V1;
using Rest_Api_Repo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(destinationMember => destinationMember.Tags,
                opt=>opt.MapFrom(src=>
                src.PostTags
                    .Select(postTag=> new ResponseTag { Name = postTag.Tag.TagName})))
                .ForMember(destinationMember => destinationMember.UserId,
                opt => opt.MapFrom(src => src.UserId));
            
            CreateMap<Tag, TagsResponse>();
        }
    }
}

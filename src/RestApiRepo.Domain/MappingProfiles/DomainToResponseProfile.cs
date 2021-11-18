using AutoMapper;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.MappingProfiles
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

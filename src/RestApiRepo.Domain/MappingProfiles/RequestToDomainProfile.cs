using AutoMapper;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Requests.V1;
using RestApiRepo.Domain.Requests.V1.Posts;

namespace RestApiRepo.Domain.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsQuery, UserFilter>();
        }
    }
}

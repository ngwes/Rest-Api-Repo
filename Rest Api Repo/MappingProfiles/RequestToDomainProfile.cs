using AutoMapper;
using Rest_Api_Repo.Domain;
using RestApi_Contracts.V1.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsQuery, GetAllPostFilter>();
        }
    }
}

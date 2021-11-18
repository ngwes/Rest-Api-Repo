using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.PresentationServices
{
    public interface IUriBuilderService
    {
        public Type ResourceType { get; }
        string GetAllRecordsUrl(string baseUri, PaginationFilter paginationFilter);
        string GetRecordByIdUrl(string baseUri, string id);
    }
}

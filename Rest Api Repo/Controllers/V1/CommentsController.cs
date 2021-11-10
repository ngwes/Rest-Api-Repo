using Microsoft.AspNetCore.Mvc;
using Rest_Api_Repo.ResponseModels;
using Rest_Api_Repo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rest_Api_Repo.Routes.V1.ApiRoutes;

namespace Rest_Api_Repo.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Comments.CommentsBase)]
    public class CommentsController : ControllerBase
    {


        [HttpGet(ApiRoutes.Comments.GetAll)]
        [ApiKeyAuth]
        public IActionResult GetAllComments()
        {
            return Ok(new { Message = "Well done"});
        }

    }
}

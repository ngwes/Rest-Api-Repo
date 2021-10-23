using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class Posts
        {
            public const string PostBase = Base + "/posts";
            public const string GetAll = "";
            public const string Get = "{postId}";
            public const string Create = "";
            public const string Update = "{postId}";
            public const string Delete = "{postId}";
        }
        public static class Comments
        {
            public const string CommentsBase = Base + "/comments";
            public const string GetAll = "";
            public const string Get = "{commentId}";
            public const string Create = "";
            public const string Update = "{commentId}";
            public const string Delete = "{commentId}";
        }
        public static class Tags
        {
            public const string TagsBase = Base + "/tags";
            public const string GetAll = "";
            public const string Get = "{tagId}";
            public const string Create = "";
            public const string Update = "{tagId}";
            public const string Delete = "{tagId}";
        }
        public static class Identity
        {
            public const string IdentityBase = Base + "/identity";
            public const string Login = "login";
            public const string Refresh = "refresh";
            public const string Register = "register";
        }
    }
}

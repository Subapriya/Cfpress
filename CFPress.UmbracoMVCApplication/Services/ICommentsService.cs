using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using CFPress.UmbracoMVCApplication.Enumerations;
using CFPress.UmbracoMVCApplication.Models;
using CFPress.UmbracoMVCApplication.Pocos;

namespace CFPress.UmbracoMVCApplication.Services
{
    public  interface ICommentsService
    {
        IEnumerable<Comments> GetCommentsByNewsItemId(int NewsItemId);
        bool AddNewCommentsByNewsItemIdAndMemberId(CommentsViewModel model);
        IEnumerable<Comments> GetAllCommentsOrderByNewsItemId();
        bool UpdateCommentStatus(int commentId, CommentStatus commentStatus);
        IEnumerable<Comments> GetCommentsByCommentStatus(CommentStatus commentStatus);


    }
}

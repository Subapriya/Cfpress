namespace CFPress.UmbracoMVCApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Umbraco.Core.Persistence;
    using CFPress.UmbracoMVCApplication.Pocos;
    using CFPress.UmbracoMVCApplication.Models;
    using CFPress.UmbracoMVCApplication.Enumerations;

    public class CommentsService : ICommentsService
    {
        public Umbraco.Core.Persistence.Database umbDatabase;
        /// <summary>
        /// Add new comments to the comments table
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <param name="memberId"></param>
        /// <param name="umbDatabase"></param>
        /// <returns></returns>
        public bool AddNewCommentsByNewsItemIdAndMemberId(CommentsViewModel model)
        {
            bool isSucess = false;
            try
            {
                if (this.umbDatabase.TableExist("Comments"))
                {
                    Comments pocoComments = new Comments();
                    pocoComments.NewsItemUmbracoId = model.UmbracoNewsItemId;
                    pocoComments.MemberId = model.MemberId;
                    pocoComments.CommentText = model.CommentText;
                    pocoComments.Status = Convert.ToInt32(model.CommentStatus);

                    //// Add the object to the DB
                    umbDatabase.Insert(pocoComments);
                    isSucess = true;
                   }
            }

            catch (Exception ex)
            {
                //// on exception abort the transaction
                umbDatabase.AbortTransaction();
                throw new Exception(ex.Message);
            }
            return isSucess;
        }


        public IEnumerable<Comments> GetCommentsByNewsItemId(int NewsItemId)
        {
            IEnumerable<Comments> commentsCollection = this.umbDatabase.Query<Comments>(Comments.GetCommentsByNewsItemIdSql(NewsItemId)).Where(element => element.Status == Convert.ToInt32(CommentStatus.Approve));
            return commentsCollection;
        }

        public IEnumerable<Comments> GetAllCommentsOrderByNewsItemId()
        {
            IEnumerable<Comments> commentsCollection = this.umbDatabase.Query<Comments>(Comments.GetAllCommentsOrderByNewsItemIdSql());
            return commentsCollection;
        }

        public bool UpdateCommentStatus(int commentId, CommentStatus commentStatus)
        {
            Comments commenttoUpdate = this.umbDatabase.Query<Comments>(Comments.GetCommentById(commentId)).FirstOrDefault();
            commenttoUpdate.Status = Convert.ToInt32(commentStatus);
            this.umbDatabase.Update(commenttoUpdate);
            return true;
        }
       
        public IEnumerable<Comments> GetCommentsByCommentStatus(CommentStatus commentStatus)
        {
            IEnumerable<Comments> commentsCollection = this.umbDatabase.Query<Comments>(Comments.GetCommentsByStatusSql(commentStatus));
            return commentsCollection;
        }
    }
}
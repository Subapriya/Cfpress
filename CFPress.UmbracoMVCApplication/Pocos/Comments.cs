using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using CFPress.UmbracoMVCApplication.Enumerations;

namespace CFPress.UmbracoMVCApplication.Pocos
{
    [TableName("Comments")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class Comments
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int CommentId { get; set; }

        [Column("NewsItemUmbracoId")]
        public int NewsItemUmbracoId { get; set; }

        [Column("MemberId")]
        public int MemberId { get; set; }
      
        [Column("CommentText")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string CommentText { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        public static string GetCommentsByNewsItemIdSql(int newsItemId)
        {
            string sqlStatement = string.Format("SELECT * FROM Comments WHERE NewsItemUmbracoId= '{0}'", newsItemId);
            return sqlStatement;
        }

        public static string GetCommentsByNewsItemIdAndMemberIdSql(int newsItemId, int memberId)
        {
            string sqlStatement = string.Format("SELECT * FROM Comments WHERE NewsItemUmbracoId= '{0}' AND MemberId = '{1}'", newsItemId, memberId);
            return sqlStatement;
        }

        public static string GetAllCommentsOrderByNewsItemIdSql()
        {
            string sqlStatement = string.Format("SELECT * FROM Comments order by Comments.NewsItemUmbracoId");
            return sqlStatement;
        }

        public static string GetCommentsByAllSql(int newsItemId, int memberId, string commentText)
        {
            string sqlStatement = string.Format("SELECT * FROM Comments WHERE NewsItemUmbracoId= '{0}' AND MemberId = '{1}' AND CommentText ='{2}'", newsItemId, memberId, commentText);
            return sqlStatement;
        }

        public static string GetCommentById (int commentId)
        {
            string sqlStatement = string.Format("SELECT * FROM Comments WHERE Id= '{0}'", commentId);
            return sqlStatement;
        }

        public static string GetCommentsByStatusSql(CommentStatus commentStatus)
        {
            string sqlStatement = string.Format("SELECT * FROM Comments WHERE Status= '{0}'", Convert.ToInt32(commentStatus));
            return sqlStatement;
        }
    }
}
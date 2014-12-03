using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Persistence.Repositories;

namespace CFPress.UmbracoMVCApplication.Pocos
{
    [TableName("Competitions")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class Competitions
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("EventName")]
        public string Name { get; set; }

        [Column("EventNewsPaperName")]
        public string NewsPaperName { get; set; }

        [Column("FormDetailsXml")]
        public string FormDetailsXml { get; set; }

        [Column("EarnedVotes")]
        public int EarnedVotes { get; set; }

        [Column("MemberId")]
        public int MemberId { get; set; }


        //[ForeignKey(typeof(int),Name = "FK_Competitions_cmsMember", Column = "nodeId")]
        //public int MemberId { get; set; }

        //[ForeignKey(typeof(Umbraco.Core.Models.Member),Name="FK_Competitions_cmsMember")]
        //public Umbraco.Core.Persistence.Repositories.IMemberRepository Member { get; set; }
        //public static string  AddNewEntryByAllSql(string name, string newspapername, string formDetails)
        //{
        //    string sqlStatement = string.Format("SELECT * FROM Comments WHERE NewsItemUmbracoId= '{0}'", newsItemId);
        //    return sqlStatement;
        //}

        public static string GetEntryByIdSql(int entryId)
        {
            string sqlStatement = string.Format("SELECT * FROM Competitions WHERE Id = '{0}'", entryId);
            return sqlStatement;
        }

        public static string UpdateVotesByEntryIdSql(int entryId, int votes)
        {
            string sqlStatement = string.Format("SET EarnedVotes = '{0}' WHERE Id ='{1}'", votes, entryId);
            return sqlStatement;
        }

       }
}
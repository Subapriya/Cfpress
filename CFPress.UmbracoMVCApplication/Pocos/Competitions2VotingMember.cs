using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace CFPress.UmbracoMVCApplication.Pocos
{
   
    [TableName("Competitions2VotingMember")]
    [ExplicitColumns]
    public class Competitions2VotingMember
    {
        [Column("EntryId")]
        [PrimaryKeyColumn(AutoIncrement = false, OnColumns = "EntryId,VotingMemberId")]
        [ForeignKey(typeof(Competitions), Column = "Id", Name = "FK_Competitions2VotingMember_Competitions_entryId")]
        public int EntryId { get; set; }

        [Column("VotingMemberId")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        public int VotingMemberId { get; set; }
            
    }
}
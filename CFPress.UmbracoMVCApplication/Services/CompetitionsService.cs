using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFPress.UmbracoMVCApplication.Models;
using CFPress.UmbracoMVCApplication.Pocos;
using Umbraco.Core.Persistence;

namespace CFPress.UmbracoMVCApplication.Services
{
    public class CompetitionsService : ICompetitionsService
    {
        public Umbraco.Core.Persistence.Database umbDatabase;

        /// <summary>
        /// Add new comments to the comments table
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <param name="memberId"></param>
        /// <param name="umbDatabase"></param>
        /// <returns></returns>
        public int AddNewCompetitionEntry(CompetitionsViewModel model)
        {
            int competitionEntryId = 0;
            try
            {
                if (this.umbDatabase.TableExist("Competitions"))
                {
                    Competitions pocoEntry = new Competitions();
                    pocoEntry.Name = model.CompetitionName;
                    pocoEntry.NewsPaperName = model.NewspaperName;
                    pocoEntry.FormDetailsXml = model.Formdetailsxml;
                    //// Add the object to the DB
                    object identity =  umbDatabase.Insert(pocoEntry);
                    competitionEntryId = Convert.ToInt32(identity);
                }
            }
            catch (Exception ex)
            {
                //// on exception abort the transaction
                umbDatabase.AbortTransaction();
                throw new Exception(ex.Message);
            }
            return competitionEntryId;
        }

        /// <summary>
        ///  Get entry by Id
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public Competitions GetEntryById(int entryId)
        {
            Competitions competitionEntry =  umbDatabase.Query<Competitions>(Competitions.GetEntryByIdSql(entryId)).FirstOrDefault();
            return competitionEntry;
        }

        /// <summary>
        /// Save the earned votes by entryid
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="earnedVotes"></param>
        /// <returns></returns>
        public bool SaveEarnedVotesByEntryId(int entryId, int earnedVotes)
        {
            //Competitions pocoEntry =  new Competitions();
            //pocoEntry.Id = entryId;
            //pocoEntry.EarnedVotes = earnedVotes;
            umbDatabase.Update<Competitions>(Competitions.UpdateVotesByEntryIdSql(entryId, earnedVotes));
            return false;
        }

        /// <summary>
        ///  Save voting member Ides by entryId
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public bool SaveVotingMemberIdes(int entryId, int memberId)
        {
            Competitions2VotingMember pocoupdate = new Competitions2VotingMember();
            pocoupdate.EntryId = entryId;
            pocoupdate.VotingMemberId = memberId;
            umbDatabase.Insert(pocoupdate);
            return false;
        }
    }
    }

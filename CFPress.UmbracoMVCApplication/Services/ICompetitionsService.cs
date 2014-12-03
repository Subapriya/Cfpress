using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFPress.UmbracoMVCApplication.Models;
using CFPress.UmbracoMVCApplication.Pocos;

namespace CFPress.UmbracoMVCApplication.Services
{
    public interface ICompetitionsService
    {
        int AddNewCompetitionEntry(CompetitionsViewModel model);
        Competitions GetEntryById(int entryId);
        bool SaveEarnedVotesByEntryId(int entryId, int votes);
        bool SaveVotingMemberIdes(int entryId, int memberId);
    }
}

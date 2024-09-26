using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace InvertItService.DataObjects
{
    public class UserRanking
    {
        public int Id { get; set; }

        public virtual UserAccount UserAccount { get; set; }

        public string UserAccountId { get; set; }

        public int PointsEasy { get; set; }

        public int PointsMedium { get; set; }

        public int PointsHard { get; set; }
        
        public int ChallengesFinished { get; set; }
        public int ChallengesLeft { get; set; }

    }
}
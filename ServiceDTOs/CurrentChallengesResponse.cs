using System.Collections.Generic;

namespace ServiceDTOs
{
    public class CurrentChallengesResponse
    {
        public List<CurrentChallenge> CurrentChallenges { get; set; }

        public CurrentChallengesResponse()
        {
            CurrentChallenges = new List<CurrentChallenge>();
        }
    }
}

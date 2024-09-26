using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.CustomControls.Challenges;
using NoNameGame.Models.ChallengeGame;
using ServiceDTOs;

namespace NoNameGame.Controllers.GameLogic.Challenges
{
    public class ChallengesController
    {
        private readonly IMobileServiceClient _mobileServiceClient;
        public ChallengesController(IMobileServiceClient mobileServiceClient)
        {
            if (mobileServiceClient == null) throw new ArgumentNullException("mobileServiceClient");
            _mobileServiceClient = mobileServiceClient;
        }
        public async Task<CurrentChallengesModel> GetCurrentChallenges()
        {
            var result = await _mobileServiceClient.
                InvokeApiAsync<CurrentChallengesResponse>("CurrentChallenges",HttpMethod.Get,null);

            var currentChallengesModel = new CurrentChallengesModel();
            currentChallengesModel.Challenges.AddRange(result.CurrentChallenges.Select(x=>new CurrentChallengeModel() {Id = x.Id,Name = x.Name}));
            return currentChallengesModel;
        }
    }
}
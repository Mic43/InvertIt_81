using System;
using System.Collections.Generic;
using System.Linq;
using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    public class AllCondtitionsMet : IGameWonCondition
    {
        private readonly IEnumerable<IGameWonCondition> _conditions;
        public AllCondtitionsMet(IEnumerable<IGameWonCondition> conditions)
        {
            if (conditions == null) throw new ArgumentNullException("conditions");
            _conditions = conditions;
        }
        public AllCondtitionsMet(params IGameWonCondition[] conditions) : this(conditions.AsEnumerable())
        {        
        }
        public bool IsTrue(GameWon gameWon)
        {
            return _conditions.All(x => x.IsTrue(gameWon));
        }
    }
}
namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public abstract class ProgressEvent : IDomainEvent
    {
        public int ProgressChange { get; protected set; }

        protected ProgressEvent(int progressChange = 1)
        {
            ProgressChange = progressChange;
        }
    }
}
namespace NoNameGame.Helpers.DateTime
{
    public class DateTimeNowProvider : IDateTimeNowProvider
    {
        public System.DateTime GetNow()
        {
            return System.DateTime.Now;
        }
    }
}
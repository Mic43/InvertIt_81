namespace NoNameGame.Controllers.GameLogic.Challenges.DTO
{
    public class RegistrationResponse
    {
        public bool IsSuccess { get; set; }
        public RegistrationResult RegistrationResult { get; set; }

        public RegistrationResponse(bool isSuccess, RegistrationResult registrationResult)
        {
            IsSuccess = isSuccess;
            RegistrationResult = registrationResult;
        }
    }

    public enum RegistrationResult
    {
       Ok,
       UserAlreadyExists,
       InvalidPassword,
       InvalidUsername
    }
}
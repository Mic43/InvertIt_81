namespace ServiceDTOs.Login
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
        public static RegistrationResponse CreateSuccess()
        {
            return new RegistrationResponse(true, RegistrationResult.Ok);
        }
        public static RegistrationResponse CreateFailure(RegistrationResult registrationResult)
        {
            return new RegistrationResponse(false, registrationResult);
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
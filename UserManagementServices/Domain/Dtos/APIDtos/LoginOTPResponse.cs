namespace UserManagementServices.Domain.Dtos.APIDtos
{
    public class LoginOTPResponse
    {
        public string Token { get; set; }
        public DateTime expiration { get; set; }
    }
}

namespace UserManagementServices.Domain.Dtos.EmailDtos
{
    public class ConfirmEmail
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime expiration { get; set; }
    }
}

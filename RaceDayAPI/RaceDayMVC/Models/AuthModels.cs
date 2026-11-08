namespace RaceDayMVC.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterViewModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Phone { get; set; }
    }

    public class AuthTokenResponse { public string Token { get; set; } = ""; }

    public class MeResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class ProfileUpdateRequest
    {
        public string FullName { get; set; } = "";
        public string? PhoneNumber { get; set; }
    }
}
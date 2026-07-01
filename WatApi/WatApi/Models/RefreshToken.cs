namespace WatApi.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Selector { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Record the token that replaced it.
        public string? ReplacedByToken { get; set; }
        // Record if the token was revoked due to a security breach or other forceful action.
        public string? RevokeReason { get; set; } = string.Empty;

        // A token is only usable if it has not been revoked and has not expired.
        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

        // Navigation property to the associated User entity.
        public User? User { get; set; }
    }
}

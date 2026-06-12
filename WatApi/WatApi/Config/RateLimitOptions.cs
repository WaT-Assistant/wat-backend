namespace WatApi.Config
{
    public class RateLimitOptions
    {
        // Use authenticated user id as key when available; otherwise use remote IP
        public bool UseUserId { get; set; } = false;

        // Maximum requests allowed per window
        public int PermitLimit { get; set; } = 100;

        // Window length in seconds
        public int WindowSeconds { get; set; } = 60;

        // Optional: apply limits per route (true) or globally per client (false)
        public bool PerRoute { get; set; } = false;
    }
}
using System;
namespace WOT_Models
{
	public class JWTSettings
	{
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string TokenExpiryTimeInMinutes { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}


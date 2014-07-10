using System;

namespace MyRepository.Entity
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }

        public bool IsAdmin { get; set; }

        #region Password Management
        public string PasswordHash { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetRequestedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public int LoginTryCount { get; set; } 
        #endregion
    }
}
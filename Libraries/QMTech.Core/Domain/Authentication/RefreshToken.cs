using System;

namespace QMTech.Core.Domain.Authentication
{
    public class RefreshToken : BaseEntity
    {
        public string HashId { get; set; }

        public string Subject { get; set; }

        public DateTime IssuedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public string ProtectedTicket { get; set; }
    }
}

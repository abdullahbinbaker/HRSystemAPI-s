using System;

namespace Core.Entity
{
    public class RefreshToken: EntityBase
    {
        public int EmployeeId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}

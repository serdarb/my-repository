using System;

namespace MyRepository.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedAt = UpdatedAt = DateTime.Now;
            IsActive = true;
            PublicId = Guid.NewGuid().ToString().Replace("-", "");
        }

        public long Id { get; set; }
        public string PublicId { get; set; }

        public bool IsActive { get; set; }

        public long UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
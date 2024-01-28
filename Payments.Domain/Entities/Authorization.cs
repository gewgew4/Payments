using Payments.Domain.Enums;

namespace Payments.Domain.Entities
{
    public class Authorization : BaseEntity<Authorization>
    {
        public DateTime? AuthorizationDate { get; set; }
        public AuthorizationType AuthorizationType { get; set; }
        public Guid ClientId { get; set; }
        public ClientType ClientType { get; set; }
        public DateTime CreationDate { get; }
        public bool IsAuthorized { get; set; }
        public bool? IsConfirmed { get; set; }
        public decimal Total { get; set; }
    }
}

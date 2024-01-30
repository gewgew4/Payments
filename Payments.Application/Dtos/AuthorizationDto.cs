using Payments.Domain.Enums;

namespace Payments.Application.Dtos
{
    public class AuthorizationDto
    {
        public AuthorizationType AuthorizationType { get; set; }
        public Guid ClientId { get; set; }
        public ClientType ClientType { get; set; }
        public bool IsAuthorized { get; set; }
        public bool? IsConfirmed { get; set; }
        public decimal Total { get; set; }
    }
}

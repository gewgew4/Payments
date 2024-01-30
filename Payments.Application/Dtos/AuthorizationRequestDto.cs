using Payments.Domain.Enums;

namespace Payments.Application.Dtos
{
    public class AuthorizationRequestDto
    {
        public AuthorizationType AuthorizationType { get; set; }
        public Guid ClientId { get; set; }
        public ClientType ClientType { get; set; }
        public decimal Total { get; set; }
    }
}

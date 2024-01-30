namespace Payments.Application.Dtos
{
    public class ApprovedAuthorizationDto
    {
        public DateTime AuthorizationDate { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
    }
}

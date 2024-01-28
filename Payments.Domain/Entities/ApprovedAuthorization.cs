namespace Payments.Domain.Entities
{
    internal class ApprovedAuthorization
    {
        public DateTime AuthorizationDate { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
    }
}

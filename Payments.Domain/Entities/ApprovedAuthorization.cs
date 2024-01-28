namespace Payments.Domain.Entities
{
    public class ApprovedAuthorization : BaseEntity<ApprovedAuthorization>
    {
        public DateTime AuthorizationDate { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
    }
}

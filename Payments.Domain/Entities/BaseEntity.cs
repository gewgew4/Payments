namespace Payments.Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        public Guid Id { get; set; }
    }
}

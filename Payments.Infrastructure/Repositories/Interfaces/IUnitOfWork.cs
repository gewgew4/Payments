namespace Payments.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApprovedAuthorizationRepository ApprovedAuthorizationRepository { get; }
        IAuthorizationRepository AuthorizationRepository { get; }
        Task<int> SaveAsync();
    }
}

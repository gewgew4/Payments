using Payments.Infrastructure.Repositories;
using Payments.Infrastructure.Repositories.Interfaces;

namespace Payments.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PbContext _context;

        public UnitOfWork(PbContext context)
        {
            _context = context;
            ApprovedAuthorizationRepository = new ApprovedAuthorizationRepository(_context);
            AuthorizationRepository = new AuthorizationRepository(_context);
        }

        public IApprovedAuthorizationRepository ApprovedAuthorizationRepository { get; private set; }
        public IAuthorizationRepository AuthorizationRepository { get; private set; }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

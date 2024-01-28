using Payments.Domain.Entities;
using Payments.Infrastructure.Repositories.Interfaces;

namespace Payments.Infrastructure.Repositories
{
    public class ApprovedAuthorizationRepository(PbContext context) : GenericRepository<ApprovedAuthorization>(context), IApprovedAuthorizationRepository;
}

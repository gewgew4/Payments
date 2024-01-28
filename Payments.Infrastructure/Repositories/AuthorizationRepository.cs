using Payments.Domain.Entities;
using Payments.Infrastructure.Repositories.Interfaces;

namespace Payments.Infrastructure.Repositories
{
    public class AuthorizationRepository(PbContext context) : GenericRepository<Authorization>(context), IAuthorizationRepository;
}

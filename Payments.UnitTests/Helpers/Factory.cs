using Payments.Application.Dtos;
using Payments.Application.Models;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.UnitTests.Helpers
{
    public class Factory
    {
        private static readonly Random random = new();

        public static AuthorizationDto GetAuthorizationDto()
        {
            return new AuthorizationDto
            {
                AuthorizationType = (AuthorizationType)random.Next(0, 2),
                ClientId = Guid.NewGuid(),
                ClientType = ClientType.Authorization,
                IsAuthorized = true,
                IsConfirmed = null,
                Total = random.Next(10, 5001)
            };
        }

        public static List<AuthorizationDto> GetAuthorizationDtos(int quantity)
        {
            var result = new List<AuthorizationDto>();
            for (int i = 0; i < quantity; i++)
            {
                result.Add(GetAuthorizationDto());
            }

            return result;
        }

        public static AuthorizationRequestDto GetAuthorizationRequestDto()
        {
            return new AuthorizationRequestDto
            {
                AuthorizationType = (AuthorizationType)random.Next(0, 2),
                ClientId = Guid.NewGuid(),
                ClientType = ClientType.Authorization,
                Total = 1000
            };
        }

        public static ApprovedAuthorizationDto GetApprovedAuthorizationDto()
        {
            return new ApprovedAuthorizationDto
            {
                AuthorizationDate = DateTime.UtcNow,
                ClientId = Guid.NewGuid(),
                Total = random.Next(10, 5001)
            };
        }

        public static List<ApprovedAuthorizationDto> GetApprovedAuthorizationDtos(int quantity)
        {
            var result = new List<ApprovedAuthorizationDto>();
            for (int i = 0; i < quantity; i++)
            {
                result.Add(GetApprovedAuthorizationDto());
            }

            return result;
        }

        public static Authorization GetConfirmableAuthorization(Guid? guid = null)
        {
            if (guid == null)
            {
                guid = Guid.NewGuid();
            }

            return new Authorization
            {
                Id = (Guid)guid,
                AuthorizationDate = DateTime.UtcNow,
                AuthorizationType = (AuthorizationType)random.Next(0, 2),
                ClientId = Guid.NewGuid(),
                ClientType = ClientType.AuthorizationConfirmation,
                CreationDate = DateTime.UtcNow,
                IsAuthorized = true,
                IsConfirmed = null,
                Total = random.Next(10, 5001)
            };
        }

        public static Authorization GetUnconfirmableAuthorization(Guid? guid = null)
        {
            if (guid == null)
            {
                guid = Guid.NewGuid();
            }

            return new Authorization
            {
                Id = (Guid)guid,
                AuthorizationDate = DateTime.UtcNow.AddYears(-10),
                AuthorizationType = (AuthorizationType)random.Next(0, 2),
                ClientId = Guid.NewGuid(),
                ClientType = ClientType.AuthorizationConfirmation,
                CreationDate = DateTime.UtcNow.AddYears(-10),
                IsAuthorized = true,
                IsConfirmed = null,
                Total = random.Next(10, 5001)
            };
        }

        public static List<Authorization> GetUnconfirmableAuthorizations(int quantity)
        {
            var result = new List<Authorization>();
            for (int i = 0; i < quantity; i++)
            {
                result.Add(GetUnconfirmableAuthorization());
            }

            return result;
        }

        public static ConfirmationRequestDto GetConfirmationRequestDto()
        {
            return new ConfirmationRequestDto
            {
                Id = Guid.NewGuid()
            };
        }

        public static ExternalResponse GetExternalResponse(bool isAuthorized = true)
        {
            return new ExternalResponse
            {
                Id = Guid.NewGuid(),
                IsAutorized = isAuthorized
            };
        }
    }
}

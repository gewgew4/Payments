using Dapper;
using EasyCronJob.Abstractions;
using Microsoft.Data.SqlClient;

namespace ExternalProcessor.Services
{
    public class CronJob : CronJobService
    {
        private readonly ILogger<CronJob> _logger;

        public CronJob(ICronConfiguration<CronJob> cronConfiguration, ILogger<CronJob> logger)
            : base(cronConfiguration.CronExpression, cronConfiguration.TimeZoneInfo, cronConfiguration.CronFormat)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start");
            return base.StartAsync(cancellationToken);
        }


        protected override Task ScheduleJob(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduled");
            return base.ScheduleJob(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Do Work");
            Reverse();
            return base.DoWork(cancellationToken);
        }

        private void Reverse()
        {
            string sql = $@"update Authorizations set
                                IsConfirmed = 0
	                            output
		                             newid(),deleted.AuthorizationDate,2,deleted.ClientId,deleted.ClientType,deleted.IsAuthorized,1,deleted.Total,getutcdate(), cast(deleted.Id as nvarchar(38)) + N' reversal',getutcdate()
	                            into Authorizations(Id,AuthorizationDate,AuthorizationType,ClientId,ClientType,IsAuthorized,IsConfirmed,Total,ConfirmationDate,Observations,CreationDate)
                            where IsConfirmed is null and
                                ClientType = 2 and
                                getutcdate() > dateadd(minute, 5, CreationDate)";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=payments;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Execute(sql);
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class TimeRegistrationGrpcService : TimeRegistration.TimeRegistrationBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ILogger<TimeRegistrationGrpcService> _logger;
        private readonly ITimeRegistrationService _timeRegistrationService;

        public TimeRegistrationGrpcService(ITimeRegistrationService timeRegistrationService,
            ILogger<TimeRegistrationGrpcService> logger, IAssignmentService assignmentService)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            _timeRegistrationService = timeRegistrationService;
            _assignmentService = assignmentService;
        }

        public override Task<TimeRegistrationResponse> CreateTimeRegistration(CreateTimeRegistrationRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Create TimeRegistration called");
            AssignmentEntity assignment = _assignmentService.GetById(request.AssignmentId);
            TimeRecordEntity timeRecordEntity = new()
            {
                Date = request.Date.ToDateTime(),
                
                AssignmentEntity = assignment,
                AssignmentId = request.AssignmentId,
                TimeRecorded = request.TimeRecorded.ToTimeSpan()
            };

            var response = _timeRegistrationService.RegisterTime(timeRecordEntity, timeRecordEntity.AssignmentId);

            // Should there be individual steps for this?
            return Task.FromResult(BoolToTimeRegistrationResponseConverter(response));
        }

        //Helper method
        private static TimeRegistrationResponse BoolToTimeRegistrationResponseConverter(bool value)
        {
            TimeRegistrationResponse response = new()
            {
                Success = value
            };

            return response;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
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
        private readonly ILogger<TimeRegistrationService> _logger;
        private readonly ITimeRegistrationService _timeRegistrationService;

        public TimeRegistrationGrpcService(ITimeRegistrationService timeRegistrationService,
            ILogger<TimeRegistrationService> logger, IAssignmentService assignmentService)
        {
            _timeRegistrationService = timeRegistrationService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public override Task<TimeRegistrationsByAssignmentId> GetTimeRegistrationByAssignmentId(GetTimeRegistrationByAssignmentIdRequest request, ServerCallContext context)
        {
            return base.GetTimeRegistrationByAssignmentId(request, context);
        }

        public override Task<TotalTimeSpentOnAssignment> GetTotalTimeRegisteredOnAssignmentId(GetTimeRegistrationByAssignmentIdRequest request, ServerCallContext context)
        {
            return base.GetTotalTimeRegisteredOnAssignmentId(request, context);
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
        
        
        
        private static IEnumerable<TimeRecordEntity> TimeRegistrationEntityToTimeRegistrationResponse(IEnumerable<TimeRecordEntity> timeRegistrations)
        {
            var response = new List<TimeRecordEntity>();
            foreach (TimeRecordEntity timeRegistration in timeRegistrations)
            {
                response.Add(new TimeRecordEntity()
                {
                    Date = timeRegistration.Date,
                    Id = timeRegistration.Id,
                    AssignmentEntity = timeRegistration.AssignmentEntity,
                    AssignmentId = timeRegistration.AssignmentId,
                    TimeRecorded = timeRegistration.TimeRecorded
                });
            }

            return response;
        }
    }
}
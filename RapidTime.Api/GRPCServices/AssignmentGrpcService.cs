using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class AssignmentGrpcService : Assignment.AssignmentBase
    {
        private ILogger<AssignmentTypeGrpcService> _logger;
        private IAssignmentService _assignmentService;

        public AssignmentGrpcService(IAssignmentService assignmentService, ILogger<AssignmentTypeGrpcService> logger)
        {
            _assignmentService = assignmentService;
            _logger = logger;
        }

        public override Task<AssignmentResponse> CreateAssignment(CreateAssignmentRequest request, ServerCallContext context)
        {
            var EntityToCreate = new AssignmentEntity()
            {
                Amount = 0,
                AssignmentTypeId = request.AssignmentType.Id,
                TimeSpentInTotal = TimeSpan.Zero,
                CustomerId = request.Customer.Id,
                DateStarted = DateTime.Today,
                UserId = Guid.Parse(request.UserId)
            };
        
            var assignmentIdToReturn = _assignmentService.Insert(EntityToCreate);
            var entity = _assignmentService.GetById(assignmentIdToReturn);

            return Task.FromResult(EntityToResponse(entity));

        }

        public override Task<AssignmentResponse> GetAssignment(AssignmentRequest request, ServerCallContext context)
        {
            var entityToReturn = _assignmentService.GetById(request.Id);
            return Task.FromResult(EntityToResponse(entityToReturn));
        }

        public override Task<Empty> DeleteAssignment(AssignmentRequest request, ServerCallContext context)
        {
            _assignmentService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<AssignmentResponse> UpdateAssignment(UpdateAssignmentRequest request, ServerCallContext context)
        {
            var entity = _assignmentService.GetById(request.Id);
            
            if (entity.Amount != request.Amount)
                entity.Amount = request.Amount;
            
            if ( entity.CustomerId != request.Customer.Id)
                entity.CustomerId = request.Customer.Id;
            
            if (entity.DateStarted != request.DateStarted.ToDateTime())
                entity.DateStarted = request.DateStarted.ToDateTime();

            // if (entity.UserId != new Guid(request.Base.UserId))
            // {
            //     //find user and insert user on the item.
            // }

            _assignmentService.Update(entity);

            return Task.FromResult(EntityToResponse(entity));
        }

        public override Task<MultiAssignmentResponse> GetMultiAssignment(AssignmentRequest request, ServerCallContext context)
        {
            var multiAssignmentRespone = new MultiAssignmentResponse();

            var assignments = _assignmentService.GetAll();

            foreach (var assignment in assignments)
            {
                multiAssignmentRespone.AssignmentResponse.Add(EntityToResponse(assignment));
            }

            return Task.FromResult(multiAssignmentRespone);
        }

        public override Task<MultiAssignmentResponse> GetMultiAssignmentByCustomerId(MultiAssignmentByCustomerId request, ServerCallContext context)
        {
            var multiAssignmentRespone = new MultiAssignmentResponse();

            var allAssignments = _assignmentService.GetAll();

            var assignments = allAssignments.Where(x => x.CustomerId == request.CustomerId);
            
            foreach (var assignment in assignments)
            {
                multiAssignmentRespone.AssignmentResponse.Add(EntityToResponse(assignment));
            }

            return Task.FromResult(multiAssignmentRespone);
        }

        private AssignmentResponse EntityToResponse(AssignmentEntity assignmentEntity)
        {
            return new()
            {
                Id = assignmentEntity.Id,
                Amount = assignmentEntity.Amount,
                Date = assignmentEntity.DateStarted.ToTimestamp(),
                TimeSpent = assignmentEntity.TimeSpentInTotal.ToString(),
                Customer = new()
                {
                    Id = assignmentEntity.CustomerId
                },
                AssignmentType = assignmentEntity.AssignmentTypeId.ToString()
            };
        }
    }
}
using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class AssignmentTypeGrpcService : AssignmentType.AssignmentTypeBase
    {
        private readonly ILogger<AssignmentTypeGrpcService> _logger;
        private readonly IAssignmentTypeService _assignmentTypeService;

        public AssignmentTypeGrpcService(IAssignmentTypeService assignmentTypeService,
            ILogger<AssignmentTypeGrpcService> logger)
        {
            _assignmentTypeService = assignmentTypeService;
            _logger = logger;
        }

        public override Task<AssignmentTypeResponse> CreateAssignmentType(CreateAssignmentTypeRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Create AssignmentType Called with the request {@Request}", request);
            var assignmentTypeToBeCreated = new AssignmentTypeEntity()
            {
                Name = request.Name,
                Number = request.Number,
                InvoiceAble = request.InvoiceAble
            };

            var id = _assignmentTypeService.Insert(assignmentTypeToBeCreated);
            
            var createdAssignmentType = _assignmentTypeService.GetById(id);
            var toReturn = EntityToResponse(createdAssignmentType) ?? throw new ArgumentNullException(nameof(request));
            return Task.FromResult(toReturn);
        }

        public override Task<AssignmentTypeResponse> GetAssignmentType(GetAssignmentTypeRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get AssignmentType called with Id: {Id}", request.Id);
            var assignmentToReturn = _assignmentTypeService.GetById(request.Id);
            var toReturn = EntityToResponse(assignmentToReturn) ?? throw new ArgumentNullException(nameof(request));
            return Task.FromResult(toReturn);

        }

        public override Task<Empty> DeleteAssignmentType(DeleteAssignmentTypeRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Delete AssignmentType called with Id: {Id}", request.Id);
            _assignmentTypeService.Delete(request.Id);
            return Task.FromResult(new Empty());

        }

        public override Task<AssignmentTypeResponse> UpdateAssignmentType(UpdateAssignmentTypeRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Update AssignmentType Called");
            var assignmentTypeToUpdate = _assignmentTypeService.GetById(request.Id);
            if (!String.IsNullOrEmpty(request.Name))
                assignmentTypeToUpdate.Name = request.Name;
            
            if (!String.IsNullOrEmpty(request.Number))
                assignmentTypeToUpdate.Number = request.Number;

            assignmentTypeToUpdate.InvoiceAble = request.InvoiceAble;
            
            _assignmentTypeService.Update(assignmentTypeToUpdate);

            var updatedAssignmentType = _assignmentTypeService.GetById(assignmentTypeToUpdate.Id);
            var toReturn = EntityToResponse(updatedAssignmentType) ?? throw new ArgumentNullException(nameof(request));
            return Task.FromResult(toReturn);
        }

        public override async Task MultiAssignmentType(IAsyncStreamReader<GetMultiAssignmentTypeRequest> requestStream, IServerStreamWriter<MultiAssignmentTypeResponse> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var input = requestStream.Current.Input;
                var items = _assignmentTypeService.GetByNameOrNumber(input);
                MultiAssignmentTypeResponse multiAssignmentTypeResponse = new MultiAssignmentTypeResponse();
                foreach (var item in items)
                {
                    multiAssignmentTypeResponse.AssignmentType.Add(EntityToResponse(item));
                }

                await responseStream.WriteAsync(multiAssignmentTypeResponse);
            }
        }

        public override Task<MultiAssignmentTypeResponse> GetAllAssignmentType(Empty request, ServerCallContext context)
        {
            MultiAssignmentTypeResponse multiAssignmentTypeResponse = new MultiAssignmentTypeResponse();
            var items = _assignmentTypeService.GetAll();
            foreach (var item in items)
            {
                multiAssignmentTypeResponse.AssignmentType.Add(EntityToResponse(item));
            }
            return Task.FromResult(multiAssignmentTypeResponse);
        }

        private AssignmentTypeResponse EntityToResponse(AssignmentTypeEntity assignmentTypeEntity)
        {
            return new()
            {
                Id = assignmentTypeEntity.Id,
                InvoiceAble = assignmentTypeEntity.InvoiceAble,
                Name = assignmentTypeEntity.Name,
                Number = assignmentTypeEntity.Number
            };
        }
    }
}
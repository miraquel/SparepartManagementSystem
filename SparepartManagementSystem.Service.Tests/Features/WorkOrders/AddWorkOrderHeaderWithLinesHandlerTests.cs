using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class AddWorkOrderHeaderWithLinesHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositoriesSucceed_AddsHeaderAndLinesThenCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var workOrderHeaderRepositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var workOrderLineRepositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var sequence = new MockSequence();
        var dto = CreateDto();
        WorkOrderHeader? addedHeader = null;
        List<WorkOrderLine>? addedLines = null;
        EventHandler<AddEventArgs>? headerOnBeforeAdd = null;
        EventHandler<AddEventArgs>? linesOnBeforeAdd = null;

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(workOrderHeaderRepositoryMock.Object);
        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(workOrderLineRepositoryMock.Object);

        workOrderHeaderRepositoryMock.InSequence(sequence)
            .Setup(repository => repository.Add(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Callback<WorkOrderHeader, EventHandler<AddEventArgs>?, EventHandler<AddEventArgs>?>((entity, onBeforeAdd, _) =>
            {
                addedHeader = entity;
                headerOnBeforeAdd = onBeforeAdd;
            })
            .Returns(Task.CompletedTask);

        unitOfWorkMock.InSequence(sequence)
            .Setup(unitOfWork => unitOfWork.GetLastInsertedId())
            .ReturnsAsync(42);

        workOrderLineRepositoryMock.InSequence(sequence)
            .Setup(repository => repository.BulkAdd(It.IsAny<IEnumerable<WorkOrderLine>>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Callback<IEnumerable<WorkOrderLine>, EventHandler<AddEventArgs>?, EventHandler<AddEventArgs>?>((entities, onBeforeAdd, _) =>
            {
                addedLines = entities.ToList();
                linesOnBeforeAdd = onBeforeAdd;
            })
            .Returns(Task.CompletedTask);

        unitOfWorkMock.InSequence(sequence)
            .Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new AddWorkOrderHeaderWithLinesHandler(mapper, unitOfWorkMock.Object, repositoryEvents);

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Header added successfully", result.Message);
        Assert.Null(result.Error);
        Assert.NotNull(addedHeader);
        Assert.Equal(dto.Name, addedHeader!.Name);
        Assert.Same(repositoryEvents.OnBeforeAdd, headerOnBeforeAdd);
        Assert.NotNull(addedLines);
        Assert.Equal(2, addedLines!.Count);
        Assert.Same(repositoryEvents.OnBeforeAdd, linesOnBeforeAdd);
        Assert.All(addedLines, line => Assert.Equal(42, line.WorkOrderHeaderId));
        Assert.Equal(dto.WorkOrderLines.First().LineTitle, addedLines[0].LineTitle);
        Assert.Equal(dto.WorkOrderLines.Last().LineTitle, addedLines[1].LineTitle);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task AddWorkOrderHeaderWithLines_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addWorkOrderHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var handlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList.IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getItemRequisitionByIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById.IGetItemRequisitionByIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByWorkOrderLineIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId.IGetItemRequisitionByWorkOrderLineIdHandler>(MockBehavior.Strict);
        var getHeaderByIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById.IGetWorkOrderHeaderByIdHandler>(MockBehavior.Strict);
        var getHeaderByIdWithLinesHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines.IGetWorkOrderHeaderByIdWithLinesHandler>(MockBehavior.Strict);
        var getHeaderByParamsPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList.IGetWorkOrderHeaderByParamsPagedListHandler>(MockBehavior.Strict);
        var getLineByIdHandlerMock = new Mock<IGetWorkOrderLineByIdHandler>(MockBehavior.Strict);
        var getLinesByHeaderIdHandlerMock = new Mock<IGetWorkOrderLineByWorkOrderHeaderIdHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateLineHandlerMock = new Mock<IUpdateWorkOrderLineHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var dto = CreateDto();
        var expectedResponse = new ServiceResponse
        {
            Success = true,
            Message = "Work Order Header added successfully"
        };

        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);
        handlerMock.Setup(handler => handler.Handle(dto))
            .ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addWorkOrderHeaderHandlerMock.Object)
            .AddSingleton(handlerMock.Object)
            .AddSingleton(addLineHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(deleteLineHandlerMock.Object)
            .AddSingleton(getAllPagedListHandlerMock.Object)
            .AddSingleton(getItemRequisitionByIdHandlerMock.Object)
            .AddSingleton(getItemRequisitionByWorkOrderLineIdHandlerMock.Object)
            .AddSingleton(getHeaderByIdHandlerMock.Object)
            .AddSingleton(getHeaderByIdWithLinesHandlerMock.Object)
            .AddSingleton(getHeaderByParamsPagedListHandlerMock.Object)
            .AddSingleton(getLineByIdHandlerMock.Object)
            .AddSingleton(getLinesByHeaderIdHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(updateLineHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.AddWorkOrderHeaderWithLines(dto);

        Assert.Same(expectedResponse, result);
        handlerMock.Verify(handler => handler.Handle(dto), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenBulkAddThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var workOrderHeaderRepositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var workOrderLineRepositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var dto = CreateDto();
        var expectedException = new InvalidOperationException("bulk add failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(workOrderHeaderRepositoryMock.Object);
        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(workOrderLineRepositoryMock.Object);

        workOrderHeaderRepositoryMock.Setup(repository => repository.Add(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.GetLastInsertedId())
            .ReturnsAsync(42);
        workOrderLineRepositoryMock.Setup(repository => repository.BulkAdd(It.IsAny<IEnumerable<WorkOrderLine>>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new AddWorkOrderHeaderWithLinesHandler(mapper, unitOfWorkMock.Object, repositoryEvents);

        var result = await handler.Handle(dto);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    private static WorkOrderHeaderDto CreateDto()
    {
        return new WorkOrderHeaderDto
        {
            Name = "WO-001",
            HeaderTitle = "Planned maintenance",
            WorkOrderLines =
            [
                new WorkOrderLineDto
                {
                    Line = 1,
                    LineTitle = "Inspect pump",
                    EntityId = "PUMP-01",
                    TaskId = "TASK-01"
                },
                new WorkOrderLineDto
                {
                    Line = 2,
                    LineTitle = "Replace seal",
                    EntityId = "PUMP-01",
                    TaskId = "TASK-02"
                }
            ]
        };
    }
}
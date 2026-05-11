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

public class AddWorkOrderLineHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_AddsLineAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        WorkOrderLine? addedLine = null;
        EventHandler<AddEventArgs>? onBeforeAdd = null;

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.Add(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Callback<WorkOrderLine, EventHandler<AddEventArgs>?, EventHandler<AddEventArgs>?>((entity, beforeAdd, _) =>
            {
                addedLine = entity;
                onBeforeAdd = beforeAdd;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.GetLastInsertedId())
            .ReturnsAsync(77);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new AddWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderLineDto
        {
            WorkOrderHeaderId = 12,
            Line = 3,
            LineTitle = "Install gasket"
        };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Line added successfully", result.Message);
        Assert.Null(result.Error);
        Assert.NotNull(addedLine);
        Assert.Equal(dto.WorkOrderHeaderId, addedLine!.WorkOrderHeaderId);
        Assert.Equal(dto.LineTitle, addedLine.LineTitle);
        Assert.Same(repositoryEvents.OnBeforeAdd, onBeforeAdd);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task AddWorkOrderLine_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList.IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getItemRequisitionByIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById.IGetItemRequisitionByIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByWorkOrderLineIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId.IGetItemRequisitionByWorkOrderLineIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByParamsHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByParams.IGetItemRequisitionByParamsHandler>(MockBehavior.Strict);
        var deleteItemRequisitionHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.DeleteItemRequisition.IDeleteItemRequisitionHandler>(MockBehavior.Strict);
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
        var expectedResponse = new ServiceResponse
        {
            Success = true,
            Message = "Work Order Line added successfully"
        };
        var dto = new WorkOrderLineDto
        {
            WorkOrderHeaderId = 12,
            Line = 3,
            LineTitle = "Install gasket"
        };

        addLineHandlerMock.Setup(handler => handler.Handle(dto))
            .ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(addLineHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(deleteLineHandlerMock.Object)
            .AddSingleton(getAllPagedListHandlerMock.Object)
            .AddSingleton(getItemRequisitionByIdHandlerMock.Object)
            .AddSingleton(getItemRequisitionByWorkOrderLineIdHandlerMock.Object)
            .AddSingleton(getItemRequisitionByParamsHandlerMock.Object)
            .AddSingleton(deleteItemRequisitionHandlerMock.Object)
            .AddSingleton(getHeaderByIdHandlerMock.Object)
            .AddSingleton(getHeaderByIdWithLinesHandlerMock.Object)
            .AddSingleton(getHeaderByParamsPagedListHandlerMock.Object)
            .AddSingleton(getLineByIdHandlerMock.Object)
            .AddSingleton(getLinesByHeaderIdHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(updateLineHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.AddWorkOrderLine(dto);

        Assert.Same(expectedResponse, result);
        addLineHandlerMock.Verify(handler => handler.Handle(dto), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var dto = new WorkOrderLineDto
        {
            WorkOrderHeaderId = 12,
            Line = 3,
            LineTitle = "Install gasket"
        };
        var expectedException = new InvalidOperationException("line add failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.Add(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new AddWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);

        var result = await handler.Handle(dto);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
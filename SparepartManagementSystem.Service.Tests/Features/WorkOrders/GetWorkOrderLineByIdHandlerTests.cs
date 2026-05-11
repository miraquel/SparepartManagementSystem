using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Domain;
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

public class GetWorkOrderLineByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_ReturnsMappedLine()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var line = new WorkOrderLine
        {
            WorkOrderLineId = 88,
            WorkOrderHeaderId = 12,
            Line = 5,
            LineTitle = "Inspect motor",
            EntityId = "MOTOR-01",
            TaskId = "TASK-05"
        };

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(88, false))
            .ReturnsAsync(line);

        var handler = new GetWorkOrderLineByIdHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(88, result.Data!.WorkOrderLineId);
        Assert.Equal("Inspect motor", result.Data.LineTitle);
        Assert.Equal("TASK-05", result.Data.TaskId);
        Assert.Null(result.Error);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetWorkOrderLineById_WhenCalled_DelegatesToHandler()
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
        var expectedResponse = new ServiceResponse<WorkOrderLineDto>
        {
            Success = true,
            Data = new WorkOrderLineDto
            {
                WorkOrderLineId = 88,
                LineTitle = "Inspect motor"
            }
        };

        getLineByIdHandlerMock.Setup(handler => handler.Handle(88))
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
            .AddSingleton(getHeaderByIdHandlerMock.Object)
            .AddSingleton(getHeaderByIdWithLinesHandlerMock.Object)
            .AddSingleton(getHeaderByParamsPagedListHandlerMock.Object)
            .AddSingleton(getLineByIdHandlerMock.Object)
            .AddSingleton(getLinesByHeaderIdHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(updateLineHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.GetWorkOrderLineById(88);

        Assert.Same(expectedResponse, result);
        getLineByIdHandlerMock.Verify(handler => handler.Handle(88), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var expectedException = new InvalidOperationException("line read failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(88, false))
            .ThrowsAsync(expectedException);

        var handler = new GetWorkOrderLineByIdHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
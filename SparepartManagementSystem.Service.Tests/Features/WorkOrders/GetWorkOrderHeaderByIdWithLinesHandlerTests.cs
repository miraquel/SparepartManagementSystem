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
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class GetWorkOrderHeaderByIdWithLinesHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_ReturnsMappedHeaderWithLines()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var header = new WorkOrderHeader
        {
            WorkOrderHeaderId = 77,
            AGSEAMWOID = "WO-77",
            HeaderTitle = "Inspect gearbox",
            WorkOrderLines =
            [
                new WorkOrderLine
                {
                    WorkOrderLineId = 10,
                    WorkOrderHeaderId = 77,
                    Line = 1,
                    LineTitle = "Inspect seals",
                    TaskId = "TASK-10"
                },
                new WorkOrderLine
                {
                    WorkOrderLineId = 11,
                    WorkOrderHeaderId = 77,
                    Line = 2,
                    LineTitle = "Replace oil",
                    TaskId = "TASK-11"
                }
            ]
        };

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByIdWithLines(77))
            .ReturnsAsync(header);

        var handler = new GetWorkOrderHeaderByIdWithLinesHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(77);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(77, result.Data!.WorkOrderHeaderId);
        Assert.Equal("Inspect gearbox", result.Data.HeaderTitle);
        Assert.Equal(2, result.Data.WorkOrderLines.Count);
        Assert.Contains(result.Data.WorkOrderLines, line => line.WorkOrderLineId == 10 && line.LineTitle == "Inspect seals");
        Assert.Contains(result.Data.WorkOrderLines, line => line.WorkOrderLineId == 11 && line.LineTitle == "Replace oil");
        Assert.Null(result.Error);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetWorkOrderHeaderByIdWithLines_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList.IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getHeaderByIdHandlerMock = new Mock<IGetWorkOrderHeaderByIdHandler>(MockBehavior.Strict);
        var getHeaderByIdWithLinesHandlerMock = new Mock<IGetWorkOrderHeaderByIdWithLinesHandler>(MockBehavior.Strict);
        var getHeaderByParamsPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList.IGetWorkOrderHeaderByParamsPagedListHandler>(MockBehavior.Strict);
        var getLineByIdHandlerMock = new Mock<IGetWorkOrderLineByIdHandler>(MockBehavior.Strict);
        var getLinesByHeaderIdHandlerMock = new Mock<IGetWorkOrderLineByWorkOrderHeaderIdHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateLineHandlerMock = new Mock<IUpdateWorkOrderLineHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var expectedResponse = new ServiceResponse<WorkOrderHeaderDto>
        {
            Success = true,
            Data = new WorkOrderHeaderDto
            {
                WorkOrderHeaderId = 77,
                HeaderTitle = "Inspect gearbox",
                WorkOrderLines =
                [
                    new WorkOrderLineDto
                    {
                        WorkOrderLineId = 10,
                        LineTitle = "Inspect seals"
                    }
                ]
            }
        };

        getHeaderByIdWithLinesHandlerMock.Setup(handler => handler.Handle(77))
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
            .AddSingleton(getHeaderByIdHandlerMock.Object)
            .AddSingleton(getHeaderByIdWithLinesHandlerMock.Object)
            .AddSingleton(getHeaderByParamsPagedListHandlerMock.Object)
            .AddSingleton(getLineByIdHandlerMock.Object)
            .AddSingleton(getLinesByHeaderIdHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(updateLineHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.GetWorkOrderHeaderByIdWithLines(77);

        Assert.Same(expectedResponse, result);
        getHeaderByIdWithLinesHandlerMock.Verify(handler => handler.Handle(77), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var expectedException = new InvalidOperationException("header with lines read failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByIdWithLines(77))
            .ThrowsAsync(expectedException);

        var handler = new GetWorkOrderHeaderByIdWithLinesHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(77);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
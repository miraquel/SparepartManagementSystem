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
using SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class GetWorkOrderHeaderByParamsPagedListHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_ReturnsMappedPagedHeaders()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var parameters = new Dictionary<string, string>
        {
            ["Name"] = "pump"
        };
        var pagedResult = new PagedList<WorkOrderHeader>(
        [
            new WorkOrderHeader
            {
                WorkOrderHeaderId = 30,
                AGSEAMWOID = "WO-30",
                HeaderTitle = "Inspect pump"
            }
        ], 1, 10, 1);

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByParamsPagedList(1, 10, parameters))
            .ReturnsAsync(pagedResult);

        var handler = new GetWorkOrderHeaderByParamsPagedListHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(1, 10, parameters);

        Assert.True(result.Success);
        Assert.Equal("Work Order Headers retrieved successfully", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data!.PageNumber);
        Assert.Equal(10, result.Data.PageSize);
        Assert.Equal(1, result.Data.TotalCount);
        Assert.Single(result.Data.Items);
        Assert.Contains(result.Data.Items, header => header.WorkOrderHeaderId == 30 && header.HeaderTitle == "Inspect pump");
        Assert.Null(result.Error);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetWorkOrderHeaderByParamsPagedList_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getItemRequisitionByIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById.IGetItemRequisitionByIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByWorkOrderLineIdHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId.IGetItemRequisitionByWorkOrderLineIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByParamsHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByParams.IGetItemRequisitionByParamsHandler>(MockBehavior.Strict);
        var deleteItemRequisitionHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.DeleteItemRequisition.IDeleteItemRequisitionHandler>(MockBehavior.Strict);
        var getHeaderByIdHandlerMock = new Mock<IGetWorkOrderHeaderByIdHandler>(MockBehavior.Strict);
        var getHeaderByIdWithLinesHandlerMock = new Mock<IGetWorkOrderHeaderByIdWithLinesHandler>(MockBehavior.Strict);
        var getHeaderByParamsPagedListHandlerMock = new Mock<IGetWorkOrderHeaderByParamsPagedListHandler>(MockBehavior.Strict);
        var getLineByIdHandlerMock = new Mock<IGetWorkOrderLineByIdHandler>(MockBehavior.Strict);
        var getLinesByHeaderIdHandlerMock = new Mock<IGetWorkOrderLineByWorkOrderHeaderIdHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateLineHandlerMock = new Mock<IUpdateWorkOrderLineHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var parameters = new Dictionary<string, string>
        {
            ["Name"] = "pump"
        };
        var expectedResponse = new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
        {
            Success = true,
            Data = new PagedListDto<WorkOrderHeaderDto>(
            [
                new WorkOrderHeaderDto
                {
                    WorkOrderHeaderId = 30,
                    HeaderTitle = "Inspect pump"
                }
            ], 1, 10, 1)
        };

        getHeaderByParamsPagedListHandlerMock.Setup(handler => handler.Handle(1, 10, parameters))
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

        var result = await service.GetWorkOrderHeaderByParamsPagedList(1, 10, parameters);

        Assert.Same(expectedResponse, result);
        getHeaderByParamsPagedListHandlerMock.Verify(handler => handler.Handle(1, 10, parameters), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var parameters = new Dictionary<string, string>
        {
            ["Name"] = "pump"
        };
        var expectedException = new InvalidOperationException("paged filtered header read failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByParamsPagedList(1, 10, parameters))
            .ThrowsAsync(expectedException);

        var handler = new GetWorkOrderHeaderByParamsPagedListHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(1, 10, parameters);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
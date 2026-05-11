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
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByParams;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId;
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

public class GetItemRequisitionByParamsHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_ReturnsMappedItemRequisitions()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var parameters = new Dictionary<string, string>
        {
            ["ItemId"] = "ITEM-101"
        };
        var itemRequisitions = new[]
        {
            new ItemRequisition
            {
                ItemRequisitionId = 101,
                WorkOrderLineId = 77,
                ItemId = "ITEM-101",
                ItemName = "Bearing",
                Quantity = 2,
                RequestQuantity = 1
            },
            new ItemRequisition
            {
                ItemRequisitionId = 103,
                WorkOrderLineId = 88,
                ItemId = "ITEM-101",
                ItemName = "Bearing spare",
                Quantity = 6,
                RequestQuantity = 4
            }
        };

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByParams(parameters))
            .ReturnsAsync(itemRequisitions);

        var handler = new GetItemRequisitionByParamsHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(parameters);

        Assert.True(result.Success);
        Assert.Equal("Item Requisitions retrieved successfully", result.Message);
        Assert.NotNull(result.Data);
        var data = result.Data!.ToList();
        Assert.Equal(2, data.Count);
        Assert.Equal(101, data[0].ItemRequisitionId);
        Assert.Equal("Bearing spare", data[1].ItemName);
        Assert.Null(result.Error);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetItemRequisitionByParams_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getItemRequisitionByIdHandlerMock = new Mock<IGetItemRequisitionByIdHandler>(MockBehavior.Strict);
        var getItemRequisitionByParamsHandlerMock = new Mock<IGetItemRequisitionByParamsHandler>(MockBehavior.Strict);
        var getItemRequisitionByWorkOrderLineIdHandlerMock = new Mock<IGetItemRequisitionByWorkOrderLineIdHandler>(MockBehavior.Strict);
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
            ["ItemId"] = "ITEM-101"
        };
        var expectedResponse = new ServiceResponse<IEnumerable<ItemRequisitionDto>>
        {
            Success = true,
            Data =
            [
                new ItemRequisitionDto
                {
                    ItemRequisitionId = 101,
                    ItemId = "ITEM-101",
                    ItemName = "Bearing"
                }
            ]
        };

        getItemRequisitionByParamsHandlerMock.Setup(handler => handler.Handle(parameters))
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
            .AddSingleton(getItemRequisitionByParamsHandlerMock.Object)
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

        var result = await service.GetItemRequisitionByParams(parameters);

        Assert.Same(expectedResponse, result);
        getItemRequisitionByParamsHandlerMock.Verify(handler => handler.Handle(parameters), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var parameters = new Dictionary<string, string>
        {
            ["ItemId"] = "ITEM-101"
        };
        var expectedException = new InvalidOperationException("item requisition filtered read failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetByParams(parameters))
            .ThrowsAsync(expectedException);

        var handler = new GetItemRequisitionByParamsHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(parameters);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
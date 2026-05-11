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

public class GetItemRequisitionByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_ReturnsMappedItemRequisition()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var itemRequisition = new ItemRequisition
        {
            ItemRequisitionId = 77,
            WorkOrderLineId = 8,
            ItemId = "ITEM-77",
            ItemName = "Bearing",
            RequiredDate = new DateTime(2026, 5, 11),
            Quantity = 4,
            RequestQuantity = 2,
            InventLocationId = "MAIN",
            WMSLocationId = "A-01",
            JournalId = "JRNL-77",
            IsSubmitted = true
        };

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(77, false))
            .ReturnsAsync(itemRequisition);

        var handler = new GetItemRequisitionByIdHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(77);

        Assert.True(result.Success);
        Assert.Equal("Item Requisition retrieved successfully", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(77, result.Data!.ItemRequisitionId);
        Assert.Equal("ITEM-77", result.Data.ItemId);
        Assert.Equal("Bearing", result.Data.ItemName);
        Assert.Null(result.Error);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetItemRequisitionById_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
        var getItemRequisitionByIdHandlerMock = new Mock<IGetItemRequisitionByIdHandler>(MockBehavior.Strict);
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
        var expectedResponse = new ServiceResponse<ItemRequisitionDto>
        {
            Success = true,
            Data = new ItemRequisitionDto
            {
                ItemRequisitionId = 77,
                ItemId = "ITEM-77",
                ItemName = "Bearing"
            }
        };

        getItemRequisitionByIdHandlerMock.Setup(handler => handler.Handle(77))
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

        var result = await service.GetItemRequisitionById(77);

        Assert.Same(expectedResponse, result);
        getItemRequisitionByIdHandlerMock.Verify(handler => handler.Handle(77), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var expectedException = new InvalidOperationException("item requisition read failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(77, false))
            .ThrowsAsync(expectedException);

        var handler = new GetItemRequisitionByIdHandler(mapper, unitOfWorkMock.Object);

        var result = await handler.Handle(77);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
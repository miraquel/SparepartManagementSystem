using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteItemRequisition;
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

public class DeleteItemRequisitionHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_DeletesItemRequisitionAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.Delete(88))
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new DeleteItemRequisitionHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.True(result.Success);
        Assert.Equal("Item Requisition deleted successfully", result.Message);
        repositoryMock.Verify(repository => repository.Delete(88), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task DeleteItemRequisition_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteItemRequisitionHandlerMock = new Mock<IDeleteItemRequisitionHandler>(MockBehavior.Strict);
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
        var expectedResponse = new ServiceResponse
        {
            Success = true,
            Message = "Item Requisition deleted successfully"
        };

        deleteItemRequisitionHandlerMock.Setup(handler => handler.Handle(88))
            .ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(addLineHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(deleteItemRequisitionHandlerMock.Object)
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

        var result = await service.DeleteItemRequisition(88);

        Assert.Same(expectedResponse, result);
        deleteItemRequisitionHandlerMock.Verify(handler => handler.Handle(88), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);
        var expectedException = new InvalidOperationException("item requisition delete failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.Delete(88))
            .ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new DeleteItemRequisitionHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
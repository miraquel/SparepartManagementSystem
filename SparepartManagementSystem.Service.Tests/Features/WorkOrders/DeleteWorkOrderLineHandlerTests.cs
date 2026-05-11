using Microsoft.Extensions.DependencyInjection;
using Moq;
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

public class DeleteWorkOrderLineHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_DeletesLineAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var workOrderLineRepositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var itemRequisitionRepositoryMock = new Mock<IItemRequisitionRepository>(MockBehavior.Strict);

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(workOrderLineRepositoryMock.Object);
        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.ItemRequisitionRepository)
            .Returns(itemRequisitionRepositoryMock.Object);
        workOrderLineRepositoryMock.Setup(repository => repository.Delete(88))
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderLineHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.True(result.Success);
        Assert.Equal("Work Order Line deleted successfully", result.Message);
        workOrderLineRepositoryMock.Verify(repository => repository.Delete(88), Times.Once);
        itemRequisitionRepositoryMock.Verify(repository => repository.Delete(It.IsAny<int>()), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task DeleteWorkOrderLine_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateLineHandlerMock = new Mock<IUpdateWorkOrderLineHandler>(MockBehavior.Strict);
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
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var expectedResponse = new ServiceResponse
        {
            Success = true,
            Message = "Work Order Line deleted successfully"
        };

        deleteLineHandlerMock.Setup(handler => handler.Handle(88))
            .ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(addLineHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(updateLineHandlerMock.Object)
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
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.DeleteWorkOrderLine(88);

        Assert.Same(expectedResponse, result);
        deleteLineHandlerMock.Verify(handler => handler.Handle(88), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var workOrderLineRepositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var expectedException = new InvalidOperationException("line delete failed");

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(workOrderLineRepositoryMock.Object);
        workOrderLineRepositoryMock.Setup(repository => repository.Delete(88))
            .ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderLineHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
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

public class UpdateWorkOrderHeaderHandlerTests
{
    [Fact]
    public async Task Handle_WhenRecordHasChanges_UpdatesHeaderAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderHeader
        {
            WorkOrderHeaderId = 44,
            Name = "WO-OLD",
            HeaderTitle = "Old title",
            ModifiedDateTime = new DateTime(2026, 5, 10, 10, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();
        WorkOrderHeader? updatedRecord = null;
        EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null;

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(44, true))
            .ReturnsAsync(existingRecord);
        repositoryMock.Setup(repository => repository.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null))
            .Callback<WorkOrderHeader, EventHandler<BeforeUpdateEventArgs>?, EventHandler<AfterUpdateEventArgs>?>((entity, beforeUpdate, _) =>
            {
                updatedRecord = entity;
                onBeforeUpdate = beforeUpdate;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new UpdateWorkOrderHeaderHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderHeaderDto
        {
            WorkOrderHeaderId = 44,
            Name = "WO-NEW",
            HeaderTitle = "New title",
            EntityShutDown = NoYes.None,
            AGSEAMSuspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime
        };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Header updated successfully", result.Message);
        Assert.NotNull(updatedRecord);
        Assert.Equal("WO-NEW", updatedRecord!.Name);
        Assert.Equal("New title", updatedRecord.HeaderTitle);
        Assert.Same(repositoryEvents.OnBeforeUpdate, onBeforeUpdate);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNoChangesDetected_ReturnsSuccessWithoutUpdateOrCommit()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderHeader
        {
            WorkOrderHeaderId = 55,
            Name = "WO-SAME",
            HeaderTitle = "Same title",
            ModifiedDateTime = new DateTime(2026, 5, 10, 12, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(55, true))
            .ReturnsAsync(existingRecord);

        var handler = new UpdateWorkOrderHeaderHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderHeaderDto
        {
            WorkOrderHeaderId = 55,
            Name = "WO-SAME",
            HeaderTitle = "Same title",
            EntityShutDown = NoYes.None,
            AGSEAMSuspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime
        };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("No changes detected in Work Order Header", result.Message);
        repositoryMock.Verify(repository => repository.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRecordWasModifiedAfterDto_ReturnsConcurrencyError()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderHeader
        {
            WorkOrderHeaderId = 66,
            Name = "WO-CURRENT",
            HeaderTitle = "Current title",
            ModifiedDateTime = new DateTime(2026, 5, 10, 13, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderHeaderRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(66, true))
            .ReturnsAsync(existingRecord);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new UpdateWorkOrderHeaderHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderHeaderDto
        {
            WorkOrderHeaderId = 66,
            Name = "WO-STALE",
            HeaderTitle = "Stale title",
            EntityShutDown = NoYes.None,
            AGSEAMSuspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime.AddMinutes(-1)
        };

        var result = await handler.Handle(dto);

        Assert.False(result.Success);
        Assert.Equal(nameof(Exception), result.Error);
        Assert.Contains(result.ErrorMessages ?? [], message => message.Contains("Work Order Header has been modified by another user"));
        repositoryMock.Verify(repository => repository.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkOrderHeader_WhenCalled_DelegatesToHandler()
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
            Message = "Work Order Header updated successfully"
        };
        var dto = new WorkOrderHeaderDto
        {
            WorkOrderHeaderId = 77,
            Name = "WO-UPDATE"
        };

        updateHeaderHandlerMock.Setup(handler => handler.Handle(dto))
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

        var result = await service.UpdateWorkOrderHeader(dto);

        Assert.Same(expectedResponse, result);
        updateHeaderHandlerMock.Verify(handler => handler.Handle(dto), Times.Once);
    }
}
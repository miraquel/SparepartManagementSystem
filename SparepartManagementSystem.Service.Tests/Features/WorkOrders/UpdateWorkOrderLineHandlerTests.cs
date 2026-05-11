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

public class UpdateWorkOrderLineHandlerTests
{
    [Fact]
    public async Task Handle_WhenRecordHasChanges_UpdatesLineAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderLine
        {
            WorkOrderLineId = 44,
            WorkOrderHeaderId = 12,
            Line = 3,
            LineTitle = "Old title",
            EntityId = "PUMP-01",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-OLD",
            WorkOrderStatus = string.Empty,
            Suspend = NoYes.None,
            ModifiedDateTime = new DateTime(2026, 5, 10, 10, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();
        WorkOrderLine? updatedRecord = null;
        EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null;

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(44, true))
            .ReturnsAsync(existingRecord);
        repositoryMock.Setup(repository => repository.Update(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null))
            .Callback<WorkOrderLine, EventHandler<BeforeUpdateEventArgs>?, EventHandler<AfterUpdateEventArgs>?>((entity, beforeUpdate, _) =>
            {
                updatedRecord = entity;
                onBeforeUpdate = beforeUpdate;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);

        var handler = new UpdateWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderLineDto
        {
            WorkOrderLineId = 44,
            WorkOrderHeaderId = 12,
            Line = 3,
            LineTitle = "New title",
            EntityId = "PUMP-01",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-NEW",
            Suspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime
        };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Line updated successfully", result.Message);
        Assert.NotNull(updatedRecord);
        Assert.Equal("New title", updatedRecord!.LineTitle);
        Assert.Equal("TASK-NEW", updatedRecord.TaskId);
        Assert.Same(repositoryEvents.OnBeforeUpdate, onBeforeUpdate);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNoChangesDetected_ReturnsSuccessWithoutUpdateOrCommit()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderLine
        {
            WorkOrderLineId = 55,
            WorkOrderHeaderId = 15,
            Line = 5,
            LineTitle = "Same title",
            EntityId = "PUMP-02",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-SAME",
            PlanningStartDate = new DateTime(2026, 5, 20, 8, 0, 0, DateTimeKind.Utc),
            PlanningEndDate = new DateTime(2026, 5, 20, 12, 0, 0, DateTimeKind.Utc),
            WorkOrderStatus = string.Empty,
            Suspend = NoYes.None,
            ModifiedDateTime = new DateTime(2026, 5, 10, 12, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(55, true))
            .ReturnsAsync(existingRecord);

        var handler = new UpdateWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderLineDto
        {
            WorkOrderLineId = 55,
            WorkOrderHeaderId = 15,
            Line = 5,
            LineTitle = "Same title",
            EntityId = "PUMP-02",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-SAME",
            PlanningStartDate = existingRecord.PlanningStartDate,
            PlanningEndDate = existingRecord.PlanningEndDate,
            Suspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime
        };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("No changes detected in Work Order Line", result.Message);
        repositoryMock.Verify(repository => repository.Update(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRecordWasModifiedAfterDto_ReturnsConcurrencyError()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto
        {
            Username = "tester"
        });
        var existingRecord = new WorkOrderLine
        {
            WorkOrderLineId = 66,
            WorkOrderHeaderId = 20,
            Line = 6,
            LineTitle = "Current title",
            EntityId = "PUMP-03",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-CURRENT",
            WorkOrderStatus = string.Empty,
            Suspend = NoYes.None,
            ModifiedDateTime = new DateTime(2026, 5, 10, 13, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(unitOfWork => unitOfWork.WorkOrderLineRepository)
            .Returns(repositoryMock.Object);
        repositoryMock.Setup(repository => repository.GetById(66, true))
            .ReturnsAsync(existingRecord);
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Rollback())
            .Returns(Task.CompletedTask);

        var handler = new UpdateWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderLineDto
        {
            WorkOrderLineId = 66,
            WorkOrderHeaderId = 20,
            Line = 6,
            LineTitle = "Stale title",
            EntityId = "PUMP-03",
            EntityShutdown = NoYes.None,
            TaskId = "TASK-STALE",
            Suspend = NoYes.None,
            ModifiedDateTime = existingRecord.ModifiedDateTime.AddMinutes(-1)
        };

        var result = await handler.Handle(dto);

        Assert.False(result.Success);
        Assert.Equal(nameof(Exception), result.Error);
        Assert.Contains(result.ErrorMessages ?? [], message => message.Contains("Work Order Line has been modified by another user"));
        repositoryMock.Verify(repository => repository.Update(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Rollback(), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkOrderLine_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var getAllPagedListHandlerMock = new Mock<SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList.IGetAllWorkOrderHeaderPagedListHandler>(MockBehavior.Strict);
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
            Message = "Work Order Line updated successfully"
        };
        var dto = new WorkOrderLineDto
        {
            WorkOrderLineId = 77,
            WorkOrderHeaderId = 22,
            Line = 7,
            LineTitle = "Update title"
        };

        updateLineHandlerMock.Setup(handler => handler.Handle(dto))
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

        var result = await service.UpdateWorkOrderLine(dto);

        Assert.Same(expectedResponse, result);
        updateLineHandlerMock.Verify(handler => handler.Handle(dto), Times.Once);
    }
}
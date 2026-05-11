# Work Orders Update Header Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders migration by moving `UpdateWorkOrderHeader` into a Work Orders-owned seam and reducing the live write-side ownership of the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `UpdateWorkOrderHeader` path moves into a dedicated feature handler under Work Orders. The slice must preserve the existing optimistic concurrency guard, the no-change short circuit, and the update/commit error handling path.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `UpdateWorkOrderHeader`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `UpdateWorkOrderHeader` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `UpdateWorkOrderHeader`, matching the existing header-create transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing another concrete Work Orders write path from the legacy service shell.
- **What the next slice should remove:** either `DeleteWorkOrderHeader` or the first Work Order Line write path, so Work Orders write ownership keeps moving out of the legacy shell.

## Implemented Scope

- [x] Add focused tests for the `UpdateWorkOrderHeader` handler seam.
- [x] Add `IUpdateWorkOrderHeaderHandler`.
- [x] Implement `UpdateWorkOrderHeaderHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.UpdateWorkOrderHeader` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `UpdateWorkOrderHeader` handler seam.
- Focused red-green validation for the live `UpdateWorkOrderHeader` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` still owns delete, query, line, and requisition paths.

### Task 1: Add Focused Tests For UpdateWorkOrderHeader Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderHeaderHandlerTests.cs`

- [ ] **Step 1: Write the failing tests for the new handler and delegation path**

```csharp
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
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
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
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
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

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.GetById(44, true)).ReturnsAsync(existingRecord);
        repositoryMock.Setup(x => x.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null))
            .Callback<WorkOrderHeader, EventHandler<BeforeUpdateEventArgs>?, EventHandler<AfterUpdateEventArgs>?>((entity, beforeUpdate, _) =>
            {
                updatedRecord = entity;
                onBeforeUpdate = beforeUpdate;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

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
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNoChangesDetected_ReturnsSuccessWithoutUpdateOrCommit()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var existingRecord = new WorkOrderHeader
        {
            WorkOrderHeaderId = 55,
            Name = "WO-SAME",
            HeaderTitle = "Same title",
            ModifiedDateTime = new DateTime(2026, 5, 10, 12, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.GetById(55, true)).ReturnsAsync(existingRecord);

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
        repositoryMock.Verify(x => x.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRecordWasModifiedAfterDto_ReturnsConcurrencyError()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var existingRecord = new WorkOrderHeader
        {
            WorkOrderHeaderId = 66,
            Name = "WO-CURRENT",
            HeaderTitle = "Current title",
            ModifiedDateTime = new DateTime(2026, 5, 10, 13, 0, 0, DateTimeKind.Utc)
        };
        existingRecord.AcceptChanges();

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.GetById(66, true)).ReturnsAsync(existingRecord);
        unitOfWorkMock.Setup(x => x.Rollback()).Returns(Task.CompletedTask);

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
        Assert.Equal("Exception", result.Error);
        Assert.Contains(result.ErrorMessages ?? [], message => message.Contains("Work Order Header has been modified by another user"));
        repositoryMock.Verify(x => x.Update(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<BeforeUpdateEventArgs>>(), null), Times.Never);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkOrderHeader_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var expectedResponse = new ServiceResponse { Success = true, Message = "Work Order Header updated successfully" };
        var dto = new WorkOrderHeaderDto { WorkOrderHeaderId = 77, Name = "WO-UPDATE" };

        updateHeaderHandlerMock.Setup(x => x.Handle(dto)).ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.UpdateWorkOrderHeader(dto);

        Assert.Same(expectedResponse, result);
        updateHeaderHandlerMock.Verify(x => x.Handle(dto), Times.Once);
    }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~UpdateWorkOrderHeaderHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement UpdateWorkOrderHeader Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/UpdateWorkOrderHeader/IUpdateWorkOrderHeaderHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/UpdateWorkOrderHeader/UpdateWorkOrderHeaderHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`

- [ ] **Step 1: Add the handler contract**

```csharp
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;

public interface IUpdateWorkOrderHeaderHandler
{
    Task<ServiceResponse> Handle(WorkOrderHeaderDto dto);
}
```

- [ ] **Step 2: Implement the minimal handler**

```csharp
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;

public class UpdateWorkOrderHeaderHandler : IUpdateWorkOrderHeaderHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<UpdateWorkOrderHeaderHandler>();

    public UpdateWorkOrderHeaderHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderHeaderDto dto)
    {
        try
        {
            var record = await _unitOfWork.WorkOrderHeaderRepository.GetById(dto.WorkOrderHeaderId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Work Order Header has been modified by another user, please refresh and try again");
            }

            record.UpdateProperties(_mapper.MapToWorkOrderHeader(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Work Order Header"
                };
            }

            await _unitOfWork.WorkOrderHeaderRepository.Update(record, _repositoryEvents.OnBeforeUpdate);
            await _unitOfWork.Commit();

            _logger.Information("Work Order Header updated successfully, Work Order Header Id: {WorkOrderHeaderId}", dto.WorkOrderHeaderId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header updated successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = [ex.Message],
                Success = false
            };
        }
    }
}
```

- [ ] **Step 3: Register the new handler**

```csharp
services.AddScoped<IUpdateWorkOrderHeaderHandler, UpdateWorkOrderHeaderHandler>();
```

- [ ] **Step 4: Run the focused tests and make them pass**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~UpdateWorkOrderHeaderHandlerTests`
Expected: PASS

### Task 3: Delegate WorkOrderService.UpdateWorkOrderHeader

**Files:**
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderWithLinesHandlerTests.cs`

- [ ] **Step 1: Extend the constructor and delegate the live method**

```csharp
private readonly IUpdateWorkOrderHeaderHandler _updateWorkOrderHeaderHandler;

public WorkOrderService(
    MapperlyMapper mapper,
    IUnitOfWork unitOfWork,
    RepositoryEvents repositoryEvents,
    IAddWorkOrderHeaderHandler addWorkOrderHeaderHandler,
    IAddWorkOrderHeaderWithLinesHandler addWorkOrderHeaderWithLinesHandler,
    IUpdateWorkOrderHeaderHandler updateWorkOrderHeaderHandler)
{
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _repositoryEvents = repositoryEvents;
    _addWorkOrderHeaderHandler = addWorkOrderHeaderHandler;
    _addWorkOrderHeaderWithLinesHandler = addWorkOrderHeaderWithLinesHandler;
    _updateWorkOrderHeaderHandler = updateWorkOrderHeaderHandler;
}

public Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
{
    return _updateWorkOrderHeaderHandler.Handle(dto);
}
```

- [ ] **Step 2: Adjust existing Work Orders delegation tests only for the new constructor dependency**

Add a strict `Mock<IUpdateWorkOrderHeaderHandler>` where needed so existing constructor-based tests still build.

- [ ] **Step 3: Run focused Work Orders handler tests together**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter "FullyQualifiedName~AddWorkOrderHeaderHandlerTests|FullyQualifiedName~AddWorkOrderHeaderWithLinesHandlerTests|FullyQualifiedName~UpdateWorkOrderHeaderHandlerTests"`
Expected: PASS

### Task 4: Final Verification

**Files:**
- Modify: `docs/superpowers/plans/2026-05-10-work-orders-update-header-slice.md`

- [ ] **Step 1: Run touched-file diagnostics**

Use the editor diagnostics tool on the touched files.
Expected: no new relevant errors.

- [ ] **Step 2: Run a solution build**

Run: `dotnet build .\SparepartManagementSystem.sln --no-restore`
Expected: PASS

- [ ] **Step 3: Mark the plan as implemented**

Add a short status block near the top summarizing that the slice was implemented and verified with focused tests plus solution build.
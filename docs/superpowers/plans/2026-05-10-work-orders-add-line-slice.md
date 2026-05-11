# Work Orders Add Line Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Order Line write-path migration by moving `AddWorkOrderLine` into a Work Orders-owned seam and reducing legacy write ownership in `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `AddWorkOrderLine` path moves into a dedicated feature handler under Work Orders. The slice preserves the current add, last-inserted-id lookup, commit, and rollback-based error behavior while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `AddWorkOrderLine`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `AddWorkOrderLine` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `AddWorkOrderLine`, matching the existing header-create transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the next smallest remaining Work Order Line write path from the legacy service shell.
- **What the next slice should remove:** `UpdateWorkOrderLine`, so the remaining line write ownership moves out of the legacy shell before line query extraction.

## Implemented Scope

- [x] Add focused tests for the `AddWorkOrderLine` handler seam.
- [x] Add `IAddWorkOrderLineHandler`.
- [x] Implement `AddWorkOrderLineHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.AddWorkOrderLine` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `AddWorkOrderLine` handler seam.
- Focused red-green validation for the live `AddWorkOrderLine` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` no longer owns the first Work Order Line create path, but it still owns `UpdateWorkOrderLine`, line queries, header queries, and requisition paths.

### Task 1: Add Focused Tests For AddWorkOrderLine Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderLineHandlerTests.cs`

- [ ] **Step 1: Write the failing tests for the new handler and delegation path**

```csharp
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Domain;
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
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class AddWorkOrderLineHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_AddsLineAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        WorkOrderLine? addedLine = null;
        EventHandler<AddEventArgs>? onBeforeAdd = null;

        unitOfWorkMock.SetupGet(x => x.WorkOrderLineRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.Add(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Callback<WorkOrderLine, EventHandler<AddEventArgs>?, EventHandler<AddEventArgs>?>((entity, beforeAdd, _) =>
            {
                addedLine = entity;
                onBeforeAdd = beforeAdd;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(x => x.GetLastInsertedId()).ReturnsAsync(77);
        unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

        var handler = new AddWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderLineDto { WorkOrderHeaderId = 12, Line = 3, LineTitle = "Install gasket" };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Line added successfully", result.Message);
        Assert.NotNull(addedLine);
        Assert.Equal(dto.WorkOrderHeaderId, addedLine!.WorkOrderHeaderId);
        Assert.Equal(dto.LineTitle, addedLine.LineTitle);
        Assert.Same(repositoryEvents.OnBeforeAdd, onBeforeAdd);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task AddWorkOrderLine_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var addLineHandlerMock = new Mock<IAddWorkOrderLineHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var expectedResponse = new ServiceResponse { Success = true, Message = "Work Order Line added successfully" };
        var dto = new WorkOrderLineDto { WorkOrderHeaderId = 12, Line = 3, LineTitle = "Install gasket" };

        addLineHandlerMock.Setup(x => x.Handle(dto)).ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(addLineHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(deleteLineHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.AddWorkOrderLine(dto);

        Assert.Same(expectedResponse, result);
        addLineHandlerMock.Verify(x => x.Handle(dto), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var dto = new WorkOrderLineDto { WorkOrderHeaderId = 12, Line = 3, LineTitle = "Install gasket" };
        var expectedException = new InvalidOperationException("line add failed");

        unitOfWorkMock.SetupGet(x => x.WorkOrderLineRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.Add(It.IsAny<WorkOrderLine>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(x => x.Rollback()).Returns(Task.CompletedTask);

        var handler = new AddWorkOrderLineHandler(mapper, unitOfWorkMock.Object, repositoryEvents);

        var result = await handler.Handle(dto);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~AddWorkOrderLineHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement AddWorkOrderLine Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/AddWorkOrderLine/IAddWorkOrderLineHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/AddWorkOrderLine/AddWorkOrderLineHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`

- [ ] **Step 1: Add the handler contract**

```csharp
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;

public interface IAddWorkOrderLineHandler
{
    Task<ServiceResponse> Handle(WorkOrderLineDto dto);
}
```

- [ ] **Step 2: Implement the minimal handler**

```csharp
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;

public class AddWorkOrderLineHandler : IAddWorkOrderLineHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<AddWorkOrderLineHandler>();

    public AddWorkOrderLineHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderLineDto dto)
    {
        try
        {
            var workOrderLineAdd = _mapper.MapToWorkOrderLine(dto);
            await _unitOfWork.WorkOrderLineRepository.Add(workOrderLineAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            await _unitOfWork.Commit();

            _logger.Information("Work Order Line added successfully, Work Order Line Id: {WorkOrderLineId}", lastInsertedId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line added successfully"
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

- [ ] **Step 3: Register and delegate the handler**

Add the handler to DI and change `WorkOrderService.AddWorkOrderLine` to delegate to `IAddWorkOrderLineHandler`.

### Task 3: Validate The Slice

**Files:**
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderWithLinesHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderLineHandlerTests.cs`

- [ ] **Step 1: Update existing delegation tests only as required for the expanded constructor**
- [ ] **Step 2: Run the focused add-line tests**
- [ ] **Step 3: Run the combined focused Work Orders handler suite**
- [ ] **Step 4: Check touched-file diagnostics**
- [ ] **Step 5: Run a no-restore solution build**
# Work Orders Delete Line Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Start the Work Order Line write-path migration by moving `DeleteWorkOrderLine` into a Work Orders-owned seam and correcting the current wrong-repository delete behavior.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `DeleteWorkOrderLine` path moves into a dedicated feature handler under Work Orders. The slice must correct the repository target from `ItemRequisitionRepository` to `WorkOrderLineRepository` while preserving the delete, commit, and rollback-based error shape.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `DeleteWorkOrderLine`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `DeleteWorkOrderLine` behavior and currently points at the wrong repository.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `DeleteWorkOrderLine`, and the live path is corrected to delete through `IWorkOrderLineRepository`.
- **Why that is acceptable:** It preserves the external contract, removes the first Work Orders line write path from the legacy shell, and fixes a concrete behavior defect at the same time.
- **What the next slice should remove:** either `AddWorkOrderLine` or `UpdateWorkOrderLine`, depending on whether the next goal is smallest seam or full line-write parity.

## Implemented Scope

- [x] Add focused tests for the `DeleteWorkOrderLine` handler seam.
- [x] Add `IDeleteWorkOrderLineHandler`.
- [x] Implement `DeleteWorkOrderLineHandler` using `WorkOrderLineRepository.Delete`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.DeleteWorkOrderLine` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `DeleteWorkOrderLine` handler seam.
- Focused red-green validation for the live `DeleteWorkOrderLine` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` still owns the remaining line create/update/query paths and requisition paths.

### Task 1: Add Focused Tests For DeleteWorkOrderLine Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderLineHandlerTests.cs`

- [ ] **Step 1: Write the failing tests for the new handler and delegation path**

```csharp
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
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

        unitOfWorkMock.SetupGet(x => x.WorkOrderLineRepository).Returns(workOrderLineRepositoryMock.Object);
        unitOfWorkMock.SetupGet(x => x.ItemRequisitionRepository).Returns(itemRequisitionRepositoryMock.Object);
        workOrderLineRepositoryMock.Setup(x => x.Delete(88)).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderLineHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.True(result.Success);
        Assert.Equal("Work Order Line deleted successfully", result.Message);
        workOrderLineRepositoryMock.Verify(x => x.Delete(88), Times.Once);
        itemRequisitionRepositoryMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task DeleteWorkOrderLine_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteLineHandlerMock = new Mock<IDeleteWorkOrderLineHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var expectedResponse = new ServiceResponse { Success = true, Message = "Work Order Line deleted successfully" };

        deleteLineHandlerMock.Setup(x => x.Handle(88)).ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(deleteLineHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.DeleteWorkOrderLine(88);

        Assert.Same(expectedResponse, result);
        deleteLineHandlerMock.Verify(x => x.Handle(88), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var workOrderLineRepositoryMock = new Mock<IWorkOrderLineRepository>(MockBehavior.Strict);
        var expectedException = new InvalidOperationException("line delete failed");

        unitOfWorkMock.SetupGet(x => x.WorkOrderLineRepository).Returns(workOrderLineRepositoryMock.Object);
        workOrderLineRepositoryMock.Setup(x => x.Delete(88)).ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(x => x.Rollback()).Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderLineHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(88);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~DeleteWorkOrderLineHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement DeleteWorkOrderLine Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/DeleteWorkOrderLine/IDeleteWorkOrderLineHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/DeleteWorkOrderLine/DeleteWorkOrderLineHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`

- [ ] **Step 1: Add the handler contract**

```csharp
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;

public interface IDeleteWorkOrderLineHandler
{
    Task<ServiceResponse> Handle(int id);
}
```

- [ ] **Step 2: Implement the minimal handler**

```csharp
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;

public class DeleteWorkOrderLineHandler : IDeleteWorkOrderLineHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<DeleteWorkOrderLineHandler>();

    public DeleteWorkOrderLineHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(int id)
    {
        try
        {
            await _unitOfWork.WorkOrderLineRepository.Delete(id);
            await _unitOfWork.Commit();

            _logger.Information("Work Order Line deleted successfully, Work Order Line Id: {WorkOrderLineId}", id);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line deleted successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}
```

- [ ] **Step 3: Register the new handler**

```csharp
services.AddScoped<IDeleteWorkOrderLineHandler, DeleteWorkOrderLineHandler>();
```

- [ ] **Step 4: Run the focused tests and make them pass**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~DeleteWorkOrderLineHandlerTests`
Expected: PASS

### Task 3: Delegate WorkOrderService.DeleteWorkOrderLine

**Files:**
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderWithLinesHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderHeaderHandlerTests.cs`

- [ ] **Step 1: Extend the constructor and delegate the live method**

```csharp
private readonly IDeleteWorkOrderLineHandler _deleteWorkOrderLineHandler;

public WorkOrderService(
    MapperlyMapper mapper,
    IUnitOfWork unitOfWork,
    RepositoryEvents repositoryEvents,
    IAddWorkOrderHeaderHandler addWorkOrderHeaderHandler,
    IAddWorkOrderHeaderWithLinesHandler addWorkOrderHeaderWithLinesHandler,
    IDeleteWorkOrderHeaderHandler deleteWorkOrderHeaderHandler,
    IUpdateWorkOrderHeaderHandler updateWorkOrderHeaderHandler,
    IDeleteWorkOrderLineHandler deleteWorkOrderLineHandler)
{
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _repositoryEvents = repositoryEvents;
    _addWorkOrderHeaderHandler = addWorkOrderHeaderHandler;
    _addWorkOrderHeaderWithLinesHandler = addWorkOrderHeaderWithLinesHandler;
    _deleteWorkOrderHeaderHandler = deleteWorkOrderHeaderHandler;
    _updateWorkOrderHeaderHandler = updateWorkOrderHeaderHandler;
    _deleteWorkOrderLineHandler = deleteWorkOrderLineHandler;
}

public Task<ServiceResponse> DeleteWorkOrderLine(int id)
{
    return _deleteWorkOrderLineHandler.Handle(id);
}
```

- [ ] **Step 2: Adjust existing Work Orders delegation tests only for the new constructor dependency**

Add a strict `Mock<IDeleteWorkOrderLineHandler>` where needed so existing constructor-based tests still build.

- [ ] **Step 3: Run focused Work Orders handler tests together**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter "FullyQualifiedName~AddWorkOrderHeaderHandlerTests|FullyQualifiedName~AddWorkOrderHeaderWithLinesHandlerTests|FullyQualifiedName~UpdateWorkOrderHeaderHandlerTests|FullyQualifiedName~DeleteWorkOrderHeaderHandlerTests|FullyQualifiedName~DeleteWorkOrderLineHandlerTests"`
Expected: PASS

### Task 4: Final Verification

**Files:**
- Modify: `docs/superpowers/plans/2026-05-10-work-orders-delete-line-slice.md`

- [ ] **Step 1: Run touched-file diagnostics**

Use the editor diagnostics tool on the touched files.
Expected: no new relevant errors.

- [ ] **Step 2: Run a solution build**

Run: `dotnet build .\SparepartManagementSystem.sln --no-restore`
Expected: PASS

- [ ] **Step 3: Mark the plan as implemented**

Add a short status block near the top summarizing that the slice was implemented and verified with focused tests plus solution build.
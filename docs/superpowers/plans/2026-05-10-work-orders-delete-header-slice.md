# Work Orders Delete Header Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders migration by moving `DeleteWorkOrderHeader` into a Work Orders-owned seam and reducing the live write-side ownership of the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `DeleteWorkOrderHeader` path moves into a dedicated feature handler under Work Orders. The slice must preserve the current delete, commit, and rollback-based error behavior while keeping `WorkOrderService` as a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `DeleteWorkOrderHeader`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `DeleteWorkOrderHeader` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `DeleteWorkOrderHeader`, matching the existing header write transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing another concrete Work Orders write path from the legacy service shell.
- **What the next slice should remove:** the first Work Order Line write path so Work Orders write ownership starts moving out of the legacy shell beyond header operations.

## Implemented Scope

- [x] Add focused tests for the `DeleteWorkOrderHeader` handler seam.
- [x] Add `IDeleteWorkOrderHeaderHandler`.
- [x] Implement `DeleteWorkOrderHeaderHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.DeleteWorkOrderHeader` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `DeleteWorkOrderHeader` handler seam.
- Focused red-green validation for the live `DeleteWorkOrderHeader` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` still owns query, line, and requisition paths.

### Task 1: Add Focused Tests For DeleteWorkOrderHeader Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderHeaderHandlerTests.cs`

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
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class DeleteWorkOrderHeaderHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_DeletesHeaderAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.Delete(55)).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderHeaderHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(55);

        Assert.True(result.Success);
        Assert.Equal("Work Order Header deleted successfully", result.Message);
        repositoryMock.Verify(x => x.Delete(55), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task DeleteWorkOrderHeader_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var updateHeaderHandlerMock = new Mock<IUpdateWorkOrderHeaderHandler>(MockBehavior.Strict);
        var deleteHeaderHandlerMock = new Mock<IDeleteWorkOrderHeaderHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var expectedResponse = new ServiceResponse { Success = true, Message = "Work Order Header deleted successfully" };

        deleteHeaderHandlerMock.Setup(x => x.Handle(55)).ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .AddSingleton(updateHeaderHandlerMock.Object)
            .AddSingleton(deleteHeaderHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.DeleteWorkOrderHeader(55);

        Assert.Same(expectedResponse, result);
        deleteHeaderHandlerMock.Verify(x => x.Handle(55), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndReturnsErrorResponse()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var expectedException = new InvalidOperationException("delete failed");

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.Delete(55)).ThrowsAsync(expectedException);
        unitOfWorkMock.Setup(x => x.Rollback()).Returns(Task.CompletedTask);

        var handler = new DeleteWorkOrderHeaderHandler(unitOfWorkMock.Object);

        var result = await handler.Handle(55);

        Assert.False(result.Success);
        Assert.Equal(nameof(InvalidOperationException), result.Error);
        Assert.Contains(expectedException.Message, result.ErrorMessages ?? []);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~DeleteWorkOrderHeaderHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement DeleteWorkOrderHeader Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/DeleteWorkOrderHeader/IDeleteWorkOrderHeaderHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/DeleteWorkOrderHeader/DeleteWorkOrderHeaderHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`

- [ ] **Step 1: Add the handler contract**

```csharp
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;

public interface IDeleteWorkOrderHeaderHandler
{
    Task<ServiceResponse> Handle(int id);
}
```

- [ ] **Step 2: Implement the minimal handler**

```csharp
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;

public class DeleteWorkOrderHeaderHandler : IDeleteWorkOrderHeaderHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<DeleteWorkOrderHeaderHandler>();

    public DeleteWorkOrderHeaderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(int id)
    {
        try
        {
            await _unitOfWork.WorkOrderHeaderRepository.Delete(id);
            await _unitOfWork.Commit();

            _logger.Information("Work Order Header deleted successfully, Work Order Header Id: {WorkOrderHeaderId}", id);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header deleted successfully"
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
services.AddScoped<IDeleteWorkOrderHeaderHandler, DeleteWorkOrderHeaderHandler>();
```

- [ ] **Step 4: Run the focused tests and make them pass**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~DeleteWorkOrderHeaderHandlerTests`
Expected: PASS

### Task 3: Delegate WorkOrderService.DeleteWorkOrderHeader

**Files:**
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderWithLinesHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderHeaderHandlerTests.cs`

- [ ] **Step 1: Extend the constructor and delegate the live method**

```csharp
private readonly IDeleteWorkOrderHeaderHandler _deleteWorkOrderHeaderHandler;

public WorkOrderService(
    MapperlyMapper mapper,
    IUnitOfWork unitOfWork,
    RepositoryEvents repositoryEvents,
    IAddWorkOrderHeaderHandler addWorkOrderHeaderHandler,
    IAddWorkOrderHeaderWithLinesHandler addWorkOrderHeaderWithLinesHandler,
    IUpdateWorkOrderHeaderHandler updateWorkOrderHeaderHandler,
    IDeleteWorkOrderHeaderHandler deleteWorkOrderHeaderHandler)
{
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _repositoryEvents = repositoryEvents;
    _addWorkOrderHeaderHandler = addWorkOrderHeaderHandler;
    _addWorkOrderHeaderWithLinesHandler = addWorkOrderHeaderWithLinesHandler;
    _updateWorkOrderHeaderHandler = updateWorkOrderHeaderHandler;
    _deleteWorkOrderHeaderHandler = deleteWorkOrderHeaderHandler;
}

public Task<ServiceResponse> DeleteWorkOrderHeader(int id)
{
    return _deleteWorkOrderHeaderHandler.Handle(id);
}
```

- [ ] **Step 2: Adjust existing Work Orders delegation tests only for the new constructor dependency**

Add a strict `Mock<IDeleteWorkOrderHeaderHandler>` where needed so existing constructor-based tests still build.

- [ ] **Step 3: Run focused Work Orders handler tests together**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter "FullyQualifiedName~AddWorkOrderHeaderHandlerTests|FullyQualifiedName~AddWorkOrderHeaderWithLinesHandlerTests|FullyQualifiedName~UpdateWorkOrderHeaderHandlerTests|FullyQualifiedName~DeleteWorkOrderHeaderHandlerTests"`
Expected: PASS

### Task 4: Final Verification

**Files:**
- Modify: `docs/superpowers/plans/2026-05-10-work-orders-delete-header-slice.md`

- [ ] **Step 1: Run touched-file diagnostics**

Use the editor diagnostics tool on the touched files.
Expected: no new relevant errors.

- [ ] **Step 2: Run a solution build**

Run: `dotnet build .\SparepartManagementSystem.sln --no-restore`
Expected: PASS

- [ ] **Step 3: Mark the plan as implemented**

Add a short status block near the top summarizing that the slice was implemented and verified with focused tests plus solution build.
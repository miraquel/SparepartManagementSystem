# Work Orders Add Header Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders migration by moving `AddWorkOrderHeader` into a Work Orders-owned seam and reducing the write-side ownership of the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` remain stable, but the live `AddWorkOrderHeader` path will move into a dedicated feature handler under Work Orders. After this slice, both header-create paths will be owned by feature code and `WorkOrderService` will shrink further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `AddWorkOrderHeader`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `AddWorkOrderHeader` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `AddWorkOrderHeader`, matching the existing `AddWorkOrderHeaderWithLines` transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing another concrete write path from the legacy service layer.
- **What the next slice should remove:** either `UpdateWorkOrderHeader` or the first Work Order Line write path, so Work Orders write ownership keeps moving out of the legacy shell.

## Implemented Scope

- [x] Add focused tests for the `AddWorkOrderHeader` handler seam.
- [x] Add `IAddWorkOrderHeaderHandler`.
- [x] Implement `AddWorkOrderHeaderHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.AddWorkOrderHeader` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust the earlier `AddWorkOrderHeaderWithLines` delegation test only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `AddWorkOrderHeader` handler seam.
- Focused red-green validation for the live `AddWorkOrderHeader` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` no longer owns either Work Order header-create path, but it still owns update, delete, query, line, and requisition paths.

### Task 1: Add Focused Tests For AddWorkOrderHeader Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`

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
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class AddWorkOrderHeaderHandlerTests
{
    [Fact]
    public async Task Handle_WhenRepositorySucceeds_AddsHeaderAndCommits()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var repositoryMock = new Mock<IWorkOrderHeaderRepository>(MockBehavior.Strict);
        var mapper = new MapperlyMapper();
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        WorkOrderHeader? addedHeader = null;
        EventHandler<AddEventArgs>? onBeforeAdd = null;

        unitOfWorkMock.SetupGet(x => x.WorkOrderHeaderRepository).Returns(repositoryMock.Object);
        repositoryMock.Setup(x => x.Add(It.IsAny<WorkOrderHeader>(), It.IsAny<EventHandler<AddEventArgs>>(), null))
            .Callback<WorkOrderHeader, EventHandler<AddEventArgs>?, EventHandler<AddEventArgs>?>((entity, beforeAdd, _) =>
            {
                addedHeader = entity;
                onBeforeAdd = beforeAdd;
            })
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(x => x.GetLastInsertedId()).ReturnsAsync(99);
        unitOfWorkMock.Setup(x => x.Commit()).Returns(Task.CompletedTask);

        var handler = new AddWorkOrderHeaderHandler(mapper, unitOfWorkMock.Object, repositoryEvents);
        var dto = new WorkOrderHeaderDto { Name = "WO-002", HeaderTitle = "Emergency work" };

        var result = await handler.Handle(dto);

        Assert.True(result.Success);
        Assert.Equal("Work Order Header added successfully", result.Message);
        Assert.NotNull(addedHeader);
        Assert.Equal(dto.Name, addedHeader!.Name);
        Assert.Same(repositoryEvents.OnBeforeAdd, onBeforeAdd);
        unitOfWorkMock.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public async Task AddWorkOrderHeader_WhenCalled_DelegatesToHandler()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addHeaderHandlerMock = new Mock<IAddWorkOrderHeaderHandler>(MockBehavior.Strict);
        var addHeaderWithLinesHandlerMock = new Mock<IAddWorkOrderHeaderWithLinesHandler>(MockBehavior.Strict);
        var repositoryEvents = new RepositoryEvents(new UserClaimDto { Username = "tester" });
        var expectedResponse = new ServiceResponse { Success = true, Message = "Work Order Header added successfully" };
        var dto = new WorkOrderHeaderDto { Name = "WO-002" };

        addHeaderHandlerMock.Setup(x => x.Handle(dto)).ReturnsAsync(expectedResponse);

        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new MapperlyMapper())
            .AddSingleton(unitOfWorkMock.Object)
            .AddSingleton(repositoryEvents)
            .AddSingleton(addHeaderHandlerMock.Object)
            .AddSingleton(addHeaderWithLinesHandlerMock.Object)
            .BuildServiceProvider();

        var service = ActivatorUtilities.CreateInstance<WorkOrderService>(serviceProvider);

        var result = await service.AddWorkOrderHeader(dto);

        Assert.Same(expectedResponse, result);
        addHeaderHandlerMock.Verify(x => x.Handle(dto), Times.Once);
    }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~AddWorkOrderHeaderHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement AddWorkOrderHeader Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/AddWorkOrderHeader/IAddWorkOrderHeaderHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/AddWorkOrderHeader/AddWorkOrderHeaderHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`

- [ ] **Step 1: Add the handler contract**

```csharp
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;

public interface IAddWorkOrderHeaderHandler
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

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;

public class AddWorkOrderHeaderHandler : IAddWorkOrderHeaderHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<AddWorkOrderHeaderHandler>();

    public AddWorkOrderHeaderHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderHeaderDto dto)
    {
        try
        {
            var workOrderHeaderAdd = _mapper.MapToWorkOrderHeader(dto);
            await _unitOfWork.WorkOrderHeaderRepository.Add(workOrderHeaderAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            await _unitOfWork.Commit();

            _logger.Information("Work Order Header added successfully, Work Order Header Id: {WorkOrderHeaderId}", lastInsertedId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header added successfully"
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

- [ ] **Step 3: Register the handler in DI**

```csharp
services.AddScoped<IAddWorkOrderHeaderHandler, AddWorkOrderHeaderHandler>();
```

- [ ] **Step 4: Run the focused tests and confirm the handler tests pass**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~AddWorkOrderHeaderHandlerTests`
Expected: PASS.

### Task 3: Delegate The Live AddWorkOrderHeader Path

**Files:**
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`
- Test: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`

- [ ] **Step 1: Inject the new handler and delegate only AddWorkOrderHeader**

```csharp
private readonly IAddWorkOrderHeaderHandler _addWorkOrderHeaderHandler;

public WorkOrderService(
    MapperlyMapper mapper,
    IUnitOfWork unitOfWork,
    RepositoryEvents repositoryEvents,
    IAddWorkOrderHeaderHandler addWorkOrderHeaderHandler,
    IAddWorkOrderHeaderWithLinesHandler addWorkOrderHeaderWithLinesHandler)
{
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _repositoryEvents = repositoryEvents;
    _addWorkOrderHeaderHandler = addWorkOrderHeaderHandler;
    _addWorkOrderHeaderWithLinesHandler = addWorkOrderHeaderWithLinesHandler;
}

public Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto)
{
    return _addWorkOrderHeaderHandler.Handle(dto);
}
```

- [ ] **Step 2: Run the focused tests for the new slice and confirm they pass**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter "AddWorkOrderHeaderHandlerTests|AddWorkOrderHeaderWithLinesHandlerTests"`
Expected: PASS.

### Task 4: Verify Slice Integration

**Files:**
- Modify: none unless build requires a contract-safe adjustment

- [ ] **Step 1: Build the solution without restore**

Run: `dotnet build .\SparepartManagementSystem.sln --no-restore`
Expected: BUILD SUCCEEDED.

- [ ] **Step 2: Record the remaining compatibility shell**

```text
After this slice, WorkOrderService still owns update, delete, query, line, and requisition paths, but both header-create paths are delegated to feature-owned handlers.
```

## Self-Review

- Spec coverage: this slice removes one more concrete write path from `WorkOrderService` while preserving the public contract.
- Transitional posture: explicit and documented.
- Type consistency: the new handler mirrors the existing `AddWorkOrderHeaderWithLines` transition pattern so DI and service construction remain predictable.
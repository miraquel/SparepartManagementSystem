# Work Orders Update Line Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Order Line write-path migration by moving `UpdateWorkOrderLine` into a Work Orders-owned seam and shrinking the legacy `WorkOrderService` write surface further.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `UpdateWorkOrderLine` path moves into a dedicated feature handler under Work Orders. The slice preserves the existing concurrency check, no-change short circuit, update, commit, and rollback-based error behavior while reducing legacy write ownership.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `UpdateWorkOrderLine`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `UpdateWorkOrderLine` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `UpdateWorkOrderLine`, matching the existing header-update transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the last remaining Work Order Line write mutation with branching logic from the legacy service shell.
- **What the next slice should remove:** line queries or the first header query path, depending on whether the next goal is full line feature ownership or query-side symmetry.

## Implemented Scope

- [x] Add focused tests for the `UpdateWorkOrderLine` handler seam.
- [x] Add `IUpdateWorkOrderLineHandler`.
- [x] Implement `UpdateWorkOrderLineHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.UpdateWorkOrderLine` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `UpdateWorkOrderLine` handler seam.
- Focused red-green validation for the live `UpdateWorkOrderLine` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` no longer owns the remaining Work Order Line write mutation path, but it still owns line queries, header queries, and requisition paths.

### Task 1: Add Focused Tests For UpdateWorkOrderLine Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderLineHandlerTests.cs`

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
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Tests.Features.WorkOrders;

public class UpdateWorkOrderLineHandlerTests
{
    [Fact]
    public async Task Handle_WhenRecordHasChanges_UpdatesLineAndCommits() { }

    [Fact]
    public async Task Handle_WhenNoChangesDetected_ReturnsSuccessWithoutUpdateOrCommit() { }

    [Fact]
    public async Task Handle_WhenRecordWasModifiedAfterDto_ReturnsConcurrencyError() { }

    [Fact]
    public async Task UpdateWorkOrderLine_WhenCalled_DelegatesToHandler() { }
}
```

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~UpdateWorkOrderLineHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement UpdateWorkOrderLine Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/UpdateWorkOrderLine/IUpdateWorkOrderLineHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/UpdateWorkOrderLine/UpdateWorkOrderLineHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`

- [ ] **Step 1: Add the handler contract**
- [ ] **Step 2: Implement the minimal handler**
- [ ] **Step 3: Register and delegate the handler**

### Task 3: Validate The Slice

**Files:**
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderHeaderWithLinesHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/AddWorkOrderLineHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/UpdateWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderHeaderHandlerTests.cs`
- Modify: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/DeleteWorkOrderLineHandlerTests.cs`

- [ ] **Step 1: Update existing delegation tests only as required for the expanded constructor**
- [ ] **Step 2: Run the focused update-line tests**
- [ ] **Step 3: Run the combined focused Work Orders handler suite**
- [ ] **Step 4: Check touched-file diagnostics**
- [ ] **Step 5: Run a no-restore solution build**
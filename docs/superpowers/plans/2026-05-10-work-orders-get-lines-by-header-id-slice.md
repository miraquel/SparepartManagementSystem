# Work Orders Get Lines By Header Id Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders query-path migration by moving `GetWorkOrderLineByWorkOrderHeaderId` into a Work Orders-owned seam and further reducing query ownership in the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `GetWorkOrderLineByWorkOrderHeaderId` path moves into a dedicated feature handler under Work Orders. The slice preserves the current repository read, list mapping, success message, and structured error response while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetWorkOrderLineByWorkOrderHeaderId`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `GetWorkOrderLineByWorkOrderHeaderId` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetWorkOrderLineByWorkOrderHeaderId`, matching the existing Work Orders read/write transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the remaining Work Order Line query path from the legacy service shell.
- **What the next slice should remove:** the first Work Order Header query path, unless the repository baseline fix becomes more urgent.

## Implemented Scope

- [x] Add focused tests for the `GetWorkOrderLineByWorkOrderHeaderId` handler seam.
- [x] Add `IGetWorkOrderLineByWorkOrderHeaderIdHandler`.
- [x] Implement `GetWorkOrderLineByWorkOrderHeaderIdHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetWorkOrderLineByWorkOrderHeaderId` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `GetWorkOrderLineByWorkOrderHeaderId` handler seam.
- Focused red-green validation for the live `GetWorkOrderLineByWorkOrderHeaderId` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` no longer owns the remaining Work Order Line query path, but it still owns header queries and requisition paths.

### Task 1: Add Focused Tests For GetWorkOrderLineByWorkOrderHeaderId Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/GetWorkOrderLineByWorkOrderHeaderIdHandlerTests.cs`

- [ ] **Step 1: Write the failing tests for the new handler and delegation path**

Run focused tests that cover:
- successful repository read and DTO list mapping
- delegation through `WorkOrderService`
- structured error response when the repository throws

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~GetWorkOrderLineByWorkOrderHeaderIdHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement GetWorkOrderLineByWorkOrderHeaderId Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/GetWorkOrderLineByWorkOrderHeaderId/IGetWorkOrderLineByWorkOrderHeaderIdHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/GetWorkOrderLineByWorkOrderHeaderId/GetWorkOrderLineByWorkOrderHeaderIdHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`

- [ ] **Step 1: Add the handler contract**
- [ ] **Step 2: Implement the minimal handler**
- [ ] **Step 3: Register and delegate the handler**

### Task 3: Validate The Slice

**Files:**
- Modify: existing Work Orders handler test files as needed for constructor injection

- [ ] **Step 1: Update existing delegation tests only as required for the expanded constructor**
- [ ] **Step 2: Run the focused get-lines-by-header-id tests**
- [ ] **Step 3: Run the combined focused Work Orders handler suite**
- [ ] **Step 4: Check touched-file diagnostics**
- [ ] **Step 5: Run a no-restore solution build**
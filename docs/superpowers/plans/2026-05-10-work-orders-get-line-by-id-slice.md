# Work Orders Get Line By Id Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders query-path migration by moving `GetWorkOrderLineById` into a Work Orders-owned seam and reducing query ownership in the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `GetWorkOrderLineById` path moves into a dedicated feature handler under Work Orders. The slice preserves the current repository read, DTO mapping, and structured error response while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetWorkOrderLineById`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `GetWorkOrderLineById` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetWorkOrderLineById`, matching the existing write-side transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the smallest remaining Work Order Line query path from the legacy service shell.
- **What the next slice should remove:** `GetWorkOrderLineByWorkOrderHeaderId`, so the remaining Work Order Line query ownership moves out of the legacy shell before header-query extraction.

## Implemented Scope

- [x] Add focused tests for the `GetWorkOrderLineById` handler seam.
- [x] Add `IGetWorkOrderLineByIdHandler`.
- [x] Implement `GetWorkOrderLineByIdHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetWorkOrderLineById` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and full solution build.

## Validation Performed

- Focused red-green validation for the new `GetWorkOrderLineById` handler seam.
- Focused red-green validation for the live `GetWorkOrderLineById` delegation path.
- Combined focused Work Orders handler tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

After this slice, `WorkOrderService` no longer owns the single-line Work Order query path, but it still owns `GetWorkOrderLineByWorkOrderHeaderId`, header queries, and requisition paths.

### Task 1: Add Focused Tests For GetWorkOrderLineById Handler

**Files:**
- Create: `SparepartManagementSystem.Service.Tests/Features/WorkOrders/GetWorkOrderLineByIdHandlerTests.cs`

- [ ] **Step 1: Write the failing tests for the new handler and delegation path**

Run focused tests that cover:
- successful repository read and DTO mapping
- delegation through `WorkOrderService`
- rollback-free structured error response when the repository throws

- [ ] **Step 2: Run the focused tests to verify the missing handler fails the build**

Run: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --filter FullyQualifiedName~GetWorkOrderLineByIdHandlerTests`
Expected: FAIL because the new handler types do not exist yet.

### Task 2: Implement GetWorkOrderLineById Feature Handler

**Files:**
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/GetWorkOrderLineById/IGetWorkOrderLineByIdHandler.cs`
- Create: `SparepartManagementSystem.Service/Features/WorkOrders/GetWorkOrderLineById/GetWorkOrderLineByIdHandler.cs`
- Modify: `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`
- Modify: `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`

- [ ] **Step 1: Add the handler contract**
- [ ] **Step 2: Implement the minimal handler**
- [ ] **Step 3: Register and delegate the handler**

### Task 3: Validate The Slice

**Files:**
- Modify: existing Work Orders handler test files as needed for constructor injection

- [ ] **Step 1: Update existing delegation tests only as required for the expanded constructor**
- [ ] **Step 2: Run the focused get-line-by-id tests**
- [ ] **Step 3: Run the combined focused Work Orders handler suite**
- [ ] **Step 4: Check touched-file diagnostics**
- [ ] **Step 5: Run a no-restore solution build**
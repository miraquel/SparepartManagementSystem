# Work Orders Get Header By Id Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented and validated in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders query-path migration by moving `GetWorkOrderHeaderById` into a Work Orders-owned seam and reducing header-query ownership in the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `GetWorkOrderHeaderById` path moves into a dedicated feature handler under Work Orders. The slice preserves the current repository read, DTO mapping, and structured error response while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetWorkOrderHeaderById`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `GetWorkOrderHeaderById` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetWorkOrderHeaderById`, matching the existing Work Orders query transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the narrowest remaining header query path from the legacy service shell.
- **What the next slice should remove:** `GetWorkOrderHeaderByIdWithLines`, so the read-only single-header ownership is gone before paged header query extraction.

## Planned Scope

- [x] Add focused tests for the `GetWorkOrderHeaderById` handler seam.
- [x] Add `IGetWorkOrderHeaderByIdHandler`.
- [x] Implement `GetWorkOrderHeaderByIdHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetWorkOrderHeaderById` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `GetWorkOrderHeaderByIdHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 32/32.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Solution tests: `dotnet test .\SparepartManagementSystem.sln --no-build` passed 156/156.
- Vulnerability gate: `dotnet list .\SparepartManagementSystem.sln package --vulnerable --include-transitive` is clean.
# Work Orders Get Header By Params Paged List Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented; full repository-backed validation remains blocked by local Docker availability in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders query-path migration by moving `GetWorkOrderHeaderByParamsPagedList` into a Work Orders-owned seam and reducing the remaining header-query ownership in the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `GetWorkOrderHeaderByParamsPagedList` path moves into a dedicated feature handler under Work Orders. The slice preserves the current repository read, paged DTO mapping, parameter passthrough, and structured error response while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetWorkOrderHeaderByParamsPagedList`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `GetWorkOrderHeaderByParamsPagedList` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetWorkOrderHeaderByParamsPagedList`, matching the existing Work Orders query transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the last header paged-query path from the legacy service shell.
- **What the next slice should remove:** the next remaining query path outside Work Orders or the first Item Requisition seam, depending on which owning abstraction you want to shrink next.

## Planned Scope

- [x] Add focused tests for the `GetWorkOrderHeaderByParamsPagedList` handler seam.
- [x] Add `IGetWorkOrderHeaderByParamsPagedListHandler`.
- [x] Implement `GetWorkOrderHeaderByParamsPagedListHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetWorkOrderHeaderByParamsPagedList` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `GetWorkOrderHeaderByParamsPagedListHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 41/41.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Full repository-backed solution tests remain blocked by local Docker availability from the current environment, so the last missing gate is still `dotnet test .\SparepartManagementSystem.sln --no-build` once Docker is back.
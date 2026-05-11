# Work Orders Get All Header Paged List Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented; full repository-backed validation is currently blocked by local Docker availability in branch `work-orders-first-slice`.

**Goal:** Continue the Work Orders query-path migration by moving `GetAllWorkOrderHeaderPagedList` into a Work Orders-owned seam and reducing remaining header-query ownership in the legacy `WorkOrderService`.

**Architecture:** This is a transitional Work Orders slice. The public API and `IWorkOrderService` stay stable, but the live `GetAllWorkOrderHeaderPagedList` path moves into a dedicated feature handler under Work Orders. The slice preserves the current repository read, paged DTO mapping, informational logging, and structured error response while shrinking `WorkOrderService` further into a compatibility facade.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetAllWorkOrderHeaderPagedList`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns the live `GetAllWorkOrderHeaderPagedList` behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetAllWorkOrderHeaderPagedList`, matching the existing Work Orders query transition pattern.
- **Why that is acceptable:** It preserves the external contract while removing the simplest remaining paged header read path from the legacy service shell.
- **What the next slice should remove:** `GetWorkOrderHeaderByParamsPagedList`, which shares the same paged response shape and should follow immediately after this extraction.

## Planned Scope

- [x] Add focused tests for the `GetAllWorkOrderHeaderPagedList` handler seam.
- [x] Add `IGetAllWorkOrderHeaderPagedListHandler`.
- [x] Implement `GetAllWorkOrderHeaderPagedListHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetAllWorkOrderHeaderPagedList` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `GetAllWorkOrderHeaderPagedListHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 38/38.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Service tests: `dotnet test .\SparepartManagementSystem.Service.Tests\SparepartManagementSystem.Service.Tests.csproj --no-build` passed 38/38.
- Full solution repository-backed tests are currently blocked by local Docker availability:
	- `docker version` cannot reach the daemon.
	- `Test-Path \\.\pipe\docker_engine` returned `False`.
	- `com.docker.service` is `Stopped`, and `Start-Service com.docker.service` failed due to insufficient permissions.
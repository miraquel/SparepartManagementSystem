# Work Orders Delete Item Requisition Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented and validated as a transitional slice in branch `v2`.

**Goal:** Continue shrinking the remaining Item Requisition behavior inside the legacy `WorkOrderService` by extracting `DeleteItemRequisition` into a Work Orders-owned seam while preserving the current API and service contract.

**Architecture:** Item Requisition remains inside the Work Orders module per the governing architecture spec, so this slice stays under `Features/WorkOrders`. The public controller and `IWorkOrderService` contract remain stable, but the live `DeleteItemRequisition` behavior moves behind a dedicated module-owned handler to reduce legacy service ownership.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `DeleteItemRequisition`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` still owns the remaining Item Requisition write behavior for add, update, and delete.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `DeleteItemRequisition` while the Item Requisition add and update paths stay inline for now.
- **Why that is acceptable:** It removes the narrowest remaining Item Requisition write path without widening scope into the mutation-heavy add or update flows.
- **What the next slice should remove:** `AddItemRequisition` or `UpdateItemRequisition`, depending on whether the next priority is lower-risk deletion parity or broader write-path cleanup.

## Planned Scope

- [x] Add focused tests for the `DeleteItemRequisition` handler seam.
- [x] Add `IDeleteItemRequisitionHandler`.
- [x] Implement `DeleteItemRequisitionHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.DeleteItemRequisition` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `DeleteItemRequisitionHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 53/53.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Full solution tests: `dotnet test .\SparepartManagementSystem.sln --no-build` passed 177/177 with Docker-backed repository tests available.
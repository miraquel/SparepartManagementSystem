# Work Orders Get Item Requisition By Id Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented and validated as a transitional slice in branch `v2`.

**Goal:** Start shrinking the remaining Item Requisition behavior inside the legacy `WorkOrderService` by extracting `GetItemRequisitionById` into a Work Orders-owned seam while preserving the current API and service contract.

**Architecture:** Item Requisition belongs to the Work Orders module per the governing architecture spec, so this slice stays under `Features/WorkOrders`. The public controller and `IWorkOrderService` contract remain stable, but the live `GetItemRequisitionById` behavior moves behind a dedicated module-owned handler to reduce legacy service ownership.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetItemRequisitionById`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` owns all Item Requisition behavior.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetItemRequisitionById` while the rest of Item Requisition behavior stays inline for now.
- **Why that is acceptable:** It moves the first Item Requisition query path under Work Orders ownership without widening scope across all remaining Item Requisition methods.
- **What the next slice should remove:** another small Item Requisition seam, preferably an adjacent query or the narrowest write path.

## Planned Scope

- [x] Add focused tests for the `GetItemRequisitionById` handler seam.
- [x] Add `IGetItemRequisitionByIdHandler`.
- [x] Implement `GetItemRequisitionByIdHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetItemRequisitionById` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `GetItemRequisitionByIdHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 44/44.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Full solution tests: `dotnet test .\SparepartManagementSystem.sln --no-build` passed 168/168 with Docker-backed repository tests available.
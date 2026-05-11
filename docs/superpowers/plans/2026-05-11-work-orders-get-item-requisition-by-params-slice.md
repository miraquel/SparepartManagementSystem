# Work Orders Get Item Requisition By Params Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented and validated as a transitional slice in branch `v2`.

**Goal:** Continue shrinking the remaining Item Requisition behavior inside the legacy `WorkOrderService` by extracting `GetItemRequisitionByParams` into a Work Orders-owned seam while preserving the current API and service contract.

**Architecture:** Item Requisition remains inside the Work Orders module per the governing architecture spec, so this slice stays under `Features/WorkOrders`. The public controller and `IWorkOrderService` contract remain stable, but the live `GetItemRequisitionByParams` behavior moves behind a dedicated module-owned handler to reduce legacy service ownership.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `GetItemRequisitionByParams`
- **Legacy abstraction still authoritative before this slice:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` still owns the remaining Item Requisition behavior except the already extracted `GetItemRequisitionById` and `GetItemRequisitionByWorkOrderLineId` paths.
- **Legacy abstraction after this slice:** `WorkOrderService` becomes a compatibility facade for `GetItemRequisitionByParams` while the Item Requisition write paths stay inline for now.
- **Why that is acceptable:** It extracts the last Item Requisition read query from the legacy service without widening scope into the write paths.
- **What the next slice should remove:** the narrowest remaining Item Requisition write path, starting with `DeleteItemRequisition` or `AddItemRequisition` depending on desired risk profile.

## Planned Scope

- [x] Add focused tests for the `GetItemRequisitionByParams` handler seam.
- [x] Add `IGetItemRequisitionByParamsHandler`.
- [x] Implement `GetItemRequisitionByParamsHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.GetItemRequisitionByParams` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Adjust existing Work Orders delegation tests only as required for the expanded `WorkOrderService` constructor.
- [x] Verify the slice with focused tests, touched-file diagnostics, and a solution build.

## Validation Result

- Focused tests: `GetItemRequisitionByParamsHandlerTests` passed 3/3.
- Work Orders suite: `FullyQualifiedName~Features.WorkOrders` passed 50/50.
- Solution build: `dotnet build .\SparepartManagementSystem.sln --no-restore` succeeded.
- Full solution tests: `dotnet test .\SparepartManagementSystem.sln --no-build` passed 174/174 with Docker-backed repository tests available.
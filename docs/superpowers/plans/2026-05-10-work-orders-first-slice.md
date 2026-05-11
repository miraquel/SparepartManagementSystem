# Work Orders First Slice Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Governing Specs:**
- `docs/superpowers/specs/2026-05-10-feature-boundary-modular-monolith.md`
- `docs/superpowers/specs/2026-05-10-superpowers-refactor-operating-model.md`

**Status:** Implemented as a transitional slice in branch `work-orders-first-slice`.

**Goal:** Start the modular-monolith migration by extracting `AddWorkOrderHeaderWithLines` into a Work Orders-owned seam with test coverage while keeping the current API contract stable.

**Architecture:** This is a transitional Work Orders slice, not the final module boundary. The live path is delegated through the legacy `WorkOrderService`, but ownership of this use case has started moving under Work Orders feature code. Future slices must continue shrinking the legacy service until Work Orders behavior is owned at the module boundary.

**Tech Stack:** ASP.NET Core, xUnit, Moq, Dapper/MySQL-compatible existing repositories, Mapperly

---

## Transitional Decision Log

- **Current module:** Work Orders
- **Current extracted seam:** `AddWorkOrderHeaderWithLines`
- **Legacy abstraction still authoritative:** `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs` remains the compatibility facade for the public service contract.
- **Why that is acceptable right now:** It keeps the API stable while the first Work Orders seam is extracted and verified.
- **What the next slice must do:** Move adjacent Work Orders behavior under module ownership and continue reducing `WorkOrderService` until it is a thin adapter or removable.

## Implemented Scope

- [x] Add focused tests for the new Work Orders seam.
- [x] Add `IAddWorkOrderHeaderWithLinesHandler`.
- [x] Implement `AddWorkOrderHeaderWithLinesHandler`.
- [x] Register the handler in DI.
- [x] Delegate `WorkOrderService.AddWorkOrderHeaderWithLines` to the new handler.
- [x] Keep the API contract stable by leaving controller and `IWorkOrderService` unchanged.
- [x] Verify the slice with focused tests and full solution build.

## Validation Performed

- Focused red-green validation for the handler seam.
- Focused red-green validation for the live delegation path.
- Focused Work Orders slice tests passing.
- Touched-file diagnostics clean.
- Solution build passing.

## Remaining Legacy Ownership

The following still reflect the old layered shape and should be reduced in future Work Orders slices:
- `SparepartManagementSystem.Service/Implementation/WorkOrderService.cs`
- `SparepartManagementSystem.Service/ServiceCollectionExtension.cs`
- broad `IUnitOfWork` and repository usage in the Work Orders path

## Next Slice Intent

The next Work Orders slice should not be framed as another isolated handler extraction without context. It should explicitly state how it moves Work Orders closer to full feature ownership and which legacy responsibility it removes or shrinks.
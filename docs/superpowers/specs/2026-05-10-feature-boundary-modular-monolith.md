# Feature-Boundary Modular Monolith

**Status:** Canonical architecture spec

**Purpose:** Define the target architecture for the Sparepart Management System rearchitecture so future slices optimize for feature/module boundaries instead of reinforcing the current horizontal Service/Repository layering.

## Goal

Rebuild the current layered monolith into a single deployable modular monolith whose primary boundary is business feature. The intended modules are cohesive business areas such as Identity and Access, Work Orders, Goods Receipt and Inventory, Reference Data, External Integration, and Release Tracking.

## Core Rules

### 1. Feature Owns the Boundary

The primary architectural boundary is the feature/module, not the technical layer.

Each module should own:
- its commands and queries
- its transport contracts
- its application handlers
- its tests
- its persistence ports
- its external integration ports and adapters

Shared abstractions must stay minimal. A shared utility is acceptable. A second horizontal architecture is not.

### 2. Transitional Seams Are Allowed, But They Are Not the Target

During migration, a slice may introduce a smaller seam inside a feature, such as a single handler or a small cluster of use cases. That is a transitional move, not the final boundary.

Legacy services, repositories, and DTOs may remain only as facades or adapters while a slice is in flight. They must not be treated as the permanent owner of the behavior once the module-owned seam exists.

### 3. Module Dependencies Must Stay Explicit

The API host may depend on module contracts, endpoint composition, authentication, and cross-cutting infrastructure setup.

Modules may depend on:
- shared platform primitives
- carefully limited shared helpers
- contracts of other modules only when the dependency is intentionally designed

Modules must not depend on each other through concrete infrastructure or reach across boundaries into another module's persistence or implementation details.

### 4. Persistence Is Hybrid by Design

The target persistence model is hybrid:
- EF Core owns transactional write-side aggregates, concurrency, and write-side unit of work concerns.
- Dapper remains acceptable for read-heavy projections, search, paging, and explicit SQL where it is clearer than ORM mapping.

The current broad repository factory and god-style `IUnitOfWork` are migration baggage, not target architecture.

### 5. Authorization Is Not a Service-Layer Concern

Authorization belongs in policies, handlers, or equivalent infrastructure that can evaluate permissions without forcing controllers or filters to call application services as an ownership shortcut.

Service-calling action filters are explicitly out of bounds for the target design.

### 6. External Systems Are Anti-Corruption Boundaries

External systems such as GMK must be represented behind explicit ports and adapters. A module can consume an external capability, but it should not directly inherit the transport shape or lifecycle management concerns of the external system.

### 7. Tests Follow Module Boundaries

Tests should primarily prove behavior at the feature/module boundary:
- handler and use-case tests
- contract tests
- authorization tests
- integration adapter tests
- focused end-to-end coverage for critical flows

Repository-plumbing-heavy tests should shrink over time and survive only where the retained Dapper read model genuinely needs them.

## Module Intent

### Identity and Access

Owns authentication, authorization, roles, permissions, refresh tokens, and current-user context.

### Work Orders

Owns work order headers, work order lines, item requisitions, and the use cases that manipulate them.

### Goods Receipt and Inventory

Owns goods receipt behavior and inventory-oriented local use cases.

### Reference Data

Owns stable lookup-like business data such as number sequences and other shared reference concepts that are business-level, not technical.

### External Integration

Owns anti-corruption adapters and external transport boundaries, especially GMK integration.

### Release Tracking

Owns version tracker behavior and release-related domain concerns.

## Interpretation of the Current Work Orders Slice

The `AddWorkOrderHeaderWithLines` extraction under `SparepartManagementSystem.Service/Features/WorkOrders/AddWorkOrderHeaderWithLines/` is a valid first seam, but it is not the final Work Orders module design.

Its current meaning is:
- the behavior has started moving under Work Orders ownership
- the legacy `WorkOrderService` is now a compatibility facade for this use case
- future slices must continue shrinking the legacy service until Work Orders behavior is owned by the feature/module boundary, not by the old layer shell

The existence of a handler folder must never be interpreted as "handler-level folders are the long-term architecture." The long-term architecture is module ownership.

## Exit Criteria For A Migrated Module

A module is considered migrated only when:
- its primary use cases are owned by module code, not the old service layer
- legacy services are facade-only or removed
- the module no longer depends on the old horizontal abstractions as its main organizational boundary
- its verification strategy is primarily module-oriented

## Out Of Scope

This spec does not require:
- microservices
- a separate deployable per module
- preserving old DTO or service boundaries for their own sake
- a full rewrite in one step

It does require architectural progress toward module ownership on every accepted modernization slice.
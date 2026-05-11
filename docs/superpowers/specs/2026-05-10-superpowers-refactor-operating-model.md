# Superpowers Refactor Operating Model

**Status:** Canonical execution spec

**Purpose:** Define how Superpowers-driven modernization work is prepared, executed, reviewed, and verified so future slices stay aligned with the intended architecture instead of becoming ad hoc refactors.

## Goal

Make every modernization slice explicit, reviewable, incremental, and reversible enough to keep architectural direction stable while real code keeps moving.

## Operating Rules

### 1. Spec Before Plan Before Code

Every modernization thread follows this order:
1. validate or update the architecture spec
2. write or revise the implementation plan
3. implement one slice
4. verify the slice

Do not begin by editing production code when the intended boundary or migration posture is still ambiguous.

### 2. Business-Scoped Slices Only

Each slice must be explainable as one of the following:
- one use case inside one module
- one small cohesive set of adjacent use cases inside one module
- one cross-cutting infrastructure concern that is directly required by module-boundary migration

Slices must not be framed as "refactor services" or "clean repositories" without a business-boundary explanation.

### 3. Transitional Slices Must Say So Explicitly

If a slice introduces a feature-owned seam but leaves the old service or repository layer as the true owner, the slice must be labeled transitional.

Each transitional slice must declare:
- what legacy abstraction is still authoritative
- why that temporary state is acceptable
- what next step removes or shrinks that legacy ownership

### 4. Extraction Order Is Fixed

For most migrations, the preferred order is:
1. create the new module-owned seam
2. prove it with focused tests
3. delegate the live path to it
4. migrate adjacent behavior into the module-owned seam
5. delete the obsolete legacy compatibility layer

This prevents broad rewrites without anchored behavior checks.

### 5. TDD Is Required For New Seams

Every new use-case seam or behavior change uses TDD:
- write the failing test
- observe the failure
- add minimal implementation
- verify the pass

Where the live delegation path changes, capture red-green evidence for the delegation as well, not just for the new handler in isolation.

### 6. Review Order Is Fixed

When using Superpowers execution flow, reviews happen in this order:
1. spec compliance review
2. code-quality review

Do not move to code-quality review when the slice still has spec drift.

### 7. Verification Minimum

Each slice must end with fresh evidence. Minimum required verification is:
- focused slice tests passing
- touched-file diagnostics clean
- solution build passing

If broader tests are already red for unrelated reasons, that baseline issue must be stated explicitly and narrower validation must be used honestly.

### 8. Legacy Deletion Is Part Of The Plan, Not An Optional Future Idea

Every slice plan must name:
- which legacy code became facade-only
- which future slice removes it
- what condition proves it is safe to remove

### 9. Documentation Responsibilities

If the target architecture changes, update the architecture spec first.

If the execution workflow changes, update this operating model spec.

If a slice is intentionally transitional, record the decision in the plan for that slice.

## Required Superpowers Skill Flow

Use this workflow when operating on modernization work:

### Architecture And Rearchitecture Discussion

Use `brainstorming` first to define or revise the design intent.

### Multi-Step Planning

Use `writing-plans` after the spec is approved.

### Implementation Execution

Use `subagent-driven-development` when the slice can be executed and reviewed incrementally in the current session.

Use `executing-plans` only when the chosen execution style genuinely fits that workflow better.

### Behavior Change Discipline

Use `test-driven-development` for each new seam or behavior change.

### Isolation

Use `using-git-worktrees` before starting a production-code slice unless the environment is already isolated.

### Completion Gate

Use `verification-before-completion` before claiming the slice is done.

## What This Operating Model Forbids

This operating model forbids:
- starting a modernization slice without checking the spec
- broad layer cleanup that is not tied to module progress
- calling a transitional extraction the final architecture
- skipping red-state proof for new seams
- claiming completion without fresh verification evidence

## Current Example

The Work Orders `AddWorkOrderHeaderWithLines` extraction is the reference example for a valid transitional slice:
- new feature-owned seam created
- focused tests added
- live path delegated to the seam
- legacy service retained only as compatibility shell for now

That is valid because it is explicitly transitional. Repeating that pattern without documenting the next deletion step would be drift.
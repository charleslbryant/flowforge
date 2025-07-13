# Architecture Decision Records (ADRs)

## Purpose
Architecture Decision Records (ADRs) document significant architectural decisions made during the development of a project. Each ADR captures the context, decision, consequences, and rationale for important technical choices that shape the system's architecture.

## Structure
- Each ADR is a standalone markdown file in this directory.
- ADRs are numbered sequentially with 4 digits (adr-0001, adr-0002, etc.).
- Each ADR follows a consistent template format.
- ADRs are immutable once accepted - new decisions create new ADRs.

## Usage
- Use the ADR template to document new architectural decisions.
- Reference ADRs in code comments, documentation, and discussions.
- Review ADRs when considering architectural changes.
- Update ADRs only to add new decisions, never to modify existing ones.

## ADR Status
- **Proposed**: Under consideration
- **Accepted**: Decision made and implemented
- **Deprecated**: Superseded by newer decision
- **Superseded**: Replaced by another ADR

## Current ADRs
- [ADR-0001: .NET Port of FlowForge CLI](adr-0001-dotnet-port-decision.md)
- [ADR-0002: Test-Driven Development Approach](adr-0002-test-driven-development-approach.md)
- [ADR-0003: Spectre.Console for Rich Console Output](adr-0003-spectre-console-adoption.md)
- [ADR-0004: Health Command Development Journey](adr-0004-health-command-development.md)
- [ADR-0005: Doctor Command Development Journey](adr-0005-doctor-command-development.md)
- [ADR-0006: Start Command Development Journey](adr-0006-start-command-development.md)
- [ADR-0007: Services and Infrastructure Separation](adr-0007-services-infrastructure-separation.md)
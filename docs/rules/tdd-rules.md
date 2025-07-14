# TDD Rules

This file defines the test-driven development (TDD) expectations for all work completed in `/dev` mode.

## Purpose
To ensure consistent, high-quality, test-first development that improves system design, confidence, and maintainability.

---

## 🔁 TDD Cycle (Red → Green → Refactor)

### 1. **Write a Failing Test (Red)**
- Write a minimal test that describes the next behavior to implement
- Confirm that the test fails with a clear, expected error
- Don’t write multiple tests or implementation code yet

### 2. **Write Minimum Code to Pass (Green)**
- Only write the **smallest amount of code** to make the test pass
- Avoid feature creep or premature optimization
- Return hardcoded values if needed — clarity over completeness

### 3. **Refactor**
- Clean up both implementation and test code
- Remove duplication, simplify logic, rename for clarity
- Maintain all tests green throughout

---

## ✅ Rules for Assistants
- Always follow the TDD loop in sequence — never skip red
- Never write implementation code **before** a failing test exists
- Every new function, method, or behavior must be test-backed
- Use `dotnet test` frequently to verify state
- Add new tests before adding new code blocks
- Check test coverage against CRD acceptance criteria

---

## ⚠️ Anti-Patterns to Avoid
- Writing code before tests
- Testing multiple behaviors in a single test
- Leaving failing tests without fix
- Adding tests **after** implementation is done
- Skipping refactor step due to time pressure

---

## Test Framework
- Use `xUnit` for all unit tests
- Use `Moq` for mocking interfaces
- Use `dotnet test --watch` or `--verbosity minimal` for fast feedback

---

## Output Format
Ensure all tests are committed and executable with:
```bash
cd dotnet
DOTNET_ENV=Test dotnet test --no-build
```

---

## References
- [Red-Green-Refactor Summary](https://en.wikipedia.org/wiki/Test-driven_development)
- [xUnit Docs](https://xunit.net/)

---
*TDD compliance is required in all `/dev` mode work. This rule file is automatically loaded into the assistant's context at the start of that mode.*

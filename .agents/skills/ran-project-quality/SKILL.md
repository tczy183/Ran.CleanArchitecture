---
name: ran-project-quality
description: Validate changes in Ran.CleanArchitecture with scoped formatting, restore, build, test, and documentation checks. Use after code or project-file changes, for CI failures, or when asked to verify repository health; do not use to implement unrelated fixes automatically.
---

# Ran Project Quality

Choose the smallest checks that give meaningful confidence, then broaden them when the change crosses project boundaries.

## Baseline facts

- Solution: `Ran.CleanArchitecture.slnx`
- Default target framework: `net10.0`; a project may override it with `TargetFrameworks`.
- Nullable, latest analyzers, code style in build, and warnings-as-errors are enabled in `Directory.Build.props`.
- NuGet versions are centrally managed by `Directory.Packages.props`.
- Local tools are declared in `.config/dotnet-tools.json`: Husky and CSharpier.
- Tests may exist under `framework/test`, `modules/*/test`, and `services/test`; discover them instead of assuming coverage.

## Validation sequence

1. Inspect changed files and map them to affected projects and downstream project references.
2. If formatting is relevant, restore local tools if needed and run `dotnet csharpier check` on the narrowest supported path. Only run a formatter that modifies files when the task authorizes edits.
3. Run `dotnet restore Ran.CleanArchitecture.slnx` when dependencies, project files, or lock state changed, or when assets are unavailable.
4. Build the smallest affected `.csproj` first. For cross-cutting or final verification, run `dotnet build Ran.CleanArchitecture.slnx --no-restore` after a successful restore.
5. Discover test projects with `rg --files services framework modules -g '*Tests.csproj' -g '*Test.csproj'`. Run relevant projects with `dotnet test <project> --no-restore`; if none exist, report that tests were not available rather than claiming they passed.
6. For documentation changes, use `$ran-docs` and run its MkDocs validation when the required Python environment is available.
7. Report each check as passed, failed, or not run. Include the first actionable error and distinguish new failures from confirmed pre-existing failures when possible.

## Guardrails

- Do not use `--no-restore` before a successful applicable restore unless existing assets are known to be current.
- Do not weaken analyzers, warning policies, nullable settings, or style rules merely to make a build green.
- Do not edit unrelated warnings or reformat the entire repository without user authorization.
- Do not claim repository-wide success after only a project-scoped build.
- If SDK, workload, package feed, Docker, or Python prerequisites are missing, state the exact limitation and retain results from checks that could run.

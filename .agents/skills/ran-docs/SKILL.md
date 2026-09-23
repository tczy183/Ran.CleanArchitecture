---
name: ran-docs
description: Create or update the Ran.CleanArchitecture MkDocs site, including Chinese and English pages, navigation, links, and the GitHub Pages workflow. Use for files under docs or documentation deployment behavior; do not use for API XML comments alone.
---

# Ran Documentation

Maintain the Material for MkDocs site under `docs` and keep navigation consistent with the content tree.

## Workflow

1. Inspect `docs/mkdocs.yml`, the target page, and its language counterpart before editing.
2. Put shared landing content directly under `docs/content`. Put localized guides under `docs/content/zh` and `docs/content/en` using matching relative paths when both languages are supported.
3. When adding, renaming, or removing a page, update `nav` in `docs/mkdocs.yml` and repair affected relative links.
4. Write actual English content in the English tree and Chinese content in the Chinese tree. Preserve technical identifiers, commands, and API names exactly.
5. Keep examples consistent with the current solution name, .NET target, package structure, and startup flow; verify those facts in source instead of copying stale prose.
6. Validate from `docs` with `mkdocs build --strict` when Python and the configured plugins are available. If prerequisites are absent, inspect navigation targets and Markdown links locally and report that the rendered build was not run.
7. The `.github/workflows/doc.yml` deployment runs on pushes to `main` and rejects the latest commit unless its message contains `docs:`. Mention this only when preparing a deployment or commit; never create or amend a commit unless requested.

## Site dependencies

The workflow installs `mkdocs-material`, `mkdocs-awesome-pages-plugin`, and `mkdocs-git-revision-date-localized-plugin`. Keep configuration within features supported by those packages unless the workflow is updated in the same requested change.

## Boundaries

- Do not mark roadmap work complete unless the implementation exists.
- Do not translate code, paths, configuration keys, or CLI commands.
- Do not alter the publishing trigger or repository permissions as a side effect of an ordinary content edit.
- Keep `README.md` and `README_CN.md` aligned when a change affects the repository overview or contribution instructions, even if the full docs pages contain more detail.

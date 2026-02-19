# Qase C# Reporters

## What This Is

C# репортеры Qase (xUnit, NUnit, Commons) с полной документацией и файловым репортером, соответствующим спецификации Qase Report (`specs/report/`). Поддержка двух режимов: TestOps API и локальный файловый отчёт. FileReporter генерирует directory-based output (run.json + results/ + attachments/) с корректной snake_case сериализацией.

## Core Value

Файловый репортер должен генерировать JSON-отчёты, строго соответствующие спецификации Qase Report — корректная структура (run.json + results/), snake_case именование, правильные типы и enum-значения.

## Requirements

### Validated

- ✓ xUnit README.md с badges, Features, Quick Start, Documentation table, Test Result Statuses — v1.0
- ✓ NUnit README.md с badges, Features, Quick Start, Documentation table, Test Result Statuses — v1.0
- ✓ Commons README.md с NuGet badge, "Used By" links, Configuration Reference — v1.0
- ✓ xUnit docs/usage.md с TOC и 9 секциями (Installation through Comments) — v1.0
- ✓ NUnit docs/usage.md с TOC и 9 секциями, NUnit-специфичные примеры — v1.0
- ✓ xUnit docs/ATTACHMENTS.md с Overview, Basic Usage, Attachment Types, Advanced Scenarios — v1.0
- ✓ NUnit docs/ATTACHMENTS.md с NUnit синтаксисом — v1.0
- ✓ xUnit docs/STEPS.md с Overview, Basic Steps, Nested Steps, Step Status — v1.0
- ✓ NUnit docs/STEPS.md с NUnit синтаксисом — v1.0
- ✓ Root README.md с навигационными таблицами (Reporters, Libraries, API Clients) — v1.0
- ✓ Терминология стандартизирована ("Qase TestOps" everywhere, не "Qase TMS") — v1.0
- ✓ Все внутренние ссылки валидны (относительные пути, якоря в kebab-case) — v1.0
- ✓ Все блоки кода с указанием языка (csharp, json, bash, xml) — v1.0
- ✓ Installation секции с dotnet CLI и PackageReference вариантами — v1.0
- ✓ FileReporter: корневая структура run.json (title, execution, stats, results, threads, suites, environment, host_data) — v1.1
- ✓ FileReporter: отдельные файлы results/<id>.json для каждого теста — v1.1
- ✓ Сериализация: snake_case для всех JSON-свойств (кастомный JsonNamingPolicy) — v1.1
- ✓ Сериализация: enum как строка в lowercase (passed, failed, skipped, blocked, invalid) — v1.1
- ✓ Модели: StepType в StepResult (default "text") — v1.1
- ✓ Модели: JsonIgnore для RunId, Ignore, Comment, ContentBytes — v1.1
- ✓ Модели: StepExecution.StartTime nullable — v1.1

### Active

#### Current Milestone: v1.2 Examples Showcase

**Goal:** Переписать примеры в `examples/` для обоих фреймворков (xUnit, NUnit) — showcase всех возможностей репортера в реалистичных тестовых сценариях с реальными файлами аттачментов.

**Target features:**
- Showcase всех атрибутов репортера (QaseIds, Title, Fields, Suites, Ignore, class-level)
- Демонстрация шагов (базовые, вложенные, с Title, статусы pass/fail)
- Демонстрация аттачментов с реальными файлами (png, log) — файл, несколько файлов, byte[], step-level
- Демонстрация Metadata.Comment()
- Параметризованные тесты (xUnit Theory/InlineData/MemberData, NUnit TestCase/Values)
- Комбинированные сценарии (несколько фич вместе)
- Реалистичные имена и контексты тестов (как в настоящем проекте)

### Out of Scope

- Документация API клиентов (V1, V2) — авто-генерируемая, ручные правки будут перезаписаны
- MSTest / SpecFlow reporters — ещё не реализованы (coming soon)
- Валидационные скрипты (validate-links.js, validate-terminology.js) — не часть C# репозитория
- MULTI_PROJECT.md и UPGRADE.md — deferred, фокус на следующий milestone
- Изменения в TestopsReporter (API reporter) — только FileReporter был в скоупе v1.1
- Смена TargetFramework — остаёмся на netstandard2.0

## Context

**Репозиторий:** `qase-tms/qase-csharp` — монорепозиторий C# интеграций для Qase TestOps.

**Спецификация:** `../specs/report/` — Qase Report specification (root.yaml, result.yaml, models/).

**Current State (after v1.1):**
- FileReporter produces spec-compliant directory output: `{path}/run.json` + `{path}/results/<id>.json` + `{path}/attachments/`
- Custom serialization: SnakeCaseNamingPolicy + LowercaseEnumConverter (netstandard2.0)
- Report models: Run, RunStats, RunExecution, ShortResult in `models/report/`
- Domain models aligned: StepType, nullable StartTime, [JsonIgnore] exclusions
- Attachment handling: file copy, string content, byte content with MIME type detection
- Target: netstandard2.0
- Tech stack: System.Text.Json, Microsoft.Extensions.Logging, Microsoft.Extensions.DependencyInjection

## Constraints

- **Target Framework**: netstandard2.0 — нельзя использовать API доступные только в .NET 6/8+
- **Обратная совместимость**: Домен-модели используются и в TestopsReporter — изменения не должны ломать API-режим
- **Спецификация**: Строго по `specs/report/` — root.yaml, result.yaml, step.yaml, attachment.yaml
- **Сериализация**: System.Text.Json (уже используется, не менять на Newtonsoft)

## Key Decisions

| Decision | Rationale | Outcome |
|----------|-----------|---------|
| Адаптировать шаблон под C#/NuGet | Шаблон написан для npm — NuGet badges, dotnet CLI | ✓ Good |
| Пропустить UPGRADE.md в v1.0 | Нет значимых breaking changes | ✓ Good |
| Обновить Commons README | Configuration Reference полезно | ✓ Good |
| Не трогать API клиенты | Auto-generated docs | ✓ Good |
| shields.io badges | Industry standard | ✓ Good |
| Progressive disclosure | Better info architecture | ✓ Good |
| Table format for root README | Clear monorepo navigation | ✓ Good |
| 5-row NUnit status table | NUnit has Inconclusive | ✓ Good |
| Standardize "Qase TestOps" | Consistent branding | ✓ Good |
| Custom JsonNamingPolicy for netstandard2.0 | Built-in SnakeCaseLower only in .NET 8+ | ✓ Good |
| Serialization scoped to FileReporter | Avoid TestopsReporter impact | ✓ Good |
| Report models separate from domain | FileReporter-specific serialization | ✓ Good |
| Write in completeTestRun not uploadResults | Spec lifecycle compliance | ✓ Good |
| WhenWritingNull for JSON output | Cleaner output, nullable fields omitted | ✓ Good |
| StepType default "text" | Matches Java SDK, always present in JSON | ✓ Good |
| MimeType detection from file extension | Matches Java SDK URLConnection pattern | ✓ Good |

---
*Last updated: 2026-02-19 after v1.2 milestone start*

# Requirements: Qase C# Reporters

**Defined:** 2026-02-19
**Core Value:** Примеры должны демонстрировать все возможности репортера в реалистичных тестовых сценариях с реальными файлами

## v1.2 Requirements

Requirements for Examples Showcase milestone. Each maps to roadmap phases.

### Attributes

- [ ] **ATTR-01**: Пример с `[QaseIds]` — привязка теста к кейсу в Qase
- [ ] **ATTR-02**: Пример с `[Title]` — кастомное имя теста
- [ ] **ATTR-03**: Пример с `[Fields]` — кастомные поля (Priority, Severity, Layer и т.д.)
- [ ] **ATTR-04**: Пример с `[Suites]` — организация тестов в иерархию сьютов
- [ ] **ATTR-05**: Пример с `[Ignore]` — исключение теста из отчёта Qase
- [ ] **ATTR-06**: Пример с class-level атрибутами (`[Fields]`, `[Suites]` на классе)

### Steps

- [ ] **STEP-01**: Базовые шаги (`[Step]` на методах)
- [ ] **STEP-02**: Вложенные шаги (step вызывает step)
- [ ] **STEP-03**: Шаги с `[Title]` для кастомных имён
- [ ] **STEP-04**: Mixed-status шаги (pass + fail в одном тесте)

### Attachments

- [ ] **ATCH-01**: Аттачмент одного файла (реальная картинка)
- [ ] **ATCH-02**: Аттачмент нескольких файлов (картинка + лог)
- [ ] **ATCH-03**: Аттачмент из byte[] (генерация контента в памяти)
- [ ] **ATCH-04**: Аттачмент на уровне шага
- [ ] **ATCH-05**: Реальные файлы (png, log/txt) в директории `testdata/`

### Metadata

- [ ] **META-01**: `Metadata.Comment()` — комментарии к результату теста

### Parameters

- [ ] **PARM-01**: xUnit `[Theory]` с `[InlineData]` в реалистичном сценарии
- [ ] **PARM-02**: xUnit `[Theory]` с `[MemberData]`
- [ ] **PARM-03**: NUnit `[TestCase]` в реалистичном сценарии
- [ ] **PARM-04**: NUnit `[Values]`/`[Range]` параметры

### Combined

- [ ] **COMB-01**: Полный E2E тест — шаги + аттачменты + комментарии + атрибуты
- [ ] **COMB-02**: Data-driven тест с шагами и аттачментами

### Docs

- [ ] **DOCS-01**: Обновить `examples/README.md` с описанием всех примеров
- [ ] **DOCS-02**: Обновить README для xUnit и NUnit примеров

## Future Requirements

Deferred to future release.

### Configuration Examples

- **CONF-01**: Пример qase.config.json для Report mode
- **CONF-02**: Пример qase.config.json для TestOps mode
- **CONF-03**: Пример настройки через environment variables

## Out of Scope

| Feature | Reason |
|---------|--------|
| MSTest / SpecFlow примеры | Reporters ещё не реализованы |
| Конфигурационные примеры | Deferred — фокус на код тестов |
| Примеры CI/CD интеграции | Отдельная тема, не связана с reporter features |
| Async test examples | xUnit/NUnit async support не связан с reporter |

## Traceability

Which phases cover which requirements. Updated during roadmap creation.

| Requirement | Phase | Status |
|-------------|-------|--------|
| ATTR-01 | — | Pending |
| ATTR-02 | — | Pending |
| ATTR-03 | — | Pending |
| ATTR-04 | — | Pending |
| ATTR-05 | — | Pending |
| ATTR-06 | — | Pending |
| STEP-01 | — | Pending |
| STEP-02 | — | Pending |
| STEP-03 | — | Pending |
| STEP-04 | — | Pending |
| ATCH-01 | — | Pending |
| ATCH-02 | — | Pending |
| ATCH-03 | — | Pending |
| ATCH-04 | — | Pending |
| ATCH-05 | — | Pending |
| META-01 | — | Pending |
| PARM-01 | — | Pending |
| PARM-02 | — | Pending |
| PARM-03 | — | Pending |
| PARM-04 | — | Pending |
| COMB-01 | — | Pending |
| COMB-02 | — | Pending |
| DOCS-01 | — | Pending |
| DOCS-02 | — | Pending |

**Coverage:**
- v1.2 requirements: 24 total
- Mapped to phases: 0
- Unmapped: 24 ⚠️

---
*Requirements defined: 2026-02-19*
*Last updated: 2026-02-19 after initial definition*

# Tests

Browser smoke tests for the rendered Quarto site, using
[pytest](https://docs.pytest.org/) and
[Playwright](https://playwright.dev/python/).

`test_home_page.py` re-renders `index.qmd`, serves `dist/` over a local HTTP
server (see `conftest.py`), then loads the home page in a headless browser and
checks that the title, main heading, intro text, and sidebar navigation are
present.

## Running locally

```bash
# One-time: install dependencies and the browser binaries
uv sync
uv run playwright install --with-deps chromium

# Run the tests
uv run pytest

# Watch a real browser run the test
uv run pytest --headed
```

## Viewing results

Every run writes a self-contained HTML summary (via `pytest-html`):

```
test-results/report.html
```

On failure, Playwright also keeps a screenshot and a trace for each failed test
under `test-results/playwright/<test-id>/`. Open the trace in Playwright's
**Trace Viewer** — a local HTML app with the DOM snapshot, console, network, and
a step-by-step timeline:

```bash
uv run playwright show-trace test-results/playwright/*/trace.zip
```

You can also drag `trace.zip` onto <https://trace.playwright.dev> to view it
without a local install.

These artifacts (`--screenshot`, `--tracing`, `--html`) are configured in
`addopts` in [`../pyproject.toml`](../pyproject.toml).

## In CI

The `test` job in
[`../.github/workflows/publish-quarto.yml`](../.github/workflows/publish-quarto.yml)
runs these tests on every push and pull request, and uploads `test-results/`
(including `report.html` and any trace) as the **playwright-report** workflow
artifact. The site is published only if the tests pass.

# CSP (Computer Science Project)

Slides, code demos, and reference notes for lectures delivered in DigiPen's
**CSP-200/300/400** and **CSP-250/350/450** (Computer Science Project) courses, academic years 25/26 and 26/27.

The rendered site is published to GitHub Pages:
**<https://simonguest.github.io/CSP>**

## Contents

| Lecture | Topic | Source |
| --- | --- | --- |
| 00 | Introducing AI Agents | [`src/00/`](src/00/) |
| 01 | Exploring Generative AI Models (Part 1) | [`src/01/`](src/01/) |
| 02 | Exploring Generative AI Models (Part 2) | [`src/02/`](src/02/) |
| 03 | GitHub Best Practices | [`src/03/`](src/03/) |
| 04 | Presentation Skills | [`src/04/`](src/04/) |
| — | Networking Protocols (WebSockets, WebRTC, WebRTC + SFU, HTTP/3) | [`src/networking/`](src/networking/) |

Runnable demo code that accompanies the lectures lives under [`demos/`](demos/);
most demos include their own `README.md` with setup instructions:

- [`demos/00/campus-agent/`](demos/00/campus-agent/) — multi-agent DigiPen campus assistant (Gradio + OpenAI Agents)
- [`demos/00/gradio-helloworld/`](demos/00/gradio-helloworld/) — minimal Gradio app
- [`demos/01/lmstudio-client/`](demos/01/lmstudio-client/) — C# client for a local LM Studio server
- [`demos/02/pbr/`](demos/02/pbr/) — physically based rendering demo (Babylon.js + TypeScript + Vite)

## Repository structure

```
.
├── _quarto.yml            # Quarto website configuration
├── index.qmd              # Site landing page
├── src/                   # Lecture slides (.qmd), images, and resources
├── demos/                 # Standalone, runnable demo projects
├── tests/                 # Playwright smoke tests for the rendered site
├── .devcontainer/         # Dev Container definitions for select demos
├── .github/workflows/     # GitHub Actions (test, render + publish to Pages)
├── pyproject.toml         # Project metadata and Python dependencies
├── uv.lock                # Pinned dependency lockfile (uv)
└── .python-version        # Python version pin (3.13)
```

The build output directory (`dist/`) is generated locally and by CI; it is
listed in [`.gitignore`](.gitignore) and is **not** committed.

## Building the site locally

Prerequisites:

- [Quarto](https://quarto.org/docs/get-started/) 1.7 or later
- Python 3.13 (see [`.python-version`](.python-version))
- [uv](https://docs.astral.sh/uv/) for dependency management (optional but recommended)

```bash
# Install dependencies into a local virtual environment
uv sync

# Live preview with hot reload
uv run quarto preview

# One-off render into dist/
uv run quarto render
```

If you prefer not to use uv, install Quarto directly and run `quarto preview` /
`quarto render` from the repository root.

## Tests

[`tests/`](tests/) holds Playwright browser tests (pytest) that render the site
and check the home page loads correctly. See [`tests/README.md`](tests/README.md).

```bash
uv sync
uv run playwright install --with-deps chromium
uv run pytest
```

Each run writes `test-results/report.html`; failures also keep a Playwright
trace you can open with `uv run playwright show-trace`. Details in
[`tests/README.md`](tests/README.md).

## Publishing

Every push to `main` and every pull request runs the
[`Quarto Publish`](.github/workflows/publish-quarto.yml) GitHub Actions workflow.
The `test` job renders the site and runs the Playwright tests; only if it passes
does the `build-deploy` job (push to `main` only) deploy the rendered site to the
`gh-pages` branch. GitHub Pages serves that branch at
<https://simonguest.github.io/CSP>.

## Secrets

No secrets are required to build the site. Individual demos that call external
APIs (for example, `demos/00/campus-agent/` needs an `OPENAI_API_KEY`) read them
from a local `.env` file that is git-ignored — see each demo's `README.md`.

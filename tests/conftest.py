"""Shared fixtures for the site tests.

Renders the Quarto site once per test session (if it isn't already built) and
serves the ``dist/`` folder over HTTP so Playwright can load real pages.
"""

from __future__ import annotations

import functools
import http.server
import socketserver
import subprocess
import threading
from collections.abc import Iterator
from pathlib import Path

import pytest

REPO_ROOT = Path(__file__).resolve().parents[1]
DIST_DIR = REPO_ROOT / "dist"
RESULTS_DIR = REPO_ROOT / "test-results"


def pytest_configure(config: pytest.Config) -> None:
    # pytest-html and the Playwright artifacts both write here; make sure it
    # exists even on a fully green run.
    RESULTS_DIR.mkdir(exist_ok=True)


@pytest.fixture(scope="session")
def rendered_site() -> Path:
    """(Re)render the home page so the tests always run against current source.

    Only ``index.qmd`` is rendered to keep the loop fast; Quarto still emits the
    full sidebar/navigation from ``_quarto.yml``. CI renders the whole site
    separately before invoking pytest.
    """
    subprocess.run(["quarto", "render", "index.qmd"], cwd=REPO_ROOT, check=True)
    return DIST_DIR


@pytest.fixture(scope="session")
def base_url(rendered_site: Path) -> Iterator[str]:
    """Serve the rendered site on an ephemeral port for the whole session.

    Overrides the ``base_url`` fixture provided by ``pytest-playwright`` so that
    ``page.goto("/")`` resolves against this local server.
    """
    handler = functools.partial(
        http.server.SimpleHTTPRequestHandler, directory=str(rendered_site)
    )
    with socketserver.TCPServer(("127.0.0.1", 0), handler) as httpd:
        host, port = httpd.server_address
        thread = threading.Thread(target=httpd.serve_forever, daemon=True)
        thread.start()
        try:
            yield f"http://{host}:{port}"
        finally:
            httpd.shutdown()
            thread.join()

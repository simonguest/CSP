"""Smoke tests: the Quarto site builds a working home page."""

from __future__ import annotations

import re

from playwright.sync_api import Page, expect


def test_home_page_renders(page: Page) -> None:
    page.goto("/")

    # <title> comes from the site title in _quarto.yml
    expect(page).to_have_title(re.compile(r"CSP \(Computer Science Project\)"))

    # Level-1 heading from index.qmd
    expect(
        page.get_by_role(
            "heading", name="CSP (Computer Science Project)", level=1
        )
    ).to_be_visible()

    # Body copy from index.qmd
    expect(
        page.get_by_text("collection of slides, code, and other resources")
    ).to_be_visible()


def test_sidebar_lists_lectures(page: Page) -> None:
    page.goto("/")

    sidebar = page.locator("#quarto-sidebar")
    expect(sidebar).to_be_visible()
    expect(sidebar.get_by_text("Lecture: GitHub Best Practices")).to_be_visible()
